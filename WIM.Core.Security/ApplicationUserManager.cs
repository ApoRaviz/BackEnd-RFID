using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WIM.Core.Common.Mail;
using WIM.Core.Security.Context;
using WIM.Core.Security.Entity;

namespace WIM.Core.Security
{

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUserStore<ApplicationUser, string>
    {
        public ApplicationUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()));

            // Configure validation logic for usernames
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(2)
                };
            }

            return appUserManager;
        }

        public static ApplicationUser GetUser(string _userId)
        {
            return GetUser(new ApplicationDbContext(), _userId);
        }

        public static ApplicationUser GetUser(ApplicationDbContext db, string _userId)
        {
            ApplicationUser _retVal = null;
            try
            {
                _retVal = db.Users.Where(p => p.Id == _userId)
                    .Include(p => p.Roles).Include(x => x.Roles.Select(r => r.Role.Permissions)).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return _retVal;
        }


        //public static object GetUserAccessProject(string _userId)
        //{
        //    object _retVal = null;
        //    try
        //    {
        //        ApplicationDbContext db = new ApplicationDbContext();
        //        var query = from u in db.Users
        //                    join acc in db.AccessProjecs on u.Id equals acc.UserID
        //                    where u.Id == _userId
        //                    select new
        //                    {
        //                        acc.ProjectIDSys
        //                    };
        //        _retVal = query.ToList();

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return _retVal;
        //}

        public static void PasswordHistoryOverYear(string _userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var phTop = db.PasswordHistory.Where(u => u.UserID == _userId).Take(1).FirstOrDefault();
                if (phTop != null && (DateTime.Now - phTop.CreatedDate).TotalDays >= 365)
                {
                    var phBottom = db.PasswordHistory.Where(u => u.UserID == _userId).OrderByDescending(o => o.ID)
                        .Take(1).FirstOrDefault();
                    IEnumerable<PasswordHistory> phOldList = db.PasswordHistory.Where(u => u.UserID == _userId && u.ID != phBottom.ID).ToList();
                    db.PasswordHistory.RemoveRange(phOldList);
                    db.SaveChanges();
                }
            }
        }

        public static bool PasswordHistoryOver3Month(string _userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var phBottom = db.PasswordHistory.Where(u => u.UserID == _userId).OrderByDescending(o => o.ID).Take(1).FirstOrDefault();
                if (phBottom != null && (phBottom.CreatedDate - DateTime.Now).TotalDays >= 90)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPreviousPassword(string _userId, string password)
        {

            List<PasswordHistory> pass = new List<PasswordHistory>();
            PasswordHasher ph = new PasswordHasher();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    pass = (from p in db.PasswordHistory where p.UserID == _userId select p).ToList();
                }
                PasswordVerificationResult pvResult;
                foreach (var proc in pass)
                {
                    pvResult = ph.VerifyHashedPassword(proc.PasswordHash, password);
                    if (pvResult == PasswordVerificationResult.Success)
                        return true;
                }
            }
            catch (Exception)
            {

            }
            InsertPasswordHistory(_userId, password);
            return false;
        }

        private static Task InsertPasswordHistory(string _userId, string password)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    PasswordHistory passhis = new PasswordHistory();
                    PasswordHasher ph = new PasswordHasher();
                    passhis.UserID = _userId;
                    passhis.PasswordHash = ph.HashPassword(password);
                    passhis.CreatedDate = DateTime.Now;
                    db.PasswordHistory.Add(passhis);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            return Task.FromResult(0);
        }

        public static List<PasswordHistory> DuplicatePassword(string _userId)
        {
            List<PasswordHistory> pass = new List<PasswordHistory>();
            try
            {
                /*_retVal = db.Users.Where(p => p.Id == _userId)
                    .Include(p => p.Roles).Include(x => x.Roles.Select(r => r.Role.Permissions)).FirstOrDefault();*/
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    pass = (from p in db.PasswordHistory select p).ToList();
                }

            }
            catch (Exception)
            {
            }

            return pass;
        }

        public static List<ApplicationRole> GetRoles(ApplicationUser user)
        {
            List<ApplicationRole> roles = new List<ApplicationRole>();
            foreach (var r in user.Roles)
            {
                ApplicationRole role = ApplicationRoleManager.GetRole(r.RoleId);
                roles.Add(role);
            }
            return roles;
        }
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            bool flag = true;
            for (int i = 0; i < a.Length; i++)
            {
                flag &= (a[i] == b[i]);
            }
            return flag;
        }

        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return ByteArraysEqual(buffer3, buffer4);
        }

    }
}
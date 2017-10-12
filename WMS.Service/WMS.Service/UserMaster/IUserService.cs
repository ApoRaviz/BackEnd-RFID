using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.UserManagement;
using WMS.Common;

namespace WMS.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserByUserID(string id);
        string CreateUser(User User);
        string CreateUserAndPerson(User User,Person_MT Person);
        bool GetKeyRegisterMobile(string userid,string key);
        string GetFirebaseTokenMobileByUserID(string userid, int keyOtp);
        bool RegisterTokenMobile(KeyAccessModel param);
        bool UodateTokenMobile(FirebaseTokenModel param);
        bool UpdateUser( User User);
        bool DeleteUser(int id);
        List<User> getUserNotHave(string RoleID);
        User GetUserByPersonIDSys(int personIDSys);
        object GetCustonersByUserID(string userid);
    }
}

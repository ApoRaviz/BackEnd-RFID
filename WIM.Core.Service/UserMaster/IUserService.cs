using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserByUserID(string id);
        string CreateUser(User User , string username);
        //string CreateUserAndPerson(User User,Person_MT Person);
        bool GetKeyRegisterMobile(string userid,string key);
        string GetFirebaseTokenMobileByUserID(string userid, int keyOtp );
        bool RegisterTokenMobile(KeyAccessModel param , string username);
        bool UodateTokenMobile(FirebaseTokenModel param , string username);
        bool UpdateUser( User User , string username);
        bool DeleteUser(int id);
        List<User> getUserNotHave(string RoleID);
        User GetUserByPersonIDSys(int personIDSys);
        object GetCustonersByUserID(string userid);
    }
}

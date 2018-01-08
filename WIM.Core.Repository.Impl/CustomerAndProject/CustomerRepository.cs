using System.Collections.Generic;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;

namespace WIM.Core.Repository.Impl
{
    public class CustomerRepository : Repository<Customer_MT>, ICustomerRepository
    {
        private CoreDbContext Db { get; set; }

        public CustomerRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public object GetByUserID(string userid)
        {
            var query = (from ctm in Db.Customer_MT
                         join c in Db.Project_MT on ctm.CusIDSys equals c.CusIDSys
                         join d in Db.Role on c.ProjectIDSys equals d.ProjectIDSys
                         join e in Db.UserRoles on d.RoleID equals e.RoleID
                         where e.UserID == userid
                         select new
                         {
                             ctm.CusID,
                             ctm.CusIDSys,
                             ctm.CusName
                         }).Distinct();
            return query.ToList();
        }

        public object GetProjectByUserIDCusID(string userid, int cusIDSys)
        {
            var query = from ctm in Db.Customer_MT
                        join pm in Db.Project_MT on ctm.CusIDSys equals pm.CusIDSys
                        join r in Db.Role on pm.ProjectIDSys equals r.ProjectIDSys
                        join ru in Db.UserRoles on r.RoleID equals ru.RoleID
                        where ru.UserID == userid && pm.CusIDSys == cusIDSys
                        select new
                        {
                            pm.ProjectIDSys,
                            pm.ProjectName,
                        };
            return query.ToList();
        }


        public IEnumerable<AutocompleteCustomerDto> AutocompleteItem(string term)
        {
            var qr = (from cm in Db.Customer_MT
                      where cm.CusID.Contains(term)
                      || cm.CusName.Contains(term)
                      select new AutocompleteCustomerDto
                      {
                          CusIDSys = cm.CusIDSys,
                          CusName = cm.CusName,
                          CusID = cm.CusID
                      }
                     ).ToList();
            return qr;
        }

    }
}

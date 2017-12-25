using System;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.LabelManagement;
using System.Linq;
using AutoMapper;

namespace WIM.Core.Repository.Impl
{


    public class LabelControlRepository : Repository<LabelControl>, ILabelControlRepository
    {
        private CoreDbContext Db { get; set; }
        private LabelControlDto res;
        LabelControl resData;
        public LabelControlRepository(CoreDbContext context) : base(context)
        {
            Db = context;
            res = new LabelControlDto();
            resData = new LabelControl();
        }

        public LabelControlDto GetDto(string Lang, int ProjectID)
        {
            try
            {
                using (CoreDbContext db = new CoreDbContext())
                {
                    resData = (
                          from p in db.LabelControl
                          join pj in db.Project_MT on p.ProjectIDSys equals pj.ProjectIDSys
                          where p.Lang == Lang && p.ProjectIDSys == ProjectID
                          select p
                    ).FirstOrDefault();
                    res = Mapper.Map<LabelControl, LabelControlDto>(resData);

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return res;
        }

        public LabelControlDto EditData(LabelControl LabelData)
        {
            try
            {
                using (CoreDbContext db = new CoreDbContext())
                {
                    resData = (
                          from p in db.LabelControl
                          where p.LabelIDSys == LabelData.LabelIDSys
                          select p
                    ).FirstOrDefault();

                    res = Mapper.Map<LabelControl, LabelControlDto>(resData);

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return res;
        }

        public LabelControlDto EditData(string Lang, int ProjectID)
        {
            throw new NotImplementedException();
        }

        public LabelControlDto CreateData(string Lang, int ProjectID)
        {
            throw new NotImplementedException();
        }
    }
}

using Isuzu.Context;
using Isuzu.Entity;
using Isuzu.Repository.LabelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;

namespace Isuzu.Repository.Impl.LabelManagement
{
    public class LabelRepository: Repository<LabelRunning>, ILabelRepository
    {
        private IsuzuDataContext Db { get; set; }
        private IIdentity Identity;

        public LabelRepository(IsuzuDataContext context,IIdentity identity) :base(context,identity)
        {
            Db = context;
            Identity = identity;
        }
    }
}


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

        public LabelRepository(IsuzuDataContext context) :base(context)
        {
            Db = context;
        }
    }
}

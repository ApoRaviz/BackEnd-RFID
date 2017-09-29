using Fuji.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Service.ProgramVersion
{
    public interface IProgramVersionService
    {        
        ProgramVersionHistory GetProgramVersion(string programName);       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface
{
    public interface IProgramVersionService
    {        
        ProgramVersionHistory GetProgramVersion(string programName);       
    }
}

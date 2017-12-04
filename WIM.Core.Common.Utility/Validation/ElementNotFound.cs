using System;
namespace WIM.Core.Common.Utility.Validation
{
    public class ElementNotFound : Exception
    {
        public string[] Params { get; set; }
        public ElementNotFound(params string[] args)
        {
            this.Params = args;
        }
    }
}

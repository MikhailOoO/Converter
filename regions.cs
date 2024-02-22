using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class regions
    {
        public string regionName { get; set; }
        public List<string> codes { get; set; }

        public List<string> stName { get; set; }

        public regions() { codes = new List<string>(); stName = new List<string>(); }
        public regions(string tempRgNm, List<string> tempCodes)
        {
            regionName = tempRgNm;
            codes = tempCodes;
        }

        
    }
}

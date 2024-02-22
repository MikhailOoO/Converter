using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class wmoCodes
    {
        private string path = @".\wmoCodes\meteostations.csv";
        private List<regions> rgs { get; set; }
        public wmoCodes()
        {
            rgs = new List<regions>();
            using (var reader = new StreamReader(path, Encoding.GetEncoding("windows-1251")))
            {
                reader.ReadLine();
                string[] info;
                regions tempRg = new regions();
                info = reader.ReadLine().Split(';');
                tempRg.codes.Add(info[0]);
                tempRg.stName.Add(info[1]);
                tempRg.regionName = info[2];
                while (!reader.EndOfStream)
                {
                    info = reader.ReadLine().Split(';');
                    if (tempRg.regionName == info[2])
                    {
                        tempRg.codes.Add(info[0]);
                        tempRg.stName.Add(info[1]);
                        //MessageBox.Show(info[0] + " " + info[1]);
                    }
                    else
                    {
                       
                        rgs.Add(tempRg);
                        tempRg = new regions();
                        tempRg.regionName = info[2];
                        tempRg.codes.Add(info[0]);
                        tempRg.stName.Add(info[1]);
                    }
                }
                rgs.Add(tempRg);
            }

        }

        public List<regions> getListOfRegions() { return rgs; }
    }
}

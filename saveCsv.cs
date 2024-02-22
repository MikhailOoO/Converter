using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class saveCsv
    {
        public saveCsv(List<decodedWeather> dW, string path) {
            try
            {
                using (var streamReader = new StreamWriter(path, false, Encoding.GetEncoding("windows-1251")))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
                    {
                        Delimiter = ";"

                    };
                    using (var csvReader = new CsvWriter(streamReader, csvConfig))
                    {
                        csvReader.WriteRecords(dW);
                    }
                }
                MessageBox.Show("Csv файл сохранен.");
            }
            catch (Exception exc) { MessageBox.Show(exc.Message); }
        }
    }
}

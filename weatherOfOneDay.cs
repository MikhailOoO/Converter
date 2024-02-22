using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class weatherOfOneDay
    {
        private string date;
        private string[] weatherFromStations;

        public weatherOfOneDay() { }
        public weatherOfOneDay(string Date, string[] WeatherFromStations)
        {
            this.date = Date;
            this.weatherFromStations = WeatherFromStations;
        }
        public string[] getWeatherFromStations()
        {
            return this.weatherFromStations;
        }
        public string getDate()
        {
            return this.date;
        }

    }
}

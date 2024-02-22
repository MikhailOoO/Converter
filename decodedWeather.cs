using System;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    class decodedWeather
    {
        private bool flag = false;
        private bool flagTemp = false;
        private bool flagDew = false;
        private bool flagWindSpeed = false;
        private bool flagWindDirection = false;
        private bool flagPressureSea = true;
        private string weather = "";
        private int? standartNumSet = null;
        private string standartStringSet = null; 
        private string alphabet = "1234567890/";
        public int? station { get; set; }
        public string stationName { get; set; }
        public string region { get; set; }
        public string date { get; set; }
        public float? temperature { get; set; }
        public float? dew { get; set; }
        public float? pressureStation { get; set; }
        public float? pressureSea { get; set; }
        public int? windSpeed { get; set; }
        public int? windDirection { get; set; }
        public string directionName { get; set; }
        public float? precipitation { get; set; }
        public int? cloudsCount { get; set; }

        private void setStandartSettings() {
            date = standartStringSet;
            station = standartNumSet;
            temperature = standartNumSet;
            dew = standartNumSet;
            windSpeed = standartNumSet;
            windDirection = standartNumSet;
            directionName = standartStringSet;
            precipitation = standartNumSet;
        }

        public int decodeAll(List<string> wmo, List<string> rgs, List<string> stationN) {

            if (!weather.Contains("NIL"))
            {
                string[] groups = weather.Split(' ');
                char index = '\0';
                int part = 0, count = 0, temp = 0;
                string tempGroups;
                try
                {
                    foreach (string g in groups)
                    {
                        tempGroups = g.Trim();
                        if (tempGroups.Length >= 3)
                        {
                            switch (part)
                            {
                                case 0:
                                    if (count == 0)
                                    {
                                        if (wmo.Contains(tempGroups))
                                        {
                                            station = int.Parse(tempGroups);
                                            region = rgs[wmo.IndexOf(tempGroups)];
                                            stationName = stationN[wmo.IndexOf(tempGroups)];
                                        }
                                        else {
                                            station = int.Parse(tempGroups);
                                        }
                                    }
                                    if (count == 2)
                                    {
                                        if (tempGroups.Length == 5)
                                        {
                                            if(tempGroups[0] != '/')
                                            {
                                                cloudsCount = int.Parse(tempGroups[0].ToString());
                                            }
                                            if (tempGroups[1] != '/' && tempGroups[2] != '/')
                                            {
                                                windDirection = int.Parse(tempGroups.Substring(1, 2));
                                                if ((windDirection > -1 && windDirection < 37) || windDirection == 99) {
                                                    //windDirection *= 10;
                                                    flagWindDirection = true;
                                                    directionName = getStringNameOfWindDirection(windDirection);
                                                    windDirection *= 10;
                                                }
                                                else
                                                {
                                                    flag = true;
                                                }
                                            }
                                            else flag = true;
                                            if (tempGroups[3] != '/' && tempGroups[4] != '/')
                                            {
                                                flagWindSpeed = true;
                                                windSpeed = int.Parse(tempGroups.Substring(3, 2));
                                            }
                                            else flag = true;
                                        }
                                        else flag = true;
                                        part = 1;
                                        break;
                                    }
                                    count++;
                                    break;
                                case 1:
                                    temp = whichPart(tempGroups, part);
                                    if (temp != part) { part = temp; break; }
                                    foreach (char c in tempGroups)
                                    {
                                        if (alphabet.Contains(c.ToString())) { index = c; break; }
                                    }
                                    switch (index)
                                    {
                                        case '1':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    flagTemp = true;
                                                    temperature = float.Parse(tempGroups.Substring(2, 3)) / 10;
                                                    if (tempGroups[1] == '1') temperature *= -1;
                                                }
                                                else flag = true;
                                            }
                                            else flag = true;
                                            break;
                                        case '2':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    flagDew = true;
                                                    dew = float.Parse(tempGroups.Substring(2, 3)) / 10;
                                                    if (tempGroups[1] == '1') dew *= -1;
                                                }
                                                else flag = true;
                                            }
                                            else flag = true;
                                            break;
                                        case '3':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    if(tempGroups[1] == '0') {
                                                        pressureStation = (float.Parse(tempGroups.Substring(1))/10) + 1000;
                                                    }
                                                    else
                                                    {
                                                        pressureStation = float.Parse(tempGroups.Substring(1))/10;
                                                    }
                                                }
                                            }
                                            break;
                                        case '4':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/") && flagPressureSea)
                                                {
                                                    if (tempGroups[1] == '0')
                                                    {
                                                        pressureSea = (float.Parse(tempGroups.Substring(1)) / 10) + 1000;
                                                    }
                                                    else
                                                    {
                                                        pressureSea = float.Parse(tempGroups.Substring(1)) / 10;
                                                    }
                                                    flagPressureSea = false;
                                                }
                                            }
                                            break;
                                        case '5':
                                            break;
                                        case '6':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    if (tempGroups.Substring(1, 2) != "99")
                                                    {
                                                        precipitation = float.Parse(tempGroups.Substring(1, 3));
                                                    }
                                                    else
                                                    {
                                                        precipitation = float.Parse(tempGroups[3].ToString()) / 10;
                                                    }


                                                }
                                            }
                                            break;
                                            /*case '7':
                                                break;
                                            case '8':
                                                break;*/

                                    }
                                    break;
                                case 3:
                                    //MessageBox.Show(tempGroups + " - " + part);
                                    temp = whichPart(tempGroups, part);
                                    if (temp != part) { part = temp; break; }
                                    foreach (char c in tempGroups)
                                    {
                                        if (alphabet.Contains(c.ToString())) { index = c; break; }
                                    }
                                    switch (index)
                                    {
                                        /*case '1':
                                            break;
                                        case '2':
                                            break;
                                        case '3':
                                            break;
                                        case '4':
                                            break;
                                        case '5':
                                            break;*/
                                        case '6':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    if (tempGroups.Substring(1, 2) != "99")
                                                    {
                                                        precipitation = float.Parse(tempGroups.Substring(1, 3));
                                                    }
                                                    else
                                                    {
                                                        precipitation = float.Parse(tempGroups[3].ToString()) / 10;
                                                    }


                                                }
                                            }
                                            break;
                                            /*case '8':
                                                break;
                                            case '9':
                                                break;*/
                                    }
                                    break;
                                    /*case 5:
                                        foreach (char c in tempGroups)
                                        {
                                            if (alphabet.Contains(c.ToString())) { index = c; break; }
                                        }
                                        switch (index)
                                        {
                                            case '1':
                                                break;
                                            case '5':
                                                break;
                                            case '7':
                                                break;
                                            case '8':
                                                break;
                                        }
                                        break;*/

                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }

                }
                catch (Exception exc)
                {
                    Console.Write(exc.StackTrace);
                    return 1;
                }
            }
            return 0;
        }

        public int decodeUseFilter(List<string> wmo, List<string> rgs, List<string> stationN)
        {

            if (!weather.Contains("NIL"))
            {
                string[] groups = weather.Split(' ');
                char index = '\0';
                int part = 0, count = 0, temp = 0;
                string tempGroups;
                try
                {
                    foreach (string g in groups)
                    {
                        tempGroups = g.Trim();
                        if (tempGroups.Length >= 3)
                        {
                            switch (part)
                            {
                                case 0:
                                    if (count == 0)
                                    {
                                        if (wmo.Contains(tempGroups)) {
                                            station = int.Parse(tempGroups);
                                            region = rgs[wmo.IndexOf(tempGroups)];
                                            stationName = stationN[wmo.IndexOf(tempGroups)];
                                        }
                                        else { flag = true; break; }
                                    }
                                    if (count == 2)
                                    {
                                        if (tempGroups.Length == 5)
                                        {
                                            if (tempGroups[0] != '/')
                                            {
                                                cloudsCount = int.Parse(tempGroups[0].ToString());
                                            }
                                            if (tempGroups[1] != '/' && tempGroups[2] != '/')
                                            {
                                                windDirection = int.Parse(tempGroups.Substring(1, 2));
                                                if ((windDirection > -1 && windDirection < 37) || windDirection == 99)
                                                {
                                                    //windDirection *= 10;
                                                    flagWindDirection = true;
                                                    directionName = getStringNameOfWindDirection(windDirection);
                                                    windDirection *= 10;
                                                }
                                                else
                                                {
                                                    flag = true;
                                                }
                                            }
                                            else flag = true;
                                            if (tempGroups[3] != '/' && tempGroups[4] != '/')
                                            {
                                                flagWindSpeed = true;
                                                windSpeed = int.Parse(tempGroups.Substring(3, 2));
                                            }
                                            else flag = true;
                                        }
                                        else flag = true;
                                        part = 1;
                                        break;
                                    }
                                    count++;
                                    break;
                                case 1:
                                    temp = whichPart(tempGroups, part);
                                    if (temp != part) { part = temp; break; }
                                    foreach (char c in tempGroups)
                                    {
                                        if (alphabet.Contains(c.ToString())) { index = c; break; }
                                    }
                                    switch (index)
                                    {
                                        case '1':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    flagTemp = true;
                                                    temperature = float.Parse(tempGroups.Substring(2, 3)) / 10;
                                                    if (tempGroups[1] == '1') temperature *= -1;
                                                }
                                                else flag = true;
                                            }
                                            else flag = true;
                                            break;
                                        case '2':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    flagDew = true;
                                                    dew = float.Parse(tempGroups.Substring(2, 3)) / 10;
                                                    if (tempGroups[1] == '1') dew *= -1;
                                                }
                                                else flag = true;
                                            }
                                            else flag = true;
                                            break;
                                        case '3':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    if (tempGroups[1] == '0')
                                                    {
                                                        pressureStation = (float.Parse(tempGroups.Substring(1)) / 10) + 1000;
                                                    }
                                                    else
                                                    {
                                                        pressureStation = float.Parse(tempGroups.Substring(1)) / 10;
                                                    }
                                                }
                                            }
                                            break;
                                        case '4':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/") && flagPressureSea)
                                                {
                                                    if (tempGroups[1] == '0')
                                                    {
                                                        pressureSea = (float.Parse(tempGroups.Substring(1)) / 10) + 1000;
                                                    }
                                                    else
                                                    {
                                                        pressureSea = float.Parse(tempGroups.Substring(1)) / 10;
                                                    }
                                                    flagPressureSea = false;
                                                }
                                            }
                                            break;
                                        case '5':
                                            break;
                                        case '6':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    if (tempGroups.Substring(1, 2) != "99")
                                                    {
                                                        precipitation = float.Parse(tempGroups.Substring(1, 3));
                                                    }
                                                    else
                                                    {
                                                        precipitation = float.Parse(tempGroups[3].ToString()) / 10;
                                                    }


                                                }
                                            }
                                            break;
                                            /*case '7':
                                                break;
                                            case '8':
                                                break;*/

                                    }
                                    break;
                                case 3:
                                    //MessageBox.Show(tempGroups + " - " + part);
                                    temp = whichPart(tempGroups, part);
                                    if (temp != part) { part = temp; break; }
                                    foreach (char c in tempGroups)
                                    {
                                        if (alphabet.Contains(c.ToString())) { index = c; break; }
                                    }
                                    switch (index)
                                    {
                                        /*case '1':
                                            break;
                                        case '2':
                                            break;
                                        case '3':
                                            break;
                                        case '4':
                                            break;
                                        case '5':
                                            break;*/
                                        case '6':
                                            if (tempGroups.Length == 5)
                                            {
                                                if (!tempGroups.Contains("/"))
                                                {
                                                    if (tempGroups.Substring(1, 2) != "99")
                                                    {
                                                        precipitation = float.Parse(tempGroups.Substring(1, 3));
                                                    }
                                                    else
                                                    {
                                                        precipitation = float.Parse(tempGroups[3].ToString()) / 10;
                                                    }


                                                }
                                            }
                                            break;
                                            /*case '8':
                                                break;
                                            case '9':
                                                break;*/
                                    }
                                    break;
                                    /*case 5:
                                        foreach (char c in tempGroups)
                                        {
                                            if (alphabet.Contains(c.ToString())) { index = c; break; }
                                        }
                                        switch (index)
                                        {
                                            case '1':
                                                break;
                                            case '5':
                                                break;
                                            case '7':
                                                break;
                                            case '8':
                                                break;
                                        }
                                        break;*/

                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }

                }
                catch (Exception exc)
                {
                    Console.Write(exc.StackTrace);
                    return 1;
                }
            }
            return 0;
        }

        public decodedWeather(string Weather, string Date)
        {
            setStandartSettings();
            weather = Weather;
            date = Date;
        }
 
        private int whichPart(string text, int oldPart)
        {
            if (text == "333")
            {
                return 3;
            }
            if (text == "555")
            {
                return 5;
            }
            return oldPart;

        }

        public bool isRightWeather() {
            if (flagDew && flagTemp && flagWindDirection && flagWindSpeed) return true;
            return false;
        }

        private string getStringNameOfWindDirection(int? temp)
        {
            string output = "";
            if (temp.HasValue == false)
                output = null;
            else if (temp == 0)
                output = "ШТЛ";
            else if (temp == 99)
                output = "ПЕРЕМ";
            else if (temp == 35 || temp == 36 || temp == 1)
                output = "С";
            else if (temp <= 3)
                output = "ССВ";
            else if (temp <= 5)
                output = "СВ";
            else if (temp <= 7)
                output = "ВСВ";
            else if (temp <= 10)
                output = "В";
            else if (temp <= 12)
                output = "ВЮВ";
            else if (temp <= 14)
                output = "ЮВ";
            else if (temp <= 16)
                output = "ЮЮВ";
            else if (temp <= 19)
                output = "Ю";
            else if (temp <= 21)
                output = "ЮЮЗ";
            else if (temp <= 23)
                output = "ЮЗ";
            else if (temp <= 25)
                output = "ЗЮЗ";
            else if (temp <= 28)
                output = "З";
            else if (temp <= 30)
                output = "ЗСЗ";
            else if (temp <= 32)
                output = "СЗ";
            else
                output = "ССЗ";
            return output;
        }

    }
}

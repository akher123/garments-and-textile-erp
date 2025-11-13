using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Cms;

namespace SCERP.Common
{
    public static class DateTimeExtension
    {
        public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }

        public static IEnumerable<DateTime> ToDays(DateTime? fromDate, DateTime? toDate)
        {
            IEnumerable<DateTime> days = new LinkedList<DateTime>();
            if (fromDate.HasValue && toDate.HasValue)
            {
                days = fromDate.Value.Range(toDate.Value).ToList();
            }
            return days;
        }

        public static DateTime ToLastMonthDate( this DateTime dateTime,int prevMonthDay)
        {
           return  new DateTime(dateTime.Year, dateTime.Month - 1, prevMonthDay);
        }
        public static DateTime ToThisMonthDate(this DateTime dateTime, int day)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
        }
        public static DateTime ToMargeDateAndTime(this DateTime date, string time)
        {
           // date = date.Date;
            DateTime dateTime = DateTime.Parse(date.ToString("yyyy-MM-dd ") + time);
            return dateTime;
        }
        public static TimeSpan GetHHTime(string Hours, string Minutes, string Period)
        {
            TimeSpan timeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
            TimeSpan PeriodSpan = new TimeSpan(12, 0, 0);

            if (Period == "AM" && Hours == "12")
                timeSpan = timeSpan.Subtract(PeriodSpan);
        
            else if (Period == "PM" && Hours != "12")
                timeSpan = timeSpan.Add(PeriodSpan);

            return timeSpan;
        }

        public static string RemoveWhiteSpace(this string stringValue)
        {
            return Regex.Replace(stringValue??"", @"^[\s,]+|[\s,]+$", "");
        }

        public static string PadZero(this string value, int totalWidth)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = string.Empty;
            }
            return value.PadLeft(totalWidth,'0');
        }

        public static string IncrementOne(this string value)
        {
            var incrementValue = 0;
            if (string.IsNullOrWhiteSpace(value))
            {
                return "1";
            }
            else
            {
                incrementValue = Convert.ToInt32(value) + 1;
            }
            return Convert.ToString(incrementValue);
        }

        private static dynamic BindDynamic(DataRow dataRow)
        {
            dynamic result = null;
            if (dataRow != null)
            {
                result = new ExpandoObject();
                var resultDictionary = (IDictionary<string, object>)result;
                foreach (DataColumn column in dataRow.Table.Columns)
                {
                    var dataValue = dataRow[column.ColumnName];
                    resultDictionary.Add(column.ColumnName, DBNull.Value.Equals(dataValue) ? null : dataValue);
                }
            }
            return result;
        }
        public static IEnumerable<dynamic> Todynamic(this DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return BindDynamic(row);
            }
        }


       public static String NumWordsWrapper(this double n)
        {
            string words = "";
            double intPart;
            double decPart = 0;
            if (n == 0)
                return "zero";
            try
            {
                string[] splitter = n.ToString().Split('.');
                intPart = double.Parse(splitter[0]);
                decPart = double.Parse(splitter[1]);
            }
            catch
            {
                intPart = n;
            }

            words = NumWords(intPart);

            if (decPart > 0)
            {
                if (words != "")
                    words += " and ";
                int counter = decPart.ToString().Length;
                switch (counter)
                {
                    case 1: words += NumWords(decPart) + " tenths"; break;
                    case 2: words += NumWords(decPart) + " hundredths"; break;
                    case 3: words += NumWords(decPart) + " thousandths"; break;
                    case 4: words += NumWords(decPart) + " ten-thousandths"; break;
                    case 5: words += NumWords(decPart) + " hundred-thousandths"; break;
                    case 6: words += NumWords(decPart) + " millionths"; break;
                    case 7: words += NumWords(decPart) + " ten-millionths"; break;
                }
            }
            return words;
        }

        static String NumWords(double n) //converts double to words
        {
            string[] numbersArr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tensArr = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            string[] suffixesArr = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
            string words = "";

            bool tens = false;

            if (n < 0)
            {
                words += "negative ";
                n *= -1;
            }

            int power = (suffixesArr.Length + 1) * 3;

            while (power > 3)
            {
                double pow = Math.Pow(10, power);
                if (n >= pow)
                {
                    if (n % pow > 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                    }
                    else if (n % pow == 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
                    }
                    n %= pow;
                }
                power -= 3;
            }
            if (n >= 1000)
            {
                if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " thousand, ";
                else words += NumWords(Math.Floor(n / 1000)) + " thousand";
                n %= 1000;
            }
            if (0 <= n && n <= 999)
            {
                if ((int)n / 100 > 0)
                {
                    words += NumWords(Math.Floor(n / 100)) + " hundred";
                    n %= 100;
                }
                if ((int)n / 10 > 1)
                {
                    if (words != "")
                        words += " ";
                    words += tensArr[(int)n / 10 - 2];
                    tens = true;
                    n %= 10;
                }

                if (n < 20 && n > 0)
                {
                    if (words != "" && tens == false)
                        words += " ";
                    words += (tens ? "-" + numbersArr[(int)n - 1] : numbersArr[(int)n - 1]);
                    n -= Math.Floor(n);
                }
            }

            return words;

        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public class BanglaConversion
    {
        public static string ConvertToBanglaNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            str = str.Trim().Replace(" ", "");

            char[] bangla = {'০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯'};
            char[] input = str.ToCharArray();
            const string english = "0123456789";
            string output = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (input[i] == '.')
                {
                    output += ".";
                    continue;
                }
                if (input[i] == '/')
                {
                    output += "/";
                    continue;
                }
                if (input[i] == '-')
                {
                    output += "-";
                    continue;
                }
                output += bangla[english.IndexOf(input[i])];
            }
            return output;
        }

        public static string ConvertToBanglaMonth(string str)
        {
            string[] Month = {"জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "আগস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর"};

            int i = 0;

            if (!string.IsNullOrEmpty(str))
            {
                i = Convert.ToInt32(str);
            }
            return Month[i - 1];
        }
        public static string ConvertEnglishMonthtoBanglaMonth(string str)
        {
            string[] EnglishMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string[] BanglaMonth = { "জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "আগস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };

            for (int i = 0; i < EnglishMonth.Count(); i++)
            {
                if (String.Equals(str, EnglishMonth[i], StringComparison.CurrentCultureIgnoreCase))
                {
                    return BanglaMonth[i];
                }
            }
            return BanglaMonth[0];
        }
        public static string ConvertToEnglishMonth(string str)
        {
            string[] Month = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

            int i = 0;

            if (!string.IsNullOrEmpty(str))
            {
                i = Convert.ToInt32(str);
            }
            return Month[i - 1];
        }

        public static string ConvertEnglishDaytoBanglaDay(string str)
        {
            string[] EnglishDay = {"Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};
            string[] BanglaDay = {"শনিবার", "রবিবার", "সোমবার", "মঙ্গলবার", "বুধবার", "বৃহস্পতিবার", "শুক্রবার"};

            for (int i = 0; i < EnglishDay.Count(); i++)
            {
                if (String.Equals(str, EnglishDay[i], StringComparison.CurrentCultureIgnoreCase))
                {
                    return BanglaDay[i];
                }
            }
            return BanglaDay[0];
        }

    }
}







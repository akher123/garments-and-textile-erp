using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public abstract class DisplayProperty
    {
        public string Text { get; set; }

    }
    public sealed class Hour : DisplayProperty
    {
        public string HourKey { get; set; }

    }
    public sealed class Minute : DisplayProperty
    {
        public string MinuteKey { get; set; }

    }
    public sealed class Period : DisplayProperty
    {
        public string PeriodKey { get; set; }
    }

    public  static class TimeConfiguration 
    {

        public static List<Hour> GetHours()
        {
            var hours=new List<Hour>();
            for (var i = 0; i <= 12; i++)
            {
                if (i < 10)
                    hours.Add(new Hour()
                    {
                        HourKey = "0" + i,
                        Text = "0" + i
                    }) ;
                else
                    hours.Add(new Hour()
                    {
                        HourKey = Convert.ToString(i),
                        Text = Convert.ToString(i),
                    });
            }
            return hours;
        }

        public static List<Minute> GetMunites()
        {
            var munites = new List<Minute>();
            for (var i = 0; i <= 60; i++)
            {
                if (i < 10)
                {
                    munites.Add(new Minute()
                    {
                        MinuteKey = "0" + i,
                        Text = "0" + i
                    });
                }
                else
                    munites.Add(new Minute()
                    {
                        MinuteKey = Convert.ToString(i),
                        Text = Convert.ToString(i),
                    });
            }
            return munites;
        }

        public static List<Period> GePeriods()
        {
           return new List<Period>
           {
               new Period()
               {
                   PeriodKey = "AM",
                   Text = "AM"
               },
                   new Period()
               {
                   PeriodKey = "PM",
                   Text = "PM"
               },
           };
        }
    }
}

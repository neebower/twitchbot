using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitchbot
{
    public static class TimeSpanExtension
    {
        private static Dictionary<string, int> dict = new Dictionary<string, int>()
        {
            {"min", 60 },
            {"hour", 60*60 },
            {"day" , 24*60*60 },
            {"month", 30*24*60*60 },
            {"year", 12*30*24*60*60 }
        };

        //TODO yield return
        public static IEnumerable<int> GetDataFromYearsToSeconds(this TimeSpan tsp)
        {
            var dict1 = dict.Reverse();
            List<int> result = new List<int>();

            var seconds = tsp.TotalSeconds;

            foreach (var tm in dict1)
            {
                if(tm.Value > seconds)
                {
                    continue;
                }
                else
                {

                    result.Add((int)(seconds / tm.Value));
                    seconds -= seconds / tm.Value * tm.Value;
                    
                }
            }
            result.Add((int)seconds);
            return result;
        }
    }
    //public class CustomTimeSpan
    //{
    //    private IEnumerable<int> _data;

    //    private

    //    public CustomTimeSpan(IEnumerable<int> data)
    //    {    
    //        _data = data;
    //        _data.ElementAt(0);
    //    }

    //    public string GetAsFormattedString(string lang)
    //    {
    //        if(lang.StartsWith(lang))
    //        return $"{_data}";
    //    }
    //}
}

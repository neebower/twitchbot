using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitchbot
{
    public static class Utils
    {
        public static string GetDeclension(int num, string nominativ, string genetiv, string plural)
        {
            num = num % 100;
            if(num >=11 && num <= 19)
            {
                return plural;
            }

            var i = num % 10;

            switch (i)
            {
                case 1: return nominativ;
                case 2:
                case 3:
                case 4: return genetiv;
                default: return plural;
            }
        }
    }
    
}

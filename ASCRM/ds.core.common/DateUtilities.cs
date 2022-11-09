using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.core.common
{
    public static class DateUtilities
    {


        public static string ConvertDateToSQL(string theDate)
        {
            if (theDate == null || theDate == "")
            {
                return "";
            }
            if (theDate.Contains(" 00:00:00")) { theDate = theDate.Replace(" 00:00:00", ""); }

            string[] theData = theDate.Split("/".ToCharArray());
            int iDay = int.Parse(theData[0]);
            int iMonth = int.Parse(theData[1]);
            int iYear = int.Parse(theData[2]);
            return iMonth.ToString() + "/" + iDay.ToString() + "/" + iYear.ToString();


        }
    }
}

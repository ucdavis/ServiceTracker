using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceTracker.Helpers
{
    public class YearFinder
    {
        private static int _Year;

        public static int Year
        {
            get
            {
                if (_Year== 0)
                {
                    if (DateTime.Now.Month >= 9 && DateTime.Now.Month <= 12)
                    {
                        _Year = DateTime.Now.Year;
                    }
                    else
                    {
                        _Year = DateTime.Now.Year -1;
                    }
                }
                return _Year;
            }
        }

        
        public static List<int> YearList
        {
            get{
                return Enumerable.Range(2019, Year - 2017).ToList();
            }
        }

        public static List<int> YearListReverse
        {
            get{
                return Enumerable.Range(2022, Year - 2021).Reverse().ToList();
            }
        }
    }
}
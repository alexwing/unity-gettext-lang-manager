using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LangManager
{
    public static class CountryFlagsHelper
    {
        public static List<CountryFlagEntry> Search(this List<CountryFlagEntry> countryFlags, string query)
        {
            var qLower = query.ToLower();
            return countryFlags.FindAll(x =>
                x.name.ToLower().Contains(qLower)           
                || x.code.ToLower().Contains(query));
        }
        
        public static int GetLastNum(this string a)
        {
            string b = string.Empty;
            int val = 0;
            bool wasDigit = false;

            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                {
                    if (!wasDigit)
                    {
                        b = string.Empty;
                    }

                    b += a[i];
                    wasDigit = true;
                }
                else
                {
                    wasDigit = false;
                }
            }

            if (b.Length > 0)
            {
                val = int.Parse(b);
                return val;
            }

            return 0;
        }
    }
}
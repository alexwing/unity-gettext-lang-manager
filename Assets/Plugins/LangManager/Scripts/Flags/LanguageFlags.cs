using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LangManager
{
    public static class LanguageFlags
    {
        const string SpriteResourcesPath = "Flags/";
        const string DataResourcesPath = "Data/flagsData";

        private static List<CountryFlagEntry> countryFlagEntries;

        public static List<CountryFlagEntry> Flags
        {
            get
            {
                if (countryFlagEntries == null)
                {
                    ParseCountryEntries();
                }
                
                return countryFlagEntries;
            }
            set => countryFlagEntries = value;
        }

        static void ParseCountryEntries()
        {
            //LoadSprites();

            var flagsToCountries = Resources.Load<TextAsset>(DataResourcesPath);
            Flags = JsonHelper.FromJson<CountryFlagEntry>(flagsToCountries.text).ToList();
            for (var i = 0; i < Flags.Count; i++)
            {
                string spritePath = SpriteResourcesPath + Flags[i].flagName.ToLower();
                Flags[i].flag = Resources.Load<Sprite>(spritePath);
            }   
        }
    }
}
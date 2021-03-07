using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

namespace LangManager
{
    public class UILangManager 
    {     
        public static string GetString(string text, Font @SourceFont, TMP_Text @langText)
        {
            text = LanguageManager.catalog.GetString(text);
            TMP_FontAsset m_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFont, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic);
            TextInfo nameInfo = new CultureInfo("en-US", false).TextInfo;
            m_FontAsset.TryAddCharacters(nameInfo.ToString());
            langText.font = m_FontAsset;
            langText.text = text;
            return text;
        }

        public static string GetString(string text, Font @SourceFont, Text @langText)
        {
            text = LanguageManager.catalog.GetString(text);
            langText.font = SourceFont;
            langText.text = text;
            return text;
        }
    }
}
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

namespace LangManager
{
    public static class UILangManager 
    {

        public static string GetString(string text, TMP_FontAsset @SourceFont, TMP_Text @langText)
        {
            text = LanguageManager.catalog.GetString(text);
            TMP_FontAsset m_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFont.sourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic);
            TextInfo nameInfo = new CultureInfo("en-US", false).TextInfo;
            @SourceFont.TryAddCharacters(nameInfo.ToString());
            langText.font = m_FontAsset;
            langText.text = text;
            return text;
        }

        public static string GetString(string text, Font @SourceFont, TMP_Text @langText, Material material = null)
        {
            text = LanguageManager.catalog.GetString(text);
            TMP_FontAsset m_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFont, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic);
            TextInfo nameInfo = new CultureInfo("en-US", false).TextInfo;
            m_FontAsset.TryAddCharacters(nameInfo.ToString());

            if (material) {
                material.mainTexture =  m_FontAsset.material.mainTexture;
                m_FontAsset.material = material;
            }
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

        public static string GetString(string text, Font @SourceFont, TextMesh @langText)
        {
            text = LanguageManager.catalog.GetString(text);
            langText.font = SourceFont;
            langText.text = text;
            return text;
        }
    }
}
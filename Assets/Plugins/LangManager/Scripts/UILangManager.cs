using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

namespace LangManager
{
    public static class UILangManager
    {




        public static string GetString(string text, TMP_FontAsset @SourceFont, TMP_Text @langText, Material @material = null)
        {
            text = LanguageManager.catalog.GetString(text);
            TMP_FontAsset m_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFont.sourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic);
            TextInfo nameInfo = new CultureInfo("en-US", false).TextInfo;
            @SourceFont.TryAddCharacters(nameInfo.ToString());
            if (material)
            {
                material.mainTexture = m_FontAsset.material.mainTexture;
                m_FontAsset.material = material;
            }
            text = langRTL(text, langText);

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

            if (material)
            {
                material.mainTexture = m_FontAsset.material.mainTexture;
                m_FontAsset.material = material;
            }

            text = langRTL(text, langText);

            langText.font = m_FontAsset;
            langText.text = text;
            return text;
        }


        public static string GetString(string text, Font @SourceFont, Text @langText)
        {
            text = LanguageManager.catalog.GetString(text);
            text = langRTL(text, langText);
            langText.font = SourceFont;
            langText.text = text;
            return text;
        }

        public static string GetString(string text, Font @SourceFont, TextMesh @langText)
        {
            text = LanguageManager.catalog.GetString(text);
            text = langRTL(text, langText);

            langText.font = SourceFont;
            langText.text = text;
            return text;
        }

        #region Utils
        private static string ReverseString(string srtVarable)
        {
            return new string(srtVarable.Reverse().ToArray());
        }

        private static string langRTL(string text, Text @langText)
        {

            if (LanguageManager.instance.RTLLanguages.Contains(LanguageManager.LanguagesList.Current))
            {
                if (LanguageManager.LanguagesList.Current == "ar")
                {
                    text = ArabicSupport.ArabicFixer.Fix(text, LanguageManager.instance.ArabicShowTaskkeel, LanguageManager.instance.ArabicUseHinduNumbers);
                }
            }

            return text;
        }

        private static string langRTL(string text, TextMesh @langText)
        {
            if (LanguageManager.LanguagesList.Current == "ar")
            {
                text = ArabicSupport.ArabicFixer.Fix(text, LanguageManager.instance.ArabicShowTaskkeel, LanguageManager.instance.ArabicUseHinduNumbers);
            }

            if (LanguageManager.instance.LeftToRightRTL)
            {
                if (@langText.alignment != TextAlignment.Center)
                {
                    @langText.alignment = TextAlignment.Right;
                }
                else
                {
                    @langText.alignment = TextAlignment.Left;

                }
            }
            return text;
        }


        private static string langRTL(string text, TMP_Text @langText)
        {
            @langText.isRightToLeftText = LanguageManager.instance.RTLLanguages.Contains(LanguageManager.LanguagesList.Current);
            if (LanguageManager.LanguagesList.Current == "ar")
            {
                text = ReverseString(ArabicSupport.ArabicFixer.Fix(text, LanguageManager.instance.ArabicShowTaskkeel, LanguageManager.instance.ArabicUseHinduNumbers));
            }

            if (LanguageManager.instance.LeftToRightRTL)
            {
                if (@langText.isRightToLeftText && @langText.alignment != TextAlignmentOptions.Center)
                {
                    @langText.alignment = TextAlignmentOptions.Right;
                }
                else
                {
                    @langText.alignment = TextAlignmentOptions.Left;

                }
            }
            return text;
        }
        #endregion Utils
    }
}
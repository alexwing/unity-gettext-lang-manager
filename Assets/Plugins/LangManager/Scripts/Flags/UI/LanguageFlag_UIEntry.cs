using UnityEngine;
using UnityEngine.UI;

namespace LangManager
{
    public class LanguageFlag_UIEntry : MonoBehaviour
    {
        public Image flagImage;
        public Text countryName;
        public CountryFlagEntry FlagEntry { get; private set; }

        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        public void Init(CountryFlagEntry countryFlagEntry)
        {
            FlagEntry = countryFlagEntry;
            
            name = $"Country Entry ({countryFlagEntry.code}) {countryFlagEntry.name}";
            flagImage.sprite = countryFlagEntry.flag;

            countryName.text = string.Empty;
            if (countryFlagEntry.HasName) countryName.text += countryFlagEntry.name + " ";
           // countryName.text += $"({countryFlagEntry.code})";
        }

        public void ChangeLanguage()
        {
            LangManager.LanguageManager.LoadLang(FlagEntry.code.ToLower());
        }
    }

}
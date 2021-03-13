using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LangManager
{
    public class LanguageFlags_Example : MonoBehaviour
    {
        public LanguageFlag_UIEntry LanguageFlagUiEntryPrefab;
        public ScrollRect flagsScrollRect;

        public bool showLanguagesWithNoNames;
        
        private List<LanguageFlag_UIEntry> _uiEntries = new List<LanguageFlag_UIEntry>();

        private void Start()
        {
            SpawnAllEntries();
        }

        public void SpawnAllEntries()
        {
            var languageEntries = LanguageFlags.Flags;

            while (flagsScrollRect.content.childCount > 0)
            {
                DestroyImmediate(flagsScrollRect.content.GetChild(0).gameObject);
            }
            
            _uiEntries.Clear();

            for (int i = 0; i < languageEntries.Count; i++)
            {

                if (LanguageManager.LanguagesList.Contains(languageEntries[i].code.ToLower())){
                    var entry = Instantiate(LanguageFlagUiEntryPrefab, flagsScrollRect.content);
                    entry.Init(languageEntries[i]);

                    _uiEntries.Add(entry);
                }

            }
            
            SetEntriesVisible(_uiEntries);
        }

        void SetEntriesVisible(List<LanguageFlag_UIEntry> entries)
        { 
            foreach (var languageFlagUiEntry in _uiEntries)
            {
                bool visibleIfNoName = showLanguagesWithNoNames || languageFlagUiEntry.FlagEntry.HasName;
                languageFlagUiEntry.Visible = entries.Contains(languageFlagUiEntry) && visibleIfNoName;
            }
        }

    }
}
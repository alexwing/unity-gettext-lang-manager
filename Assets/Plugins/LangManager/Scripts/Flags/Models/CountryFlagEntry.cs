using UnityEngine;

namespace LangManager
{
    [System.Serializable]
    public class CountryFlagEntry
    {
        public string code;
        public string name;
        public string flagName;
        public Sprite flag;

        public bool HasName => !string.IsNullOrEmpty(name);
    }
}

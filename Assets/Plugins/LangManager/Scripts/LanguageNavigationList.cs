using System;
using System.Collections.Generic;
namespace LangManager
{
    public class LanguageNavigationList<String> : List<String>
    {
        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get
            {
                if (_currentIndex > Count - 1) { _currentIndex = Count - 1; }
                if (_currentIndex < 0) { _currentIndex = 0; }
                return _currentIndex;
            }
            set { _currentIndex = value; }
        }

        public String MoveNext
        {
            get { _currentIndex++; return this[CurrentIndex]; }
        }

        public String MovePrevious
        {
            get { _currentIndex--; return this[CurrentIndex]; }
        }

        public String Current
        {
            get { return this[CurrentIndex]; }
        }

        internal String Select(String lang)
        {

            if (Contains(lang))
            {
                _currentIndex = this.FindIndex(s => s.ToString().Equals(lang));
            }
            return this[CurrentIndex];
        }
    }

}
using NGettext;
using System.Collections;
using System.Globalization;
using System.IO;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace LangManager
{
    public class LanguageManager : MonoBehaviour
    {

        public List<string> Languages = new List<string>(){
                           "en", //English
                           "es", //Spanish
                           "ja", //Japanese
                           "ko", //Korean
                           "ru", //Russian
                           "zh", //Chinese
                           };

        public static readonly LanguageNavigationList<string> LanguagesList = new LanguageNavigationList<string>();

        public static ICatalog catalog = new Catalog();

        public static LanguageManager instance;

        public static bool langChangeComplete = false;


        void Awake()
        {
            //make navigation language list
            foreach (string str in Languages)
            {
                LanguagesList.Add(str);
            }
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void LoadLang(string lang)
        {
            instance.LoadLangInstance(lang);
        }

        public void LoadLangInstance(string lang)
        {
            StartCoroutine(instance.LoadLandCoroutine(lang));
        }


        public static async Task LoadLangAsync(string lang)
        {
            LoadLang(lang);
            await WaitUntil(() => langChangeComplete);

        }

        public static async Task WaitUntil(Func<bool> condition, int waitTimeMs = 25)
        {
            while (!condition.Invoke())
            {
                await Task.Delay(waitTimeMs);
            }
        }

        [Obsolete]
        public IEnumerator LoadLandCoroutine(string lang)
        {
            langChangeComplete = false;
            string url = Application.streamingAssetsPath + "/Languages/" + lang.ToLower() + "/lang.mo";

            // Debug.Log("Lang URL: " + url);

            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;

                Stream moFileStream = new MemoryStream(results);

                catalog = new Catalog(moFileStream, new CultureInfo(lang));

                LanguagesList.Select(lang);

                Debug.Log($"Language [{ LanguagesList.Current }] selected");
                langChangeComplete = true;
                EventManager.TriggerEvent("LangChanged");
            }

        }
    }
}
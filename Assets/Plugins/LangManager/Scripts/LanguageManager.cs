using NGettext;
using System.Collections;
using System.Globalization;
using System.IO;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


namespace LangManager
{
    public class LanguageManager : MonoBehaviour
    {

        public static ICatalog catalog = new Catalog();
        public static string CurrentLang = "es";

        public static LanguageManager instance;

        public static bool langChangeComplete = false;


        // Static global event to notify damage on any entity
        static public event Action<String, float> EventEntityDamage;

        void Awake()
        {
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
                // Show results as text
                // Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

                Stream moFileStream = new MemoryStream(results);

                catalog = new Catalog(moFileStream, new CultureInfo(lang));
                

                Debug.Log($"Language [{ lang }] selected");
                CurrentLang = lang;
                langChangeComplete = true;
                EventManager.TriggerEvent("LangChanged");
            }

        }
    }
}
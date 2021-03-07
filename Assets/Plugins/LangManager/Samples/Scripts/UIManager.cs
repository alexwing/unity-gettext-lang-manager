using LangManager;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<TMP_Text> texts;

    public Text simpletext;
    public Text currentLang;

    public Font SourceFont;
    public Font SourcePixelFont;
    public Material SunnyDaysMaterial;
    public List<string> Languages = new List<string>(){
                           "en", //English
                           "es", //Spanish
                           "ja", //Japanese
                           "ko", //Korean
                           "ru", //Russian
                           "zh", //Chinese
                           };

    private readonly NavigationList<string> LanguagesList = new NavigationList<string>();

    public void Awake()
    {
        //make navigation language list
        foreach (string str in Languages)
        {
            LanguagesList.Add(str);
        }
    }
    public async void UpdateLang(string lang)
    {
        await LanguageManager.LoadLangAsync(lang);

        currentLang.text = $"Language [{ lang }]";
        string text = "";
        foreach (TMP_Text langText in texts)
        {
            switch (langText.name)
            {
                case "test01":
                    text = UILangManager.GetString("Sunny Days!", SourceFont, langText, SunnyDaysMaterial);

                    langText.GetComponent<WarpTextEffect>().StartWarp();

                    break;
                case "test02":
                    text = UILangManager.GetString("This is an example of using the TextMesh", SourcePixelFont, langText);
                    break;
                case "test03":
                    text = UILangManager.GetString("Example of multiple linesof text created with TextMeshPro!", SourceFont, langText);
                    break;
            }
           // Debug.Log($"{langText.name} change to {text}");
        }
        text = UILangManager.GetString("This is a simple UI text", SourceFont, simpletext);
       // Debug.Log($"{simpletext.name} change to {text}");
    }
    private void Start()
    {
        UpdateLang(LanguagesList.Current);
    }
    public void NextLang()
    {
        UpdateLang(LanguagesList.MoveNext);
    }

    public void PrevLang()
    {
        UpdateLang(LanguagesList.MovePrevious);
    }
}
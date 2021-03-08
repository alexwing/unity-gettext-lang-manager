using LangManager;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager3D : MonoBehaviour
{
    public List<TMP_Text> texts;

    public TextMesh simpletext;
    public Text currentLang;

    public Font TitleFont;
    public Font TitleFontDetail;
    public Material TitleMaterial;
    public Material TitleMaterialDetail;
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
                case "TextMesh Pro - Text":
                    text = UILangManager.GetString("Sunny Days!", TitleFont, langText, TitleMaterial);
                    break;
                case "TextMesh Pro - Caption":
                    text = UILangManager.GetString("This is a simple UI text", TitleFontDetail, langText, TitleMaterialDetail);
                    break;

            }            
           // Debug.Log($"{langText.name} change to {text}");
        }
        text = UILangManager.GetString("This is a simple UI text", TitleFont, simpletext);
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
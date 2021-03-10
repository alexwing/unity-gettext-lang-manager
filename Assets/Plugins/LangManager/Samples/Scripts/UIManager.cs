using LangManager;
using System.Collections.Generic;
using System.Threading.Tasks;
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


    public void Awake()
    {
        EventManager.StartListening("LangChanged", UpdateLang);
    }

    private void UpdateLang()
    {
        _ = UpdateLangAsync();
    }

    public async Task UpdateLangAsync()
    {
        await LanguageManager.WaitUntil(() => LanguageManager.langChangeComplete);

        currentLang.text = $"Language [{ LanguageManager.LanguagesList.Current }]";
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
        LanguageManager.LoadLang("en");
    }
    public void NextLang()
    {
        LanguageManager.LoadLang(LanguageManager.LanguagesList.MoveNext);
    }

    public void PrevLang()
    {
        LanguageManager.LoadLang(LanguageManager.LanguagesList.MovePrevious);
    }
}
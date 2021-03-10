using LangManager;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager3D : MonoBehaviour
{
    public List<TMP_Text> texts;

    public TextMesh simpletext;
    public Text currentLang;

    public TextMeshProLang textMeshProLang;

    public Font TitleFont;
    public Font TitleFontDetail;
    public Material TitleMaterial;
    public Material TitleMaterialDetail;

    public void Awake()
    {
        textMeshProLang.GetString("This is a simple UI text");
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
        LanguageManager.LoadLang(LanguageManager.LanguagesList.Current);
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
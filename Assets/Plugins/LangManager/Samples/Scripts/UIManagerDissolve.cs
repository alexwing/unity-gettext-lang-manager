using Coffee.UIExtensions;
using LangManager;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerDissolve : MonoBehaviour
{

    public Text currentLang;

    public TextMeshProLang textMeshProLang;


    public TextMeshPro LangManagerLogo;

    public void Start()
    {
        textMeshProLang.GetString("This is a simple UI text");
        EventManager.StartListening("LangChanged", UpdateLang);
        LanguageManager.LoadLang(LanguageManager.LanguagesList.Current);
    }


    private void UpdateLang()
    {
        _ = UpdateLangAsync();
    }

    public async Task UpdateLangAsync()
    {
        await LanguageManager.WaitUntil(() => LanguageManager.langChangeComplete);

        currentLang.text = $"Language [{ LanguageManager.LanguagesList.Current }]";

        LangManagerLogo.fontSharedMaterial.SetTexture(ShaderUtilities.ID_FaceTex, GetCurrentFlag());

        // Debug.Log($"{simpletext.name} change to {text}");
    }

    public Texture GetCurrentFlag()
    {

        var languageEntries = LanguageFlags.Flags;
        for (int i = 0; i < languageEntries.Count; i++)
        {

            if  (LanguageManager.LanguagesList.Current == languageEntries[i].code)
            {
                Debug.Log("encontrador: "+languageEntries[i].code + languageEntries[i].flag.texture.name);
                return languageEntries[i].flag.texture;
            }

        }

        return null;
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
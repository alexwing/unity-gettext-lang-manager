using Coffee.UIExtensions;
using LangManager;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TextMeshProLang : TextMeshPro
{
    private string LangKeyValue = "";
    private bool FirstLoad = true;
    public string GetString(string text)
    {
        LangKeyValue = text;
        //all GetString functions return translate string for compatibilty
        return LanguageManager.catalog.GetString(text);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDisable()
    {
        EventManager.StopListening("LangChanged", UpdateLang);
        base.OnDisable();
    }

    protected override void OnEnable()
    {
        EventManager.StartListening("LangChanged", UpdateLang);
        base.OnEnable();
    }

    private void UpdateLang()
    {
        if (!string.IsNullOrEmpty(LangKeyValue))
        {
            _ = UpdateLangAsync(FirstLoad);
            FirstLoad = false;
        }
    }

    public async Task UpdateLangAsync(bool firstLoad)
    {
        await LanguageManager.WaitUntil(() => LanguageManager.langChangeComplete);

        if (GetComponent<UIDissolve>())
        {
            GetComponent<UIDissolve>().firstLoad = firstLoad;
            GetComponent<UIDissolve>().Play();
            if(!firstLoad)
            await Task.Delay((int)(GetComponent<UIDissolve>().duration * 0.5f * 1000));
        }

        text = UILangManager.GetString(LangKeyValue, font, this, fontMaterial);
    }
}

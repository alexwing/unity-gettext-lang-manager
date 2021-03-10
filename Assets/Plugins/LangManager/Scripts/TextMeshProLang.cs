using LangManager;
using System.Threading.Tasks;
using TMPro;

public class TextMeshProLang : TextMeshPro
{
    private string LangKeyValue = "";

    public string GetString(string text)
    {
         LangKeyValue = text;
        //all GetString functions return translate string for compatibilty
        return LanguageManager.catalog.GetString(text);
    }

    protected override void Awake()
    {
        EventManager.StartListening("LangChanged", UpdateLang);
        base.Awake();
        UpdateLang();
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
        UpdateLang();
    }

    private void UpdateLang()
    {
        if (!string.IsNullOrEmpty(LangKeyValue))
        {
            _ = UpdateLangAsync();
        }
    }

    public async Task UpdateLangAsync()
    {
        await LanguageManager.WaitUntil(() => LanguageManager.langChangeComplete);
        text = UILangManager.GetString(LangKeyValue, font, this);
    }
}

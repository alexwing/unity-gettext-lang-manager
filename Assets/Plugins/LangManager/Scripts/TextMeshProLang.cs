using LangManager;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TextMeshProLang : TextMeshPro
{

    private string GetStringText = "";

    protected override void Awake()
    {
        base.Awake();
        GetStringText = this.text;
        UpdateLang();
        EventManager.StartListening("LangChanged", UpdateLang);

#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += StateChange;
#endif
    }

    protected override void OnDestroy()
    {
        this.text = GetStringText;
        base.OnDestroy();
    }     
    
    private void OnApplicationQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        this.text = GetStringText;
    }
#if UNITY_EDITOR
    void StateChange()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
        {
            this.text = GetStringText;
            EventManager.StopListening("LangChanged", UpdateLang);
        }
 }
#endif
    protected override void OnDisable()
    {
        this.text = GetStringText;
        EventManager.StopListening("LangChanged", UpdateLang);
        base.OnDisable();
    }    
    
    protected override void OnEnable()
    {


        GetStringText = this.text;
        EventManager.StartListening("LangChanged", UpdateLang);
        base.OnEnable();
        UpdateLang();
    }

    private void UpdateLang()
    {
        _ = UpdateLangAsync();
    }

    public async System.Threading.Tasks.Task UpdateLangAsync()
    {
        await LanguageManager.WaitUntil(() => LanguageManager.langChangeComplete);
        text = UILangManager.GetString(GetStringText, font, this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public string currentLanguage = "English";

    private Dictionary<string, string> translations;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTranslations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadTranslations()
    {
        LanguagePack languagePack = LocalizationData.LoadLanguageFile(currentLanguage);
        translations = new Dictionary<string, string>();

        foreach (var translation in languagePack.translations)
        {
            translations.Add(translation.key, translation.value);
        }
    }


    public string GetTranslation(string key)
    {
        if (translations.ContainsKey(key))
        {
            return translations[key];
        }
        else
        {
            Debug.LogWarning($"Translation not found for key: {key}");
            return key; // Вернуть ключ, если перевод не найден
        }
    }

    public void ChangeLanguage(string newLanguage)
    {
        currentLanguage = newLanguage;
        LoadTranslations();
        // Возможно, вы захотите обновить все текстовые элементы в интерфейсе после смены языка
        // Например, с помощью событий или вызовом метода обновления текста в каждом текстовом элементе
    }

    [System.Serializable]
    public class LanguageData
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class LanguagePack
    {
        public List<LanguageData> translations;
    }

    public class LocalizationData
    {
        public static LanguagePack LoadLanguageFile(string language)
        {
            string json = File.ReadAllText($"Assets/Resources/Texts_{language}.json");
            return JsonUtility.FromJson<LanguagePack>(json);
        }
    }
}


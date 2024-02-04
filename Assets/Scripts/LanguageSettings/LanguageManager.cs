using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    public Button RussianLanguageButton;
    public Button EnglishLanguageButton;
    public Button TurkishLanguageButton;

    public Sprite activeSprite;
    public Sprite inactiveSprite;

    public TextMeshProUGUI RussianButtonText;
    public TextMeshProUGUI EnglishButtonText;
    public TextMeshProUGUI TurkishButtonText;

    public TextMeshProUGUI TwoGameMode;
    public TextMeshProUGUI SingleGameMode;
    public TextMeshProUGUI LanguageTitle;
    public TextMeshProUGUI MenuText;
    public TextMeshProUGUI CrossPlayAgainText;
    public TextMeshProUGUI ToePlayAgainText;
    public TextMeshProUGUI CrossWinText;
    public TextMeshProUGUI ToeWinText;
    public TextMeshProUGUI ToeMenuText;
    public TextMeshProUGUI CrossMenuText;
    public TextMeshProUGUI RulesText;
    public TextMeshProUGUI DeveloperGames;

    public Color activeTextColor;
    public Color inactiveTextColor;

    public bool RussianLanguage = true;
    public bool EnglishLanguage = false;
    public bool TurkishLanguage = false;

    void Start()
    {
        if (RussianLanguageButton != null)
        {
            RussianLanguageButton.onClick.AddListener(OnRussianLanguageButtonClick);
        }

        if (EnglishLanguageButton != null)
        {
            EnglishLanguageButton.onClick.AddListener(OnEnglishLanguageButtonClick);
        }

        if (TurkishLanguageButton != null)
        {
            TurkishLanguageButton.onClick.AddListener(OnTurkishLanguageButtonClick);
        }

        UpdateButtonVisuals();
    }

    void Update()
    {
        if (RussianLanguage) {
            TwoGameMode.text = "Игра вдвоём";
            SingleGameMode.text = "Одиночная игра";
            LanguageTitle.text = "Язык";
            RulesText.text = "Игра до 3 очков!";
            DeveloperGames.text = "Игры разработчика";

            MenuText.text = "Меню";
            ToeMenuText.text = "Меню";
            CrossMenuText.text = "Меню";

            CrossPlayAgainText.text = "Играть снова";
            ToePlayAgainText.text = "Играть снова";

            CrossWinText.text = "Крестики победили!";
            ToeWinText.text = "Нолики победили!";

        } else if (EnglishLanguage) {
            TwoGameMode.text = "Two-player game";
            SingleGameMode.text = "Single-player game";
            LanguageTitle.text = "Language";
            RulesText.text = "Game to 3 points!";
            DeveloperGames.text = "Developer's Games";

            MenuText.text = "Menu";
            ToeMenuText.text = "Menu";
            CrossMenuText.text = "Menu";

            CrossPlayAgainText.text = "Play Again";
            ToePlayAgainText.text = "Play Again";

            CrossWinText.text = "Crosses Win!";
            ToeWinText.text = "Toes Win!";
        } else if (TurkishLanguage) {
            TwoGameMode.text = "İki oyunculu oyun";
            SingleGameMode.text = "Tek oyun";
            LanguageTitle.text = "Dil";
            RulesText.text = "Maç 3 puan!";
            DeveloperGames.text = "Geliştirici Oyunları";

            MenuText.text = "Menü";
            ToeMenuText.text = "Menü";
            CrossMenuText.text = "Menü";
            
            CrossPlayAgainText.text = "Tekrar Oyna";
            ToePlayAgainText.text = "Tekrar Oyna";
            CrossWinText.text = "Haçlar Kazandı!";
            ToeWinText.text = "Sıfırlar Kazandı!";
        }
    }

    void OnRussianLanguageButtonClick()
    {
        RussianLanguage = true;
        EnglishLanguage = false;
        TurkishLanguage = false;

        UpdateButtonVisuals();
    }

    void OnEnglishLanguageButtonClick()
    {
        RussianLanguage = false;
        EnglishLanguage = true;
        TurkishLanguage = false;

        UpdateButtonVisuals();
    }

    void OnTurkishLanguageButtonClick()
    {
        RussianLanguage = false;
        EnglishLanguage = false;
        TurkishLanguage = true;

        UpdateButtonVisuals();
    }

    void UpdateButtonVisuals()
    {
        RussianLanguageButton.image.sprite = RussianLanguage ? activeSprite : inactiveSprite;
        EnglishLanguageButton.image.sprite = EnglishLanguage ? activeSprite : inactiveSprite;
        TurkishLanguageButton.image.sprite = TurkishLanguage ? activeSprite : inactiveSprite;

        RussianButtonText.color = RussianLanguage ? activeTextColor : inactiveTextColor;
        EnglishButtonText.color = EnglishLanguage ? activeTextColor : inactiveTextColor;
        TurkishButtonText.color = TurkishLanguage ? activeTextColor : inactiveTextColor;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Image soundIcon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public AudioSource musicAudioSource;

    private bool isMusicOn = true;

    void Start()
    {
        // Инициализация начального состояния иконки звука
        UpdateSoundIcon();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        // Включение/выключение музыки
        if (isMusicOn)
        {
            musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.Pause();
        }

        // Обновление иконки звука
        UpdateSoundIcon();
    }

    void UpdateSoundIcon()
    {
        soundIcon.sprite = isMusicOn ? soundOnSprite : soundOffSprite;

        Color iconColor = soundIcon.color;
        iconColor.a = isMusicOn ? 1f : 0.5f;
        soundIcon.color = iconColor;
    }
}

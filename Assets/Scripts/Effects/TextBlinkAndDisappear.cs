using System.Collections;
using TMPro;
using UnityEngine;

public class TextBlinkAndDisappear : MonoBehaviour
{
    public float blinkSpeed = 1.5f;
    public float disappearTime = 3.0f;

    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Метод для внешнего вызова (например, при активации объекта)
    public void PlayBlinkAndDisappear()
    {
        StartCoroutine(BlinkAndDisappear());
    }

    IEnumerator BlinkAndDisappear()
    {
        float elapsedTime = 0f;

        while (elapsedTime < disappearTime)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);

            Color textColor = text.color;
            textColor.a = alpha;
            text.color = textColor;

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // Устанавливаем непрозрачность текста на 0
        Color finalColor = text.color;
        finalColor.a = 0f;
        text.color = finalColor;
    }
}

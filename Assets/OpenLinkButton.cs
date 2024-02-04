using UnityEngine;
using UnityEngine.UI;

public class OpenLinkButton : MonoBehaviour
{
    public string link = "https://yandex.ru/games/developer?name=JuJus#redir-data=%7B%22http_ref%22%3A%22https%253A%252F%252Fyandex.ru%252Fgames%252Fsearch%253Fquery%253D%2525D1%252586%2525D0%2525B2%2525D0%2525B5%2525D1%252582%2525D0%2525BE%2525D0%2525B2%2525D0%2525B0%2525D1%25258F%252520%2525D0%2525B2%2525D0%2525BD%2525D0%2525B8%2525D0%2525BC%2525D0%2525B0%2525D1%252582%2525D0%2525B5%2525D0%2525BB%2525D1%25258C%2525D0%2525BD%2525D0%2525BE%2525D1%252581%2525D1%252582%2525D1%25258C%2523app%253D287337%22%2C%22rn%22%3A314649138%7D";

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OpenLink);
        }
    }

    public void OpenLink()
    {
        Application.OpenURL(link);
    }
}

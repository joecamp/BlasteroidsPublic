using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenURLButton : MonoBehaviour
{
    [SerializeField] private string url;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick());
    }

    private void OnButtonClick()
    {
        Application.OpenURL(url);
    }
}
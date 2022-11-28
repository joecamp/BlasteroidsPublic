using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MusicToggleButton : MonoBehaviour
{
    Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool newValue)
    {
        Color newColor = Color.white;
        newColor.a = newValue ? 1f : .5f;

        toggle.image.color = newColor;

        AudioManager.Instance.ToggleMuteMusic(!newValue);
    }
}
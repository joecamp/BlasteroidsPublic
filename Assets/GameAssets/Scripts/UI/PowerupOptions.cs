using System;

using UnityEngine;
using UnityEngine.UI;

public class PowerupOptions : UiGroup
{
    [SerializeField] private Button option1Button;
    [SerializeField] private Text option1TitleText;
    [SerializeField] private Text option1DescriptionText;

    [SerializeField] private Button option2Button;
    [SerializeField] private Text option2TitleText;
    [SerializeField] private Text option2DescriptionText;

    [SerializeField] private AudioPlayer powerupSelectedAudioPlayer;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor;

    [SerializeField] private string option1Title;
    [SerializeField] private string option1Description;
    [SerializeField] private string option2Title;
    [SerializeField] private string option2Description;

    [SerializeField] private Powerup option1Powerup;
    [SerializeField] private Powerup option2Powerup;

    private bool isPowerupSelected = false;

    public static Action<Powerup> OnPowerupSelected;

    protected override void Awake()
    {
        base.Awake();

        SetupOptionsText();

        option1Button.onClick.AddListener(() => OnOption1Click());
        option2Button.onClick.AddListener(() => OnOption2Click());
    }

    private void SetupOptionsText()
    {
        option1TitleText.text = option1Title;
        option1DescriptionText.text = option1Description;

        option2TitleText.text = option2Title;
        option2DescriptionText.text = option2Description;
    }

    public void OnOption1Click()
    {
        if (isPowerupSelected) return;

        option1Button.image.color = selectedColor;
        powerupSelectedAudioPlayer.PlayRandomClip();
        isPowerupSelected = true;

        OnPowerupSelected?.Invoke(option1Powerup);

        Deactivate(true);
    }

    public void OnOption2Click()
    {
        if (isPowerupSelected) return;

        option2Button.image.color = selectedColor;
        powerupSelectedAudioPlayer.PlayRandomClip();
        isPowerupSelected = true;

        OnPowerupSelected?.Invoke(option2Powerup);

        Deactivate(true);
    }

    public void OnPointerEnterOption1()
    {
        if (isPowerupSelected) return;

        option1Button.image.color = hoverColor;
    }

    public void OnPointerEnterOption2()
    {
        if (isPowerupSelected) return;

        option2Button.image.color = hoverColor;
    }

    public void OnPointerExitOption1()
    {
        if (isPowerupSelected) return;

        option1Button.image.color = defaultColor;
    }

    public void OnPointerExitOption2()
    {
        if (isPowerupSelected) return;

        option2Button.image.color = defaultColor;
    }

    public void Reset()
    {
        option1Button.image.color = defaultColor;
        option2Button.image.color = defaultColor;

        isPowerupSelected = false;
    }
}
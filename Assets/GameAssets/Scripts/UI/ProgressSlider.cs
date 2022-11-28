using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressSlider : MonoBehaviour
{
    [SerializeField] private Image level1StatusImage;
    [SerializeField] private Image level2StatusImage;
    [SerializeField] private Image level3StatusImage;

    [SerializeField] private Text scoreGoalText;
    [SerializeField] private Text toNextLevelText;

    [SerializeField] private ParticleSystem level1Particles;
    [SerializeField] private ParticleSystem level2Particles;
    [SerializeField] private ParticleSystem level3Particles;

    [SerializeField] private Sprite levelCompleteSprite;
    [SerializeField] private Sprite levelIncompleteSprite;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        GameManager.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int newScore)
    {
        if (newScore == 0)
        {
            return;
        }

        if (scoreGoalText.gameObject.activeSelf)
        {
            switch (GameManager.Instance.Level)
            {
                case Level.Zero:
                    scoreGoalText.text = (GameSettings.Level1ScoreRequirement - newScore).ToString();
                    break;
                case Level.One:
                    scoreGoalText.text = (GameSettings.Level2ScoreRequirement - newScore).ToString();
                    break;
                case Level.Two:
                    scoreGoalText.text = (GameSettings.Level3ScoreRequirement - newScore).ToString();
                    break;
            }
        }

        int newSliderValue = Mathf.FloorToInt((float)newScore / GameSettings.Level3ScoreRequirement * slider.maxValue);

        slider.value = newSliderValue;
    }

    public void SetLevel1Complete()
    {
        slider.value = 10;

        scoreGoalText.text = (GameSettings.Level2ScoreRequirement - GameSettings.Level1ScoreRequirement).ToString();

        SetLevel1FlagSprite(true);

        level1Particles.Play();
    }

    public void SetLevel2Complete()
    {
        slider.value = 20;

        scoreGoalText.text = (GameSettings.Level3ScoreRequirement - GameSettings.Level2ScoreRequirement).ToString();

        SetLevel2FlagSprite(true);

        level2Particles.Play();
    }

    public void SetLevel3Complete()
    {
        slider.value = 30;

        scoreGoalText.gameObject.SetActive(false);
        toNextLevelText.gameObject.SetActive(false);

        SetLevel3FlagSprite(true);

        level3Particles.Play();
    }

    public void ResetProgressSlider()
    {
        slider.value = 0;

        scoreGoalText.text = GameSettings.Level1ScoreRequirement.ToString();
        scoreGoalText.gameObject.SetActive(true);
        toNextLevelText.gameObject.SetActive(true);

        SetLevel1FlagSprite(false);
        SetLevel2FlagSprite(false);
        SetLevel3FlagSprite(false);
    }

    private void SetLevel1FlagSprite(bool value)
    {
        level1StatusImage.sprite = value ? levelCompleteSprite : levelIncompleteSprite;
    }

    private void SetLevel2FlagSprite(bool value)
    {
        level2StatusImage.sprite = value ? levelCompleteSprite : levelIncompleteSprite;
    }

    private void SetLevel3FlagSprite(bool value)
    {
        level3StatusImage.sprite = value ? levelCompleteSprite : levelIncompleteSprite;
    }
}
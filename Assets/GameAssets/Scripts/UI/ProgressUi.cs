using UnityEngine;
using UnityEngine.UI;

public class ProgressUi : MonoBehaviour
{
    [SerializeField] private Text scoreGoalText;
    [SerializeField] private Text toNextLevelText;

    private void Awake()
    {
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
    }

    public void SetLevel1Complete()
    {
        scoreGoalText.text = (GameSettings.Level2ScoreRequirement - GameSettings.Level1ScoreRequirement).ToString();
    }

    public void SetLevel2Complete()
    {
        scoreGoalText.text = (GameSettings.Level3ScoreRequirement - GameSettings.Level2ScoreRequirement).ToString();
    }

    public void SetLevel3Complete()
    {
        scoreGoalText.gameObject.SetActive(false);
        toNextLevelText.gameObject.SetActive(false);
    }

    public void Reset()
    {
        scoreGoalText.text = GameSettings.Level1ScoreRequirement.ToString();
        scoreGoalText.gameObject.SetActive(true);
        toNextLevelText.gameObject.SetActive(true);
    }
}
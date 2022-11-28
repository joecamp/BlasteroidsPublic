using DG.Tweening;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private UiGroup menuUiGroup;
    [SerializeField] private UiGroup gameUiGroup;

    [SerializeField] private List<GameObject> gameOverUiElements;

    [SerializeField] private Text scoreText;
    [SerializeField] private DOTweenAnimation updateScoreAnimation;

    [SerializeField] private ProgressUi progressUi;

    [SerializeField] private PowerupOptions powerupOptionsLevelOne;
    [SerializeField] private PowerupOptions powerupOptionsLevelTwo;
    [SerializeField] private PowerupOptions powerupOptionsLevelThree;

    [SerializeField] private GameObject cheatsUI;

    private void Awake()
    {
        GameManager.OnScoreChanged += OnScoreChanged;
        GameManager.OnGameOver += () => ToggleGameOverUI(true);
    }

    public void SwapToGameUI()
    {
        menuUiGroup.Deactivate(false);
        gameUiGroup.Activate(true);
    }

    public void SwapToMenuUI()
    {
        ToggleGameOverUI(false);

        gameUiGroup.Deactivate(false);
        menuUiGroup.Activate(true);
    }

    public void ToggleGameOverUI(bool toggle)
    {
        foreach (GameObject go in gameOverUiElements)
        {
            go.SetActive(toggle);
        }
    }

    public void SetLevelUI(Level level)
    {
        switch (level)
        {
            case Level.One:
                progressUi.SetLevel1Complete();
                powerupOptionsLevelOne.Activate(true);
                break;
            case Level.Two:
                progressUi.SetLevel2Complete();
                powerupOptionsLevelTwo.Activate(true);
                break;
            case Level.Three:
                progressUi.SetLevel3Complete();
                powerupOptionsLevelThree.Activate(true);
                break;
        }
    }

    public void ResetProgressSlider()
    {
        progressUi.Reset();
    }

    public void ResetPowerupOptions()
    {
        powerupOptionsLevelOne.Reset();
        powerupOptionsLevelTwo.Reset();
        powerupOptionsLevelThree.Reset();
    }

    public void OnScoreChanged(int newScore)
    {
        if (scoreText.text == newScore.ToString())
        {
            return;
        }

        scoreText.text = newScore.ToString();
        updateScoreAnimation.DORestart();
    }

    public void ActivateCheatsUI()
    {
        cheatsUI.SetActive(true);
    }
}
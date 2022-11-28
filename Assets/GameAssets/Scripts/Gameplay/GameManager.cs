using System;
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(SpaceshipSpawnManager))]
public class GameManager : Singleton<GameManager>
{
    public bool IsGamePlaying { get; private set; } = false;
    public Level Level { get; private set; } = Level.Zero;
    public Camera MainCamera { get; private set; }

    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private AudioPlayer levelCompleteAudioPlayer;
    [SerializeField] private AudioPlayer gameOverAudioPlayer;

    [SerializeField] private TutorialAnimation tutorialAnim;
    [SerializeField] private TutorialSpaceship tutorialSpaceship;

    private SpaceshipSpawnManager spaceshipSpawnManager;
    private int score;

    public static Action<int> OnScoreChanged;
    public static Action OnGameOver;

    private void Awake()
    {
        MainCamera = Camera.main;

        spaceshipSpawnManager = GetComponent<SpaceshipSpawnManager>();

        Spaceship.OnSpaceshipDestroyed += OnSpaceshipDestroyed;
        Asteroid.OnAsteroidDestroyed += OnAsteroidDestroyed;
    }

    public void RestartGame()
    {
        Vector3 screenCenter = new Vector3(0f, 0f, 0f);
        SpawnAsteroid(screenCenter);

        if (Utilities.IsFirstPlay())
        {
            SetupTutorial();
        }
        else
        {
            Restart();
        }
    }

    private void Restart()
    {
        Utilities.TryFlagFirstPlay();

        tutorialAnim.gameObject.SetActive(false);

        UiManager.Instance.ToggleGameOverUI(false);
        UiManager.Instance.ResetProgressSlider();
        UiManager.Instance.ResetPowerupOptions();

        PowerupManager.Instance.ResetPowerups();

        CleanupGameScene();

        spaceshipSpawnManager.ShouldSpawnShips = true;

        Level = Level.Zero;
        score = 0;
        OnScoreChanged?.Invoke(score);

        AudioManager.Instance.PlayGameMusic();

        IsGamePlaying = true;
    }

    private void SetupTutorial()
    {
        tutorialAnim.gameObject.SetActive(true);
        tutorialSpaceship.gameObject.SetActive(true);
    }

    private void SpawnAsteroid(Vector3 position)
    {
        Instantiate(asteroidPrefab, position, Utilities.GetRandomRotation2D());
    }

    private void OnSpaceshipDestroyed(Spaceship spaceship)
    {
        if (spaceship.GetComponent<TutorialSpaceship>() != null)
        {
            Restart();
            return;
        }

        score += spaceship.Stats.ScoreReward;
        OnScoreChanged?.Invoke(score);

        switch (Level)
        {
            case Level.Zero:
                if (score >= GameSettings.Level1ScoreRequirement)
                {
                    SetLevel(Level.One);
                }
                break;
            case Level.One:
                if (score >= GameSettings.Level2ScoreRequirement)
                {
                    SetLevel(Level.Two);
                }
                break;
            case Level.Two:
                if (score >= GameSettings.Level3ScoreRequirement)
                {
                    SetLevel(Level.Three);
                }
                break;
        }
    }

    private void SetLevel(Level level)
    {
        Level = level;
        UiManager.Instance.SetLevelUI(Level);
        levelCompleteAudioPlayer.PlayRandomClip();
        Debug.Log("Next Level! Now on level " + Level);

        Vector3 screenCenter = new Vector3(0f, 0f, 0f);
        SpawnAsteroid(screenCenter);

        foreach (Asteroid asteroid in FindObjectsOfType<Asteroid>())
        {
            asteroid.HaltMovement();
        }

        CleanupGameScene();

        spaceshipSpawnManager.ShouldSpawnShips = false;
    }

    private void OnAsteroidDestroyed()
    {
        // Need to wait for end of frame for asteroids to be destroyed
        StopAllCoroutines();
        StartCoroutine(CheckForGameEndCoroutine());
    }

    private IEnumerator CheckForGameEndCoroutine()
    {
        yield return new WaitForEndOfFrame();

        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

        if (asteroids.Length == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        IsGamePlaying = false;
        spaceshipSpawnManager.ShouldSpawnShips = false;
        AudioManager.Instance.TogglePauseLoopingMusic(true);
        gameOverAudioPlayer.PlayRandomClip();
        Speedup();
        OnGameOver?.Invoke();
    }

    public void CleanupGameScene()
    {
        spaceshipSpawnManager.DestroyAllSpaceships();

        foreach (Projectile projectile in FindObjectsOfType<Projectile>())
        {
            Destroy(projectile.gameObject);
        }
    }

    public void Slowdown()
    {
        SetGameSimulationSpeed(.5f);
        AudioManager.Instance.SetGlobalPitch(.5f);
    }

    public void Speedup()
    {
        SetGameSimulationSpeed(1f);
        AudioManager.Instance.SetGlobalPitch(1f);
    }

    private void SetGameSimulationSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
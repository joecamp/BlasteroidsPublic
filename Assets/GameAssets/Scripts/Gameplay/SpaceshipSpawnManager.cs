using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpaceshipSpawnManager : MonoBehaviour
{
    public bool ShouldSpawnShips = false;

    [SerializeField] private GameObject spaceshipSpawnPrefab;
    [SerializeField] private GameObject shooterSpaceshipPrefab;
    [SerializeField] private GameObject laserSpaceshipPrefab;
    [SerializeField] private GameObject bombSpaceshipPrefab;

    [SerializeField] private List<Spaceship> activeSpaceships = new List<Spaceship>();

    private float spaceshipSpawnDelay = 2.3f;
    private float spaceshipSpawnCounter = 0f;

    private void Awake()
    {
        Spaceship.OnSpaceshipDestroyed += OnSpaceshipDestroyed;
        Spaceship.OnSpaceshipRemoved += OnSpaceshipDestroyed;
        PowerupOptions.OnPowerupSelected += OnPowerupSelected;
    }

    private void Update()
    {
        if (ShouldSpawnShips && activeSpaceships.Count < Utilities.GetMaxSpaceshipsByLevel(GameManager.Instance.Level))
        {
            spaceshipSpawnCounter += Time.deltaTime;
            if (spaceshipSpawnCounter >= Utilities.GetTimeBetweenSpawnsByLevel(GameManager.Instance.Level))
            {
                SpawnSpaceships(Utilities.GetNumberOfSpawnsByLevel(GameManager.Instance.Level));
                spaceshipSpawnCounter = 0f;
            }
        }
    }

    private void SpawnSpaceships(int spaceshipCount)
    {
        for (int i = 0; i < spaceshipCount; i++)
        {
            StartCoroutine(SpawnSpaceshipCoroutine());
        }
    }

    private IEnumerator SpawnSpaceshipCoroutine()
    {
        Vector3 position = GetRandomPositionInScene();
        Quaternion rotation = Utilities.GetRandomRotation2D();

        GameObject spaceshipPrefab;
        SpaceshipStats stats;

        int spaceshipIndex = Utilities.GetSpaceshipIndexByLevel(GameManager.Instance.Level);
        switch (spaceshipIndex)
        {
            case 0:
                spaceshipPrefab = shooterSpaceshipPrefab;
                stats = SpaceshipFactory.GenerateShooterStats();
                break;
            case 1:
                spaceshipPrefab = laserSpaceshipPrefab;
                stats = SpaceshipFactory.GenerateLaserStats();
                break;
            case 2:
                spaceshipPrefab = bombSpaceshipPrefab;
                stats = SpaceshipFactory.GenerateBombStats();
                break;
            default:
                spaceshipPrefab = shooterSpaceshipPrefab;
                stats = SpaceshipFactory.GenerateShooterStats();
                break;
        }

        Instantiate(spaceshipSpawnPrefab, position, rotation);

        yield return new WaitForSeconds(spaceshipSpawnDelay);

        Spaceship newSpaceship = Instantiate(spaceshipPrefab, position, rotation).GetComponent<Spaceship>();
        newSpaceship.Stats = stats;
        activeSpaceships.Add(newSpaceship);
    }

    private void OnSpaceshipDestroyed(Spaceship spaceship)
    {
        activeSpaceships.Remove(spaceship);
    }

    public void DestroyAllSpaceships()
    {
        SpaceshipSpawn[] activeSpawns = FindObjectsOfType<SpaceshipSpawn>();

        for (int i = 0; i < activeSpawns.Length; i++)
        {
            Destroy(activeSpawns[i].gameObject);
        }

        StopAllCoroutines();

        for (int i = 0; i < activeSpaceships.Count; i++)
        {
            if (activeSpaceships[i] != null)
            {
                Destroy(activeSpaceships[i].gameObject);
            }
        }

        activeSpaceships.Clear();
    }

    private void OnPowerupSelected(Powerup powerup)
    {
        ShouldSpawnShips = true;
    }

    private Vector3 GetRandomPositionInScene()
    {
        // subtract 1 to retain border around edges of screen
        float cameraSize = GameManager.Instance.MainCamera.orthographicSize - 1;

        float randomX = Random.Range(-cameraSize, cameraSize);
        float randomY = Random.Range(-cameraSize, cameraSize);
        Vector3 position = new Vector3(randomX, randomY, 0f);

        return position;
    }
}
using System;

using UnityEngine;
using Random = UnityEngine.Random;

public static class Utilities
{
    public static Asteroid GetClosestAsteroid(Spaceship spaceship, Asteroid[] asteroids)
    {
        if (asteroids.Length == 0) return null;

        // Find closest asteroid
        Asteroid closestAsteroid = asteroids[0];
        float smallestDist = Vector3.Distance(spaceship.transform.position, closestAsteroid.transform.position);

        foreach (Asteroid asteroid in asteroids)
        {
            float dist = Vector3.Distance(spaceship.transform.position, asteroid.transform.position);
            if (smallestDist > dist)
            {
                closestAsteroid = asteroid;
                smallestDist = dist;
            }
        }

        return closestAsteroid;
    }

    public static Asteroid GetFurthestAsteroid(ShooterSpaceship spaceship, Asteroid[] asteroids)
    {
        if (asteroids.Length == 0) return null;

        // Find closest asteroid
        Asteroid furthestAsteroid = asteroids[0];
        float largestDist = Vector3.Distance(spaceship.transform.position, furthestAsteroid.transform.position);

        foreach (Asteroid asteroid in asteroids)
        {
            float dist = Vector3.Distance(spaceship.transform.position, asteroid.transform.position);
            if (largestDist < dist)
            {
                furthestAsteroid = asteroid;
                largestDist = dist;
            }
        }

        return furthestAsteroid;
    }

    public static Asteroid GetRandomAsteroid(Asteroid[] asteroids)
    {
        if (asteroids.Length == 0) return null;

        return asteroids[Random.Range(0, asteroids.Length)];
    }

    public static float GetTimeBetweenSpawnsByLevel(Level level)
    {
        switch (level)
        {
            case Level.Zero:
                return GameSettings.levelZeroTimeBetweenSpawns;
            case Level.One:
                return GameSettings.levelOneTimeBetweenSpawns;
            case Level.Two:
                return GameSettings.levelTwoTimeBetweenSpawns;
            case Level.Three:
                return GameSettings.levelThreeTimeBetweenSpawns;
            default:
                return GameSettings.levelZeroTimeBetweenSpawns;
        }
    }

    public static int GetMaxSpaceshipsByLevel(Level level)
    {
        switch (level)
        {
            case Level.Zero:
                return GameSettings.levelZeroMaxSpaceships;
            case Level.One:
                return GameSettings.levelOneMaxSpaceships;
            case Level.Two:
                return GameSettings.levelTwoMaxSpaceships;
            case Level.Three:
                return GameSettings.levelThreeMaxSpaceships;
            default:
                return GameSettings.levelZeroMaxSpaceships;
        }
    }

    public static int GetNumberOfSpawnsByLevel(Level level)
    {
        int minSpawns;
        int maxSpawns;

        switch (level)
        {
            case Level.Zero:
                minSpawns = GameSettings.levelZeroMinSpawns;
                maxSpawns = GameSettings.levelZeroMaxSpawns;
                break;
            case Level.One:
                minSpawns = GameSettings.levelOneMinSpawns;
                maxSpawns = GameSettings.levelOneMaxSpawns;
                break;
            case Level.Two:
                minSpawns = GameSettings.levelTwoMinSpawns;
                maxSpawns = GameSettings.levelTwoMaxSpawns;
                break;
            case Level.Three:
                minSpawns = GameSettings.levelThreeMinSpawns;
                maxSpawns = GameSettings.levelThreeMaxSpawns;
                break;
            default:
                minSpawns = GameSettings.levelZeroMinSpawns;
                maxSpawns = GameSettings.levelZeroMaxSpawns;
                break;
        }

        return Random.Range(minSpawns, maxSpawns + 1);
    }

    public static int GetSpaceshipIndexByLevel(Level level)
    {
        float shooterChance = 1;
        float laserChance = 1;
        float bombChance = 1;

        switch (level)
        {
            case Level.Zero:
                shooterChance = GameSettings.levelZeroShooterChance;
                laserChance = GameSettings.levelZeroLaserChance;
                bombChance = GameSettings.levelZeroBombChance;
                break;
            case Level.One:
                shooterChance = GameSettings.levelOneShooterChance;
                laserChance = GameSettings.levelOneLaserChance;
                bombChance = GameSettings.levelOneBombChance;
                break;
            case Level.Two:
                shooterChance = GameSettings.levelTwoShooterChance;
                laserChance = GameSettings.levelTwoLaserChance;
                bombChance = GameSettings.levelTwoBombChance;
                break;
            case Level.Three:
                shooterChance = GameSettings.levelThreeShooterChance;
                laserChance = GameSettings.levelThreeLaserChance;
                bombChance = GameSettings.levelThreeBombChance;
                break;
        }

        if (Random.value < shooterChance)
        {
            return 0;
        }
        if (Random.value < laserChance)
        {
            return 1;
        }
        if (Random.value < bombChance)
        {
            return 2;
        }

        return 0;
    }

    public static bool IsFirstPlay()
    {
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void TryFlagFirstPlay()
    {
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            PlayerPrefs.SetInt("FirstPlay", 0);
        }
    }

    public static Quaternion GetRandomRotation2D()
    {
        Vector3 eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 359f));

        return Quaternion.Euler(eulerAngles);
    }

    public static Vector2 GetRandomDirection2D()
    {
        return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }

    public static T GetRandomEnum<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int randomIndex = Random.Range(0, values.Length);

        return (T)values.GetValue(randomIndex);
    }
}
using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private List<DOTweenAnimation> shipDestroyedAnimations;
    [SerializeField] private List<DOTweenAnimation> asteroidHitAnimations;

    private void Awake()
    {
        Spaceship.OnSpaceshipDestroyed += OnSpaceshipDestroyed;
        Asteroid.OnAsteroidHit += OnAsteroidHit;
        Asteroid.OnAsteroidDestroyed += OnAsteroidHit;
    }

    private void OnSpaceshipDestroyed(Spaceship ship)
    {
        foreach (DOTweenAnimation anim in shipDestroyedAnimations)
        {
            anim.DORestart();
        }
    }

    private void OnAsteroidHit()
    {
        foreach (DOTweenAnimation anim in asteroidHitAnimations)
        {
            anim.DORestart();
        }

        Hitstop(.1f);
    }

    private void Hitstop(float duration)
    {
        StartCoroutine(HitstopCoroutine(duration));
    }

    private IEnumerator HitstopCoroutine(float duration)
    {
        float initTimescale = Time.timeScale;
        if (initTimescale == 0) initTimescale = 1f;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = initTimescale;
    }
}
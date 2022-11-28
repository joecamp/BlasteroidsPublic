using DG.Tweening;

using UnityEngine;

public class LaserSpaceship : Spaceship
{
    [SerializeField] private float timeBetweenShots = 3f;
    [SerializeField] private Vector2 initialWarmupRange = new Vector2(1f, 5f);

    [SerializeField] private DOTweenAnimation fireShotAnimation;
    [SerializeField] private ParticleSystem chargeShotParticles;
    [SerializeField] private Transform fireShotTransform;
    [SerializeField] private ParticleSystem laserParticleSystem;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioPlayer chargeShotAudioPlayer;
    [SerializeField] private AudioPlayer fireShotAudioPlayer;

    private float timeToChargeShot = 1.55f;
    private float laserRaycastDelay = .1f;
    private float raycastDistance = 10f;
    private float laserWidth = .8f;

    private void Start()
    {
        InvokeRepeating("ChargeShot", Random.Range(initialWarmupRange.x, initialWarmupRange.y), timeBetweenShots);
    }

    private void ChargeShot()
    {
        if (target == null) return;

        chargeShotParticles.Play();
        chargeShotAudioPlayer.PlayRandomClip();

        Invoke("ActivateLaser", timeToChargeShot);
        Invoke("LaserRaycast", timeToChargeShot + laserRaycastDelay);
    }

    private void ActivateLaser()
    {
        fireShotAnimation.DORestart();
        fireShotAudioPlayer.PlayRandomClip();

        laserParticleSystem.Play();
    }

    private void LaserRaycast()
    {
        RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(transform.position, laserWidth, transform.up, raycastDistance);

        foreach (RaycastHit2D hitInfo in raycastHits)
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            Asteroid hitAsteroid;
            if (hitAsteroid = hitObject.GetComponent<Asteroid>())
            {
                hitAsteroid.ProcessHit();
            }
        }
    }

    protected override void PlayShipDestroyedEffects()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
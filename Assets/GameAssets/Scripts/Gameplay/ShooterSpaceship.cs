using DG.Tweening;

using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class ShooterSpaceship : Spaceship
{
    [SerializeField] private float timeBetweenShots = 3f;
    [SerializeField] private Vector2 initialWarmupRange = new Vector2(2f, 4f);

    [SerializeField] private DOTweenAnimation fireShotAnimation;
    [SerializeField] private ParticleSystem chargeShotParticles;
    [SerializeField] private Transform fireShotTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioPlayer chargeShotAudioPlayer;
    [SerializeField] private AudioPlayer fireShotAudioPlayer;

    private float timeToChargeShot = 1f;

    private void Start()
    {
        InvokeRepeating("ChargeShot", Random.Range(initialWarmupRange.x, initialWarmupRange.y), timeBetweenShots);
    }

    private void ChargeShot()
    {
        if (target == null) return;

        chargeShotParticles.Play();
        chargeShotAudioPlayer.PlayRandomClip();

        Invoke("FireProjectile", timeToChargeShot);
    }

    private void FireProjectile()
    {
        fireShotAnimation.DORestart();
        fireShotAudioPlayer.PlayRandomClip();

        Instantiate(projectilePrefab, fireShotTransform.position, transform.rotation);
    }

    protected override void PlayShipDestroyedEffects()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
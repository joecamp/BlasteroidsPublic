using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BombSpaceship : Spaceship
{
    [SerializeField] private ParticleSystem chargeBombParticles;
    [SerializeField] private GameObject destroyPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioPlayer chargeBombAudioPlayer;

    private float timeToPrimeExplosion = 1.3f;
    private float detectionRadius = 3.5f;
    private float explosionRadius = 3f;
    private bool isChargingExplosion = false;

    protected override void Update()
    {
        base.Update();

        // Check if asteroid is in range
        if (isChargingExplosion == false)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.GetComponent<Asteroid>())
                {
                    PrimeExplosion();
                    isChargingExplosion = true;
                }
            }
        }
    }

    private void PrimeExplosion()
    {
        if (target == null) return;

        chargeBombParticles.Play();
        chargeBombAudioPlayer.PlayRandomClip();

        Invoke("BombExplode", timeToPrimeExplosion);
    }

    private void BombExplode()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // Check for asteroids in range and destroy them
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in hitColliders)
        {
            Asteroid hitAsteroid = collider.GetComponent<Asteroid>();

            if (hitAsteroid)
            {
                hitAsteroid.ProcessHit();
            }
        }

        Destroy(gameObject);

        OnSpaceshipRemoved?.Invoke(this);
    }

    protected override void PlayShipDestroyedEffects()
    {
        Instantiate(destroyPrefab, transform.position, Quaternion.identity);
    }
}
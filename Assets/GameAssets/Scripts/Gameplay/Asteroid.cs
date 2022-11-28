using System.Collections.Generic;
using System;

using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : ScreenWrappingObject
{
    [SerializeField] private float grabToStopSpeed = 10f;
    [SerializeField] private bool isSmallAsteroid = false;
    [SerializeField] private List<ParticleSystem> shieldParticles;
    [SerializeField] private AudioPlayer flingAudioPlayer;
    [SerializeField] private GameObject smallAsteroidPrefab;
    [SerializeField] private GameObject explosionPrefab;

    public bool IsGrabbed { get; private set; } = false;
    public bool IsShielded { get; private set; } = false;

    private new Rigidbody2D rigidbody;

    public static Action OnAsteroidHit;
    public static Action OnAsteroidDestroyed;

    private float maxSpeed = 45f;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateWhileGrabbed();

        LockVelocityToMax();
    }

    private void OnEnable()
    {
        Vortex.OnVortexPulse += OnVortexPulse;
    }

    private void OnDisable()
    {
        Vortex.OnVortexPulse -= OnVortexPulse;
    }

    private void UpdateWhileGrabbed()
    {
        if (IsGrabbed)
        {
            // Move towards zero velocity over time. Looks better than jumping right to zero velocity.
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Vector2.zero, Time.deltaTime * grabToStopSpeed);
        }
    }

    public void Grab()
    {
        rigidbody.bodyType = RigidbodyType2D.Kinematic;

        IsGrabbed = true;
    }

    public void Fling(Vector2 direction, float force, bool playAudio = true)
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        rigidbody.AddForce(direction * force);

        if (playAudio)
        {
            flingAudioPlayer.PlayRandomClip();
        }

        if (PowerupManager.Instance.ShieldAsteroidsOnFling)
        {
            ActivateShield();
        }

        IsGrabbed = false;
    }

    public void HaltMovement()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
    }

    private void ActivateShield()
    {
        IsShielded = true;

        foreach (ParticleSystem particle in shieldParticles)
        {
            particle.Play();
        }

        Invoke("DeactivateShield", PowerupManager.Instance.ShieldUptime);
    }

    private void DeactivateShield()
    {
        IsShielded = false;

        foreach (ParticleSystem particle in shieldParticles)
        {
            particle.Stop();
        }
    }

    private void OnVortexPulse(Vortex vortex)
    {
        if (IsGrabbed) return;

        Vector2 direction = vortex.transform.position - transform.position;
        float force = PowerupManager.Instance.VortexForce;

        rigidbody.AddForce(direction * force);
    }

    public void ApplyRandomAngularVelocity()
    {
        rigidbody.angularVelocity = Random.Range(-500, 500);
    }

    public void ProcessHit(Projectile projectile = null)
    {
        if (IsShielded) return;

        OnAsteroidHit();

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (PowerupManager.Instance.AreCheatsOn) return;

        if (isSmallAsteroid)
        {
            Destroy(gameObject);

            OnAsteroidDestroyed();
        }
        else
        {
            if (projectile)
            {
                // Calculate direction from projectile to this asteroid
                Vector2 hitDirection = (transform.position - projectile.transform.position).normalized;

                InstantiateSmallAsteroids(hitDirection);
            }
            else
            {
                InstantiateSmallAsteroids(Utilities.GetRandomDirection2D());
            }

            Destroy(gameObject);
        }
    }

    private void InstantiateSmallAsteroids(Vector2 hitDirection)
    {
        for (int i = 0; i < PowerupManager.Instance.NumberOfSmallAsteroidsToSpawn; i++)
        {
            Vector3 position = transform.position + (Random.onUnitSphere * .75f);

            //Debug.Log("Spawning small asteroid at position: " + position);

            GameObject newAsteroidGO = Instantiate(smallAsteroidPrefab, position, Quaternion.identity);
            Asteroid newAsteroid = newAsteroidGO.GetComponent<Asteroid>();

            // Add randomness to direction of fling
            //hitDirection += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            hitDirection += hitDirection * Random.Range(-.25f, .25f);

            newAsteroid.Fling(
                hitDirection,
                Random.Range(150, 350),
                false);

            newAsteroid.ApplyRandomAngularVelocity();
        }
    }

    private void LockVelocityToMax()
    {
        float mag = rigidbody.velocity.magnitude;
        if (mag >= maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
    }
}
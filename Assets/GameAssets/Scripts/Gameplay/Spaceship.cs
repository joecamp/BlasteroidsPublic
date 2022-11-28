using System;

using UnityEngine;

public abstract class Spaceship : ScreenWrappingObject
{
    public SpaceshipStats Stats;
    [SerializeField] protected float rotationSpeed = 1f;
    [SerializeField] protected bool shouldRotate = true;
    [SerializeField] protected float minimumFatalVelocity = 8f;
    protected Asteroid target;

    public static Action<Spaceship> OnSpaceshipDestroyed;
    public static Action<Spaceship> OnSpaceshipRemoved;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Update()
    {
        if (shouldRotate)
        {
            RotateTowardsTarget();
        }

        if (target == null && GameManager.Instance.IsGamePlaying)
        {
            TargetNewAsteroid();
        }
    }

    protected void RotateTowardsTarget()
    {
        if (target == null)
        {
            return;
        }

        var offset = -90;
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.Euler(Vector3.forward * (angle + offset));

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }

    protected void TargetNewAsteroid()
    {
        target = Utilities.GetClosestAsteroid(this, FindObjectsOfType<Asteroid>());
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid;

        if (asteroid = collision.gameObject.GetComponent<Asteroid>())
        {
            float collisionVelocity = collision.relativeVelocity.magnitude;

            //Debug.Log(collision.gameObject.name + " collided with spaceship with velocity of " +
            //    collisionVelocity);

            if (collisionVelocity >= minimumFatalVelocity)
            {
                Explode();
            }
        }
    }

    public void Explode()
    {
        OnSpaceshipDestroyed?.Invoke(this);
        PlayShipDestroyedEffects();

        Destroy(gameObject);

        if (PowerupManager.Instance.ChainReaction)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PowerupManager.Instance.ChainReactionRadius);

            foreach (Collider2D collider in colliders)
            {
                Spaceship spaceship;
                if (spaceship = collider.GetComponent<Spaceship>())
                {
                    if (spaceship != this)
                    {
                        spaceship.Explode();
                    }
                }
            }
        }
    }

    protected abstract void PlayShipDestroyedEffects();
}
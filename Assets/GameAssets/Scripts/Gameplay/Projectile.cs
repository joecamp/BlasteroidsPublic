using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifespan = 10f;
    [SerializeField] private GameObject deathPrefab;

    private void Awake()
    {
        Destroy(gameObject, lifespan);
    }

    private void Update()
    {
        // Move forward
        Vector3 newPosition = transform.position;
        newPosition += transform.up * Time.deltaTime * speed;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Asteroid hitAsteroid;
        Spaceship hitSpaceship;

        if (hitAsteroid = collision.gameObject.GetComponent<Asteroid>())
        {
            hitAsteroid.ProcessHit(this);

            Destroy(gameObject);
        }
        else if (hitSpaceship = collision.gameObject.GetComponent<Spaceship>())
        {
            Destroy(gameObject);
        }

        Instantiate(deathPrefab, transform.position, Quaternion.identity);
    }
}
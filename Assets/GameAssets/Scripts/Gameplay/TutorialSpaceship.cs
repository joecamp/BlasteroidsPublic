using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TutorialSpaceship : Spaceship
{
    [SerializeField] private GameObject explosionPrefab;

    protected override void PlayShipDestroyedEffects()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
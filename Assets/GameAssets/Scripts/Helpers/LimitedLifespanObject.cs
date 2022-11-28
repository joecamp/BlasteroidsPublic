using UnityEngine;

public class LimitedLifespanObject : MonoBehaviour
{
    [SerializeField] private float lifespan = 5f;

    private void Awake()
    {
        Destroy(gameObject, lifespan);
    }
}
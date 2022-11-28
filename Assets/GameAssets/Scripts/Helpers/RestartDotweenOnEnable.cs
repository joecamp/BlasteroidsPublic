using DG.Tweening;

using UnityEngine;

[RequireComponent(typeof(DOTweenAnimation))]
public class RestartDotweenOnEnable : MonoBehaviour
{
    private DOTweenAnimation anim;

    private void Awake()
    {
        anim = GetComponent<DOTweenAnimation>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        anim.DORestart();
    }
}
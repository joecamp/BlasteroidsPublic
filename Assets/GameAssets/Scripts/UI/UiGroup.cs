using DG.Tweening;

using System.Collections;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UiGroup : MonoBehaviour
{
    [SerializeField] private float alphaFadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private DOTweenAnimation anim;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        anim = GetComponent<DOTweenAnimation>();
    }

    public virtual void Activate(bool alphaOverTime = false)
    {
        if (anim != null)
        {
            if (anim.enabled)
            {
                anim.DOComplete();
            }
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        if (alphaOverTime)
        {
            StartCoroutine(ToAlphaCoroutine(1f, alphaFadeDuration));
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
    }

    public virtual void Deactivate(bool alphaOverTime = false)
    {
        if (anim != null)
        {
            if (anim.enabled)
            {
                anim.DOComplete();
            }
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        if (alphaOverTime)
        {
            StartCoroutine(ToAlphaCoroutine(0f, alphaFadeDuration));
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }

    private IEnumerator ToAlphaCoroutine(float newAlpha, float duration)
    {
        float initAlpha = canvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(initAlpha, newAlpha, t / duration);

            yield return null;
        }

        canvasGroup.alpha = newAlpha;
    }
}
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class InitializeAndDeactivateOnAwake : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DeactivateAtEndOfFrame());
    }

    private IEnumerator DeactivateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;

        gameObject.SetActive(false);
    }
}
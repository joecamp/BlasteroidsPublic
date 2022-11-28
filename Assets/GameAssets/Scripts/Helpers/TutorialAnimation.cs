using System.Collections;

using UnityEngine;

public class TutorialAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 lineStartPosition = Vector3.zero;
    [SerializeField] private Vector3 lineEndPosition;

    [SerializeField] private Vector3 cursorStartPosition;

    [SerializeField] private Sprite openHandSprite;
    [SerializeField] private Sprite closedHandSprite;

    [SerializeField] private float cursorSpeed = 1f;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform tutorialCursorTransform;
    [SerializeField] private SpriteRenderer tutorialCursorRenderer;

    private void Start()
    {
        StartCoroutine(AnimationCoroutine());
    }

    float t = 0f;

    private IEnumerator AnimationCoroutine()
    {
        while (true)
        {
            // Set cursor sprite to open hand
            tutorialCursorRenderer.sprite = openHandSprite;

            t = 0f;

            // Move cursor sprite from cursorStartPosition to lineStartPosition
            while (t < 1f)
            {
                t += Time.deltaTime * cursorSpeed;

                tutorialCursorTransform.position = Vector3.Lerp(cursorStartPosition, lineStartPosition, t);

                yield return null;
            }

            // Set cursor sprite to closed hand
            tutorialCursorRenderer.sprite = closedHandSprite;

            // Move cursor sprite from lineStartPosition to lineEndPosition
            // and update line renderer

            lineRenderer.enabled = true;

            t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * cursorSpeed;

                tutorialCursorTransform.position = Vector3.Lerp(lineStartPosition, lineEndPosition, t);

                UpdateLine();

                yield return null;
            }

            yield return new WaitForSeconds(1f);

            lineRenderer.enabled = false;
        }
    }

    private void UpdateLine()
    {
        lineRenderer.SetPositions(new Vector3[] {
                lineStartPosition,
                tutorialCursorTransform.position
            });

        // Fix texture stretching by directly updating the scale
        float width = lineRenderer.startWidth;
        lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }
}
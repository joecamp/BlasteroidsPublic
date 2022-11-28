using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    public Vector3 MouseWorldPosition { get; private set; } = Vector3.zero;

    [SerializeField] private float minDragDistance = .5f;
    [SerializeField] private float maxDragDistance = 5f;

    [SerializeField] private Texture2D openCursorTexture;
    [SerializeField] private Texture2D closedCursorTexture;

    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Vortex vortex;

    private Camera mainCamera;
    private Asteroid grabbedAsteroid = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        SetCursor(openCursorTexture);
    }

    private void Update()
    {
        UpdateMouseWorldPosition();

        CheckMouseInput();

        UpdateLine();

        CheckIfGrabbedAsteroidIsDestroyed();
    }

    private void UpdateMouseWorldPosition()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        MouseWorldPosition = newPosition;
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (grabbedAsteroid == null)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(MouseWorldPosition, Vector2.zero, Mathf.Infinity, raycastLayerMask);

                if (hitInfo.collider != null)
                {
                    GameObject hitObject = hitInfo.collider.gameObject;
                    Asteroid hitAsteroid;
                    if (hitAsteroid = hitObject.GetComponentInParent<Asteroid>())
                    {
                        GrabAsteroid(hitAsteroid);
                        SetCursor(closedCursorTexture);
                    }
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (grabbedAsteroid != null)
            {
                ReleaseAsteroid();
            }

            SetCursor(openCursorTexture);
        }

        if (Input.GetMouseButtonDown(1) && PowerupManager.Instance.VortexOnRightMouse)
        {
            vortex.SetActive(true);
        }

        else if (Input.GetMouseButtonUp(1) && PowerupManager.Instance.VortexOnRightMouse)
        {
            vortex.SetActive(false);
        }
    }

    private void CheckIfGrabbedAsteroidIsDestroyed()
    {
        if (grabbedAsteroid == null && PowerupManager.Instance.SlowdownOnAsteroidDrag)
        {
            GameManager.Instance.Speedup();
        }
    }

    private void UpdateLine()
    {
        if (grabbedAsteroid == null)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;

            Vector3 startLinePoint = grabbedAsteroid.transform.position;
            Vector3 endLinePoint = MouseWorldPosition;

            if (Vector3.Distance(startLinePoint, endLinePoint) > maxDragDistance)
            {
                // Calculate new endLinePoint at maxDistance from startLinePoint
                Vector3 direction = (endLinePoint - startLinePoint).normalized;
                endLinePoint = startLinePoint + direction * maxDragDistance;
            }

            lineRenderer.SetPositions(new Vector3[] {
                startLinePoint,
                endLinePoint
            });

            // Fix texture stretching by directly updating the scale
            float width = lineRenderer.startWidth;
            lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
        }
    }

    private void SetCursor(Texture2D cursorTexture)
    {
        Vector2 cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    private void GrabAsteroid(Asteroid asteroid)
    {
        if (Utilities.IsFirstPlay())
        {
            TutorialAnimation tutorialAnim = FindObjectOfType<TutorialAnimation>();
            if (tutorialAnim != null)
            {
                tutorialAnim.gameObject.SetActive(false);
            }
        }

        grabbedAsteroid = asteroid;

        grabbedAsteroid.Grab();

        if (PowerupManager.Instance.SlowdownOnAsteroidDrag)
        {
            GameManager.Instance.Slowdown();
        }
    }

    private void ReleaseAsteroid()
    {
        Vector2 mousePosition = MouseWorldPosition.AsVector2();

        // Calculate force to apply to fling
        float distance = Vector2.Distance(mousePosition, grabbedAsteroid.transform.position);
        distance = Mathf.Clamp(distance, 0f, maxDragDistance);

        if (distance > minDragDistance)
        {
            float force = distance * PowerupManager.Instance.FlingPower;

            // Calculate direction from mouse to asteroid
            Vector2 direction = (grabbedAsteroid.transform.position.AsVector2() - mousePosition).normalized;

            grabbedAsteroid.Fling(direction, force);
        }

        grabbedAsteroid = null;

        if (PowerupManager.Instance.SlowdownOnAsteroidDrag)
        {
            GameManager.Instance.Speedup();
        }
    }
}
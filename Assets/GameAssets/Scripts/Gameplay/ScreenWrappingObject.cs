using UnityEngine;

public class ScreenWrappingObject : MonoBehaviour
{
    protected Camera mainCamera;

    protected virtual void Awake()
    {
        mainCamera = Camera.main;
    }

    protected virtual void FixedUpdate()
    {
        CheckExitScreen();
    }

    private void CheckExitScreen()
    {
        if (mainCamera == null)
        {
            return;
        }

        // Reached the right/left bounds of the screen
        if (Mathf.Abs(transform.position.x) > (mainCamera.orthographicSize * mainCamera.aspect))
        {
            transform.position = new Vector3(-Mathf.Sign(transform.position.x) *
                mainCamera.orthographicSize * mainCamera.aspect, transform.position.y, 0);

            // Offset a little bit to avoid looping back & forth between the 2 edges 
            transform.position -= transform.position.normalized * 0.1f;
        }

        // Reached the top/bottom bounds of the screen
        if (Mathf.Abs(transform.position.y) > mainCamera.orthographicSize)
        {
            transform.position = new Vector3(transform.position.x,
                -Mathf.Sign(transform.position.y) * mainCamera.orthographicSize, 0);

            // Offset a little bit to avoid looping back & forth between the 2 edges 
            transform.position -= transform.position.normalized * 0.1f;
        }
    }
}
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera ownCam;

    public Transform camAnchor;
    public Transform camTransform;

    public Transform travellableBodies;

    public bool isRotating;

    public float scrollSensitivity;

    public float camZoomProgress;

    public float actualCamZoomProgress;
    public float camZoomSpeed;

    public AnimationCurve cameraRotationCurve; // it's literally just the x rotation on certain points in the progress

    // offset 0: right in front of the body (1.5f * radius * ((scaleX + scaleY + scaleZ) / 3f)) // or some other mult different than 1.5f
    // offset 1: kinda far above the body
    // 0.15f is the point of looking into the distance, camera moves into place bit after that still

    // camera anchor goes th the clicked body over like 0.6f seconds with animationCurve smoothing

    // zooming should also change FOV
    private void Update()
    {
        // left click - interact with stuff (no camera move, redundant, moving between bodies will only be possible for regular planets and certain dwarves)
        // right click - rotate (on Y only, smoothed)
        // scroll - zoom

        //print(Input.mouseScrollDelta.y);
        if (Input.mouseScrollDelta.y != 0)
        {
            print("scrollage");
            camZoomProgress = Mathf.Clamp(camZoomProgress + (float)Input.mouseScrollDelta.y * scrollSensitivity, 0f, 1f);
        }
        actualCamZoomProgress = Mathf.Lerp(actualCamZoomProgress, camZoomProgress, camZoomSpeed * Time.deltaTime);
        // Camera Zooming

        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                switch (i)
                {
                    case 0:
                        break; // interaction
                    case 1:
                        break; // rotate
                    case 2:
                        break; // slow down time one level?
                }
            } // click down
            else if (Input.GetMouseButtonUp(i))
            {
                switch (i)
                {
                    case 0:
                        break; // interaction
                    case 1:
                        break; // rotate
                    case 2:
                        break; // slow down time one level?
                }
            } // click release
        }
    }
}

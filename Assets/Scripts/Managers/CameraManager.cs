using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera ownCam;
    public Camera minigameCamera;

    public Transform camRotTarget;
    public Transform camAnchor;
    public Transform camTransform;

    public Transform[] travellableBodies;

    public bool isRotating;
    public float rotationSmoothSpeed;

    public float scrollSensitivity;

    public float camZoomProgress;

    public float actualCamZoomProgress;
    public float camZoomSpeed;

    public Transform currentAnchor;
    public Vector3 previousPos;
    //public Vector3 currentPos; // read only
    //public Vector3 targetPos; // read only
    public float moveProgress;
    public float moveTime;

    public Vector3 temp;
    public float temp2;
    public float temp3;

    public float mouseSensitivity;

    public AnimationCurve cameraRotationCurve; // it's literally just the x rotation on certain points in the progress
    public AnimationCurve cameraMoveCurve;
    public AnimationCurve cameraFOVCurve;
    public AnimationCurve moveStopperCurve;
    public AnimationCurve bodyDistanceCurve;

    public float maxCameraFOV;

    public bool testerage;

    public float fovTest;

    // offset 0: right in front of the body (1.5f * radius * ((scaleX + scaleY + scaleZ) / 3f)) // or some other mult different than 1.5f
    // offset 1: kinda far above the body
    // 0.15f is the point of looking into the distance, camera moves into place bit after that still

    // camera anchor goes th the clicked body over like 0.6f seconds with animationCurve smoothing

    // zooming should also change FOV]

    private float oldBaseBodyDistance;
    private float baseBodyDistance;
    private float outputBaseBodyDistance;

    [Space]
    public Transform newAnchor;

    public bool cameraControls;
    private void Awake()
    {
        string initialAnchor = "Sol";
        baseBodyDistance = (GameObject.Find(initialAnchor).transform.lossyScale.x + GameObject.Find(initialAnchor).transform.lossyScale.y + GameObject.Find(initialAnchor).transform.lossyScale.z) / 3f * GameObject.Find(initialAnchor).transform.GetComponent<CircleCollider2D>().radius;
        changeAnchor(GameObject.Find(initialAnchor).transform);
    }
    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                /*switch (i)
                {
                    case 0:
                        break; // interaction
                    case 1:
                        isRotating = true;
                        break; // rotate
                    case 2:
                        break; // slow down time one level?
                }//*/
                isRotating = cameraControls;
            } // click down
            else if (Input.GetMouseButtonUp(i))
            {
                /*switch (i)
                {
                    case 0:
                        break; // interaction
                    case 1:
                        isRotating = false;
                        break; // rotate
                    case 2:
                        break; // slow down time one level?
                }//*/
                isRotating = false;
            } // click release
        }

        moveProgress = Mathf.Min(moveProgress + Time.deltaTime, moveTime);
        camAnchor.position = Vector3.Lerp(previousPos, currentAnchor.position, cameraMoveCurve.Evaluate(moveProgress / moveTime));

        //currentPos = camAnchor.position;

        outputBaseBodyDistance = Mathf.Lerp(oldBaseBodyDistance, baseBodyDistance, bodyDistanceCurve.Evaluate(moveProgress / moveTime));

        temp3 = cameraMoveCurve.Evaluate(moveProgress / moveTime);
        temp2 = moveProgress / moveTime;
        temp = Vector3.Lerp(previousPos, currentAnchor.position, cameraMoveCurve.Evaluate(moveProgress / moveTime));

        // left click - interact with stuff (no camera move, redundant, moving between bodies will only be possible for regular planets and certain dwarves)
        // right click - rotate (on Y only, smoothed)
        // scroll - zoom

        // Camera Zooming
        if (cameraControls && Input.mouseScrollDelta.y != 0)
        {
            //print("scrollage");
            camZoomProgress = Mathf.Clamp(camZoomProgress - (float)Input.mouseScrollDelta.y * scrollSensitivity, 0f, 1f);
        }
        actualCamZoomProgress = Mathf.Lerp(actualCamZoomProgress, camZoomProgress, camZoomSpeed * Time.deltaTime);
        float zoomProgForMove = Mathf.Max(actualCamZoomProgress - moveStopperCurve.Evaluate(actualCamZoomProgress), 0f);

        camTransform.localPosition = Vector3.Lerp(Vector3.up * outputBaseBodyDistance * 2.5f, Vector3.forward * outputBaseBodyDistance * 3.5f, zoomProgForMove);
        camTransform.localRotation = Quaternion.Euler(cameraRotationCurve.Evaluate(actualCamZoomProgress) * 90f + 90f, 0, 0);
        ownCam.fieldOfView = cameraFOVCurve.Evaluate(actualCamZoomProgress) * maxCameraFOV;
        fovTest = cameraFOVCurve.Evaluate(actualCamZoomProgress) * maxCameraFOV;

        // Camera Rotation
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            camRotTarget.Rotate(camAnchor.forward * mouseX);
        }
        camAnchor.rotation = Quaternion.Lerp(camAnchor.rotation, camRotTarget.rotation, rotationSmoothSpeed * Time.deltaTime);
    }
    public void changeAnchor(Transform anchor)
    {
        if (anchor != currentAnchor)
        {
            previousPos = camAnchor.position;
            //targetPos = anchor.position;
            currentAnchor = anchor;
            moveProgress = 0;

            oldBaseBodyDistance = baseBodyDistance;
            baseBodyDistance = (anchor.lossyScale.x + anchor.lossyScale.y + anchor.lossyScale.z) / 3f * anchor.GetComponent<CircleCollider2D>().radius;
            outputBaseBodyDistance = oldBaseBodyDistance;

            /*GameObject temptst = new GameObject("");
            temptst.transform.position = anchor.position + anchor.forward * baseBodyDistance;//*/
            // reset body UI (or something idk)
        }
    }
    public void toggleCameraControls(float input)
    {
        cameraControls = (input == -1f);
    }
}
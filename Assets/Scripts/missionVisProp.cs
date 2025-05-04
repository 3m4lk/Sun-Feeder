using UnityEngine;

public class missionVisProp : MonoBehaviour
{
    public MissionManager msManager;

    public Transform ownMain;
    public Transform targetLooker;
    public Transform target;
    public Transform defaultRotation;

    public AnimationCurve lookCurve;
    public float lookTime;
    private float lookProgress;
    private void FixedUpdate()
    {
        targetLooker.LookAt(target);
        lookProgress = Mathf.Min(lookProgress + Time.fixedDeltaTime * msManager.mManager.gameManager.gameSpeed, lookTime);

        ownMain.rotation = Quaternion.Lerp(defaultRotation.rotation, targetLooker.rotation, lookCurve.Evaluate(lookProgress / lookTime));


    }
}

using UnityEngine;

public class missionPack : MonoBehaviour
{
    public Transform target;
    public Transform shipTransform;
    public Transform[] missionShipTravel;

    public GameObject missionObjectToDrop;

    public GameObject ownBomb3;
    public GameObject ownBomb500;
    private void FixedUpdate()
    {
        shipTransform.LookAt(target);
    }
}

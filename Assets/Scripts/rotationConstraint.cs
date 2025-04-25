using UnityEngine;

public class rotationConstraint : MonoBehaviour
{
    public Transform reference;
    public float maxAngleDifference;
    public bool doMove;

    public float smoothSpeed;
    private void Update()
    {
        /*if (Quaternion.Angle(transform.rotation, reference.rotation) > maxAngleDifference)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, reference.rotation, smoothSpeed * Time.deltaTime);
        }//*/

        transform.rotation = Quaternion.Lerp(transform.rotation, reference.rotation, smoothSpeed * Time.deltaTime);

        if (doMove)
        {
            transform.position = reference.position;
        }
    }
}

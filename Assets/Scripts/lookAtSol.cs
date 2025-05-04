using UnityEngine;

public class lookAtSol : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.LookAt(Vector3.zero);
    }
}

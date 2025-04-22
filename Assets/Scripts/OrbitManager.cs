using UnityEngine;

[System.Serializable]
public class cBody
{
    [Tooltip("Display name")]
    public string name;
    [Tooltip("True name of a celestial body (displayed under the Display Name)")]
    public string trueName;
    public Transform bodyTransform;

    [Space]
    [Tooltip("in Earthen years / 100 (for accuracy)")]
    public float orbitSpeed;
    [Tooltip("in Earth masses")]
    public float mass;
    [Tooltip("in Astronomical Units")]
    public float orbitDistance;
    public Transform orbitBody;

    [Space]
    public float orbitProgress;
}
public class OrbitManager : MonoBehaviour
{
    public MainManager iManager;

    public cBody[] bodies;
    private void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = Mathf.Repeat(bodies[i].orbitProgress + (1f / bodies[i].orbitSpeed) * Time.fixedDeltaTime * iManager.gameManager.gameSpeed, 100f);

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * iManager.gameManager.AstronomicalUnit;
        }
    }
    public void WipeBodies(int inputLength)
    {
        bodies = default;
        bodies = new cBody[inputLength];
        print(inputLength);
    }
}

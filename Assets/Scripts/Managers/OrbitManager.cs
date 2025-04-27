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
[ExecuteInEditMode]
public class OrbitManager : MonoBehaviour
{
    public MainManager iManager;

    public cBody[] bodies;

    public bool recalcLive;

    [Range(0f, 1f)]
    public float asteroidBeltRemaining;
    private void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = Mathf.Repeat(bodies[i].orbitProgress + (1f / bodies[i].orbitSpeed) * Time.fixedDeltaTime * iManager.gameManager.gameSpeed * 100f, 100f);

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f)); // not gonna simplify these by purging 100s and changing Mathf.Repeat to 1f - for precision and visual clarity reasons

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * iManager.gameManager.AstronomicalUnit;
        }
    }
    [ExecuteInEditMode]
    private void Update()
    {
        if (Application.isEditor && recalcLive)
        {
            recalcPositions();
        }
    }
    public void WipeBodies(int inputLength)
    {
        bodies = default;
        bodies = new cBody[inputLength];
        print(inputLength);
    }
    public void recalcPositions()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * iManager.gameManager.AstronomicalUnit;
        }
    }
    public void randomPositions()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = Random.Range(0f, 100f);

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * iManager.gameManager.AstronomicalUnit;
        }
    }
    public void resetPositions()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = 0;

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * iManager.gameManager.AstronomicalUnit;
        }
    }
}

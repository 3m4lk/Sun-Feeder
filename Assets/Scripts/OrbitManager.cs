using UnityEngine;

[System.Serializable]
public class cBody
{
    [Tooltip("Display name")]
    public string name;
    [Tooltip("True name of a celestial body (displayed under the Display Name)")]
    public string trueName;

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
    public static OrbitManager instance { get; private set; }

    private GameManager gManager => GameManager.instance;

    public cBody[] bodies;
    private void Awake()
    {
        instance = this;
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = Mathf.Repeat(bodies[i].orbitProgress + bodies[i].orbitSpeed * Time.fixedDeltaTime * gManager.gameSpeed, 100f);
        }
    }
    public void WipeBodies(int inputLength)
    {
        bodies = default;
        bodies = new cBody[inputLength];
    }
}

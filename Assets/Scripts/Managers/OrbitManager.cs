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
    public float healthAdd;

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

    [Space]
    public int clusterIndex;

    public float orbitGrowth;

    public bool isAlive;
}
[ExecuteInEditMode]
public class OrbitManager : MonoBehaviour
{
    public MainManager mManager;

    public cBody[] bodies;

    public bool recalcLive;

    public Transform Sol;

    [Range(0f, 1f)]
    public float asteroidBeltRemaining;
    private void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = Mathf.Repeat(bodies[i].orbitProgress + (1f / bodies[i].orbitSpeed) * Time.fixedDeltaTime * mManager.gameManager.gameSpeed * 100f, 100f);

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f)); // not gonna simplify these by purging 100s and changing Mathf.Repeat to 1f - for precision and visual clarity reasons

            alterOrbitDistance(i, bodies[i].orbitGrowth * Time.deltaTime * mManager.gameManager.gameSpeed);

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * mManager.gameManager.AstronomicalUnit;
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

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * mManager.gameManager.AstronomicalUnit;
        }
    }
    public void randomPositions()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = Random.Range(0f, 100f);

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * mManager.gameManager.AstronomicalUnit;
        }
    }
    public void resetPositions()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].orbitProgress = 0;

            float sinOut = Mathf.Sin((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(bodies[i].orbitProgress / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            bodies[i].bodyTransform.position = bodies[i].orbitBody.position + new Vector3(sinOut, cosOut, 0) * bodies[i].orbitDistance * mManager.gameManager.AstronomicalUnit;
        }
    }
    public int getBodyIndex(Transform input)
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i].bodyTransform == input) return i;
        }
        return -1;
    }
    public void alterOrbitDistance(int index, float difference)
    {
        if (!bodies[index].isAlive) return;
        Vector3 bScale = bodies[index].orbitBody.localScale;
        //float killDistance = ((bScale.x + bScale.y + bScale.z) / 3f) * bodies[index].bodyTransform.GetComponent<CircleCollider2D>().radius;

        float killDistance = 0.32f;

        if (bodies[index].orbitBody != Sol)
        {
            killDistance = 0.008f;
        }

        bodies[index].orbitDistance = Mathf.Max(bodies[index].orbitDistance + difference, killDistance);
        if (bodies[index].orbitDistance == killDistance)
        {
            bodies[index].isAlive = false;
            print("DESTROY THE BODY: " + bodies[index].name); // create an explosion prefab in the body's spot under its orbiting body's hierarchy
            bodies[index].bodyTransform.gameObject.SetActive(false);
            bodies[index].isAlive = false;

            if (bodies[index].orbitBody != Sol)
            {
                int theIndex = getBodyIndex(bodies[index].orbitBody);
                alterOrbitDistance(theIndex, -bodies[theIndex].orbitDistance * 0.15f);
                bodies[theIndex].healthAdd += bodies[index].healthAdd * 1.4f;
            }
            else
            {
                mManager.solManager.addHealth(bodies[index].healthAdd);
            }

            if (index == -1) return; // don't do cluster stuff for other minor moons
            mManager.celManager.clusterAmountUpdate(index, -1);
        }
    }
    public void doMission(int index, float growthAmount, string visName)
    {
        if (!visName.Contains("hiredMiners"))
        {
            bodies[index].orbitGrowth += growthAmount;
            GameObject newVis = Instantiate(Resources.Load<GameObject>(visName));
            newVis.transform.parent = bodies[index].bodyTransform;
            newVis.transform.localPosition = Vector3.zero;
            newVis.transform.localRotation = Quaternion.identity;
            newVis.transform.localScale = Vector3.one;
            newVis.SetActive(true);
        }
    }
}

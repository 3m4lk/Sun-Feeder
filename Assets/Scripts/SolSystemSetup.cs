using UnityEngine;

[ExecuteInEditMode]
public class SolSystemSetup : MonoBehaviour
{
    private OrbitManager orbManager => OrbitManager.instance;

    public string dataFile;
    public Transform refBody;

    public float test;
    public float output;

    public Transform refTest;
    public float testDist;

    public bool doSineTest;

    [Space]
    public float defCBScale;
    [Tooltip("distance from Sun to Earth; here, a multiplier")]
    public float AstronomicalUnit;
    private void Update()
    {
        if (doSineTest)
        {
            float input = test * (Mathf.PI * 2f) / 360f;

            float sinOut = Mathf.Sin(Mathf.Repeat(input, Mathf.PI * 2f));
            float cosOut = Mathf.Cos(Mathf.Repeat(input, Mathf.PI * 2f));
            //float cosOut = 1f - sinOut; // as much as i'd love for this to work, sadly it doesn't, lmfaoooooooo

            output = sinOut;

            refTest.position = refBody.position + new Vector3(sinOut, cosOut, 0) * testDist;
        }
    }
    public void setup()
    {
        wipe();

        // 0 - display name
        // 1 - true name (can make it "" to appear blank)
        // 2 - distance
        // 3 - mass

        string[] dataArray = Resources.Load<TextAsset>("cbData/" + dataFile).text.Replace(((char)13).ToString(), "").Split("\n");
        //print(dataArray.Length);

        orbManager.WipeBodies(dataArray.Length);// i genuinely have no clue why does this not work, will need to fix this ASAP

        for (int i = 0; i < dataArray.Length; i++)
        {
            string[] currData = dataArray[i].Split(", ");

            GameObject currentCB = Instantiate(Resources.Load<GameObject>("CelestialBody"));
            currentCB.transform.localScale = Vector3.one * defCBScale;

            // Name
            currentCB.name = currData[0];

            // Distance
            float randomPeriod = Random.Range(0f, 100f);
            float randomAngle = ((randomPeriod / 100f) * 360f) * (Mathf.PI * 2f) / 360f; // yes this is all necessary, trust :prayingEmoji:

            float sinOut = Mathf.Sin(Mathf.Repeat(randomAngle, Mathf.PI * 2f));
            float cosOut = Mathf.Cos(Mathf.Repeat(randomAngle, Mathf.PI * 2f));

            Vector3 newPos = new Vector3(sinOut, cosOut, 0) * float.Parse(currData[2]) * AstronomicalUnit;

            // Plugging it to the Orbit Manager
            orbManager.bodies[i] = new cBody();

            orbManager.bodies[i].name = currData[0];
            orbManager.bodies[i].trueName = currData[1];
            orbManager.bodies[i].orbitDistance = float.Parse(currData[2]);
            orbManager.bodies[i].mass = float.Parse(currData[3]);

            orbManager.bodies[i].orbitProgress = randomPeriod; // to align it with randomized celestial body placement

            orbManager.bodies[i].orbitBody = refBody;

            // Transforms, setting up & placing celestial bodies accordingly
            currentCB.transform.position = refBody.position + newPos;
            currentCB.transform.parent = refBody;

            currentCB.name = currData[0];
        }
        print("Finished with " + dataArray.Length + " bodies added!");
    }
    void wipe()
    {
        for (int i = refBody.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(refBody.GetChild(i).gameObject);
        }
        if (refBody.childCount != 0)
        {
            DestroyImmediate(refBody.GetChild(0).gameObject);
        }
    }
}

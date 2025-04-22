using UnityEngine;

[ExecuteInEditMode]
public class SolSystemSetup : MonoBehaviour
{
    public MainManager iManager;

    public string dataFile;
    public Transform refBody;

    public float test;
    public float output;

    public Transform refTest;
    public float testDist;

    public bool doSineTest;

    [Space]
    public float defCBScale;
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
        // 4 - orbit time

        string[] dataArray = Resources.Load<TextAsset>("cbData/" + dataFile).text.Replace(((char)13).ToString(), "").Split("\n");

        //print(iManager);

        iManager.orbManager.WipeBodies(dataArray.Length);// i genuinely have no clue why does this not work, will need to fix this ASAP

        for (int i = 0; i < dataArray.Length; i++)
        {
            string[] currData = dataArray[i].Split(", ");

            GameObject currentCB = Instantiate(Resources.Load<GameObject>("CelestialBody"));
            currentCB.transform.localScale = Vector3.one * defCBScale;

            // Name
            currentCB.name = currData[0];

            // Distance
            float randomPeriod = Random.Range(0f, 100f);
            // yes this is all necessary, trust :prayingEmoji:

            float sinOut = Mathf.Sin((float)Mathf.Repeat(randomPeriod / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
            float cosOut = Mathf.Cos((float)Mathf.Repeat(randomPeriod / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

            Vector3 newPos = new Vector3(sinOut, cosOut, 0) * float.Parse(currData[2]) * iManager.gameManager.AstronomicalUnit;

            // Plugging it to the Orbit Manager
            iManager.orbManager.bodies[i] = new cBody();

            iManager.orbManager.bodies[i].name = currData[0];
            iManager.orbManager.bodies[i].trueName = currData[1];
            iManager.orbManager.bodies[i].orbitDistance = float.Parse(currData[2]);
            iManager.orbManager.bodies[i].mass = float.Parse(currData[3]);
            iManager.orbManager.bodies[i].orbitSpeed = float.Parse(currData[4]);

            iManager.orbManager.bodies[i].orbitProgress = randomPeriod; // to align it with randomized celestial body placement

            iManager.orbManager.bodies[i].orbitBody = refBody;

            iManager.orbManager.bodies[i].bodyTransform = currentCB.transform;

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

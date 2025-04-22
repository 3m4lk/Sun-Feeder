using UnityEngine;

[System.Serializable]
public class satSetups
{
    public string dataFile;
    public Transform orbitBody;

    [Tooltip("is the orbit body going to be generated freshly too?")]
    public bool useNewOrbitBody;
    [Tooltip("True Name of a body these bodies will need to orbit around")]
    public string newOrbitBodyName;

    [Tooltip("Preset scale of all celestial bodies to be generated (in Earth sizes, altered for cosmetic purposes)")]
    public float presetScale = 1f;

    public bool randomizeScale;
    public float randomThreshold;
}
[ExecuteInEditMode]
public class SolSystemSetup : MonoBehaviour
{
    public MainManager iManager;

    public satSetups[] celestialBodiesSetup;

    public Transform Sol;

    //public string dataFile;

    public float test;
    public float output;

    public Transform refTest;
    public float testDist;

    public bool doSineTest;

    public bool noRandom;

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

            refTest.position = Sol.position + new Vector3(sinOut, cosOut, 0) * testDist;
        }
    }
    public void setup()
    {
        wipe();

        int celBodiesLength = 0;

        for (int i = 0; i < celestialBodiesSetup.Length; i++)
        {
            celBodiesLength += Resources.Load<TextAsset>("cbData/" + celestialBodiesSetup[i].dataFile).text.ToString().Split("\n").Length;
        }

        // 0 - display name
        // 1 - true name (can make it "" to appear blank)
        // 2 - distance
        // 3 - mass
        // 4 - orbit time

        iManager.orbManager.WipeBodies(celBodiesLength);// i genuinely have no clue why does this not work, will need to fix this ASAP // nvm fixed it :3c

        int totalIndex = 0;

        for (int i = 0; i < celestialBodiesSetup.Length; i++)
        {
            Transform refBody = default;

            if (celestialBodiesSetup[i].useNewOrbitBody)
            {
                refBody = GameObject.Find(celestialBodiesSetup[i].newOrbitBodyName).transform;
            } // find a new body; inefficient, but this is literally just gonna be internal code not included with the final build :3c (can't say the same about the SC... oops!)
            else
            {
                refBody = celestialBodiesSetup[i].orbitBody;
            } // use selected transform

            string[] dataArray = Resources.Load<TextAsset>("cbData/" + celestialBodiesSetup[i].dataFile).text.Replace(((char)13).ToString(), "").Split("\n");

            for (int ia = 0; ia < dataArray.Length; ia++)
            {
                string[] currData = dataArray[ia].Split(", ");

                //print("creating " + currData[0] + "...");
                //print(currData[4]);

                GameObject currentCB = Instantiate(Resources.Load<GameObject>("CelestialBody"));
                currentCB.transform.localScale = Vector3.one * defCBScale;

                // Name
                currentCB.name = currData[0];
                //print("name");

                // Distance
                float randomPeriod = default;
                if (!noRandom)
                {
                    randomPeriod = Random.Range(0f, 100f);
                }
                // yes this is all necessary, trust :prayingEmoji:

                float sinOut = Mathf.Sin((float)Mathf.Repeat(randomPeriod / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));
                float cosOut = Mathf.Cos((float)Mathf.Repeat(randomPeriod / 100f * (Mathf.PI * 2f), Mathf.PI * 2f));

                Vector3 newPos = new Vector3(sinOut, cosOut, 0) * float.Parse(currData[2]) * iManager.gameManager.AstronomicalUnit;

                // Plugging it to the Orbit Manager
                iManager.orbManager.bodies[totalIndex] = new cBody();

                //print("bodyName");
                iManager.orbManager.bodies[totalIndex].name = currData[0];
                //print("trueName");
                iManager.orbManager.bodies[totalIndex].trueName = currData[1];
                //print("orbitDistance");
                iManager.orbManager.bodies[totalIndex].orbitDistance = float.Parse(currData[2]);
                //print("mass");
                iManager.orbManager.bodies[totalIndex].mass = float.Parse(currData[3]);
                //print("orbitSpeed");
                iManager.orbManager.bodies[totalIndex].orbitSpeed = float.Parse(currData[4]);

                //print("orbitProgress");
                iManager.orbManager.bodies[totalIndex].orbitProgress = randomPeriod; // to align it with randomized celestial body placement

                //print("orbitBody");
                iManager.orbManager.bodies[totalIndex].orbitBody = refBody;

                //print("bodyTransform");
                iManager.orbManager.bodies[totalIndex].bodyTransform = currentCB.transform;

                // Transforms, setting up & placing celestial bodies accordingly
                //print("position");
                currentCB.transform.position = refBody.position + newPos;
                //print("scale (preset)");
                currentCB.transform.localScale = Vector3.one * celestialBodiesSetup[i].presetScale;

                //print("randomizedScale");
                if (celestialBodiesSetup[i].randomizeScale)
                {
                    currentCB.transform.localScale += Vector3.one * Random.Range(-celestialBodiesSetup[i].randomThreshold, celestialBodiesSetup[i].randomThreshold);
                }
                //print("parent");
                currentCB.transform.parent = Sol;

                //print("currently: " + totalIndex);
                totalIndex++;
            }
        }
        print("Finished with " + celBodiesLength + " bodies added!");
    }
    void wipe()
    {
        for (int i = Sol.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(Sol.GetChild(i).gameObject);
        }
        if (Sol.childCount != 0)
        {
            DestroyImmediate(Sol.GetChild(0).gameObject);
        }
    }
}

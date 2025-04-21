using UnityEngine;

[ExecuteInEditMode]
public class SolSystemSetup : MonoBehaviour
{
    public string dataFile;
    public Transform refBody;

    public float test;
    public float output;

    public Transform refTest;
    public float testDist;

    public bool doSineTest;

    [Space]
    public float defCBScale;
    public float distMult;
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

        // name, distance, true name (optional)

        string[] dataArray = Resources.Load<TextAsset>("cbData/" + dataFile).text.Replace(((char)13).ToString(), "").Split("\n");

        for (int i = 0; i < dataArray.Length; i++)
        {
            string[] currData = dataArray[i].Split(", ");

            GameObject currentCB = Instantiate(Resources.Load<GameObject>("CelestialBody"));
            currentCB.transform.localScale = Vector3.one * defCBScale;

            // Name
            currentCB.name = currData[0];

            // Distance
            float randomAngle = Random.Range(0f, 360f) * (Mathf.PI * 2f) / 360f;

            float sinOut = Mathf.Sin(Mathf.Repeat(randomAngle, Mathf.PI * 2f));
            float cosOut = Mathf.Cos(Mathf.Repeat(randomAngle, Mathf.PI * 2f));

            Vector3 newPos = new Vector3(sinOut, cosOut, 0) * float.Parse(currData[1]) * distMult;

            currentCB.transform.position = refBody.position + newPos;
            currentCB.transform.parent = refBody;

            // True Name (if appliccable)
            if (currData.Length > 2)
            {
                currentCB.name += " \"" + currData[2] + "\"";
            }
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

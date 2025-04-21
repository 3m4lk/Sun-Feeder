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
    private void Update()
    {
        float input = test * (Mathf.PI * 2f) / 360f;

        float sinOut = Mathf.Sin(Mathf.Repeat(input, Mathf.PI * 2f));
        float cosOut = Mathf.Cos(Mathf.Repeat(input, Mathf.PI * 2f));
        //float cosOut = 1f - sinOut; // as much as i'd love for this to work, sadly it doesn't, lmfaoooooooo

        output = sinOut;

        refTest.position = refBody.position + new Vector3(sinOut, 0, cosOut) * testDist;
    }
}

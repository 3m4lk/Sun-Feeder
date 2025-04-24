using UnityEngine;

[ExecuteInEditMode]
public class quickVisSetupTool : MonoBehaviour
{
    public Transform thingsParent;
    public GameObject visPrefab;

    public float maxDistance;
    public int lightGroupAmount;
    public GameObject farthestBody;
    public GameObject closestBody;

    public float checkDistance;
    [ExecuteInEditMode]
    public void setupVisuals()
    {
        for (int i = 0; i < thingsParent.childCount; i++)
        {
            if (thingsParent.GetChild(i).childCount != 0)
            {
                DestroyImmediate(thingsParent.GetChild(i).GetChild(0).gameObject);
            }

            Mesh ownMesh = thingsParent.GetChild(i).GetComponent<MeshFilter>().sharedMesh;
            Material ownMat = thingsParent.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial;

            GameObject newVP = Instantiate(visPrefab, thingsParent.GetChild(i).position, thingsParent.GetChild(i).rotation);
            newVP.name = thingsParent.GetChild(i).name + "Vis";

            newVP.GetComponent<MeshFilter>().sharedMesh = ownMesh;
            newVP.GetComponent<MeshRenderer>().sharedMaterial = ownMat;

            newVP.transform.parent = thingsParent.GetChild(i);
            newVP.transform.localScale = Vector3.one;
            newVP.transform.localPosition = Vector3.zero;

            float bodyDistance = Vector3.Distance(thingsParent.position, thingsParent.GetChild(i).position);

            newVP.layer = LayerMask.NameToLayer("lightGroup0");

            for (int a = 0; a < lightGroupAmount; a++)
            {
                if (bodyDistance <= (maxDistance / lightGroupAmount) + (maxDistance / lightGroupAmount) * a)
                {
                    newVP.layer = LayerMask.NameToLayer("lightGroup" + a);
                    newVP.GetComponent<MeshRenderer>().renderingLayerMask = 1u << RenderingLayerMask.NameToRenderingLayer("lightGroup" + a);
                    break;
                }
            }

            newVP.SetActive(true);

            DestroyImmediate(thingsParent.GetChild(i).GetComponent<MeshFilter>());
            DestroyImmediate(thingsParent.GetChild(i).GetComponent<MeshRenderer>());
        }
    }
    public void findFarthest()
    {
        float farthestDist = -1f;
        string farthestName = "None";
        for (int i = 0; i < thingsParent.childCount; i++)
        {
            if (Vector3.Distance(thingsParent.position, thingsParent.GetChild(i).position) > farthestDist)
            {
                farthestName = thingsParent.GetChild(i).name;
                farthestDist = Vector3.Distance(thingsParent.position, thingsParent.GetChild(i).position);
                farthestBody = thingsParent.GetChild(i).gameObject;
            }
        }
        print("Farthest Body: " + farthestName + ", at distance of " + farthestDist + "!");
    }
    public void findClosest()
    {
        float closestDist = float.MaxValue;
        string closestName = "None";
        for (int i = 0; i < thingsParent.childCount; i++)
        {
            if (Vector3.Distance(thingsParent.position, thingsParent.GetChild(i).position) < closestDist)
            {
                closestName = thingsParent.GetChild(i).name;
                closestDist = Vector3.Distance(thingsParent.position, thingsParent.GetChild(i).position);
                closestBody = thingsParent.GetChild(i).gameObject;
            }
        }
        print("Farthest Body: " + closestName + ", at distance of " + closestDist + "!");
    }
    public void amountOfAllUnderDistance()
    {
        int count = 0;
        for (int i = 0; i < thingsParent.childCount; i++)
        {
            if (Vector3.Distance(thingsParent.position, thingsParent.GetChild(i).position) > checkDistance)
            {
                count++;
            }
        }
        print(count + " celestial bodies are farther than " + checkDistance + " units from the Sun!");
    }
}
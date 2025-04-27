using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    public MainManager mManager;

    public MinigameManager mgmanager;
    public float bumpMult = 1f;

    public SkinnedMeshRenderer meshRenderer;
    [Header("0 - Light;\n1 - Spotlight (visible light beam).")]
    public Material[] materialsOn;
    public Material[] materialsOff;

    public GameObject[] beamVisuals;
    public int beamLevel;

    public Transform miningRig;
    private void Awake()
    {
        switchLights(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("asiuldofhj");
        if (collision.gameObject.tag == "MinigameHitter")
        {
            mgmanager.hit(collision.rigidbody.mass / 10f);
            switchLights(false);
        }
    }
    public void switchLights(bool mode)
    {
        //print("LIGHTS SWITCHED TO " + mode);

        if (mode)
        {
            meshRenderer.materials = materialsOn;
            for (int i = 0; i < beamVisuals.Length; i++)
            {
                beamVisuals[i].SetActive(false);
            }
            beamVisuals[beamLevel].SetActive(true);
            return;
        } // on

        for (int i = 0; i < beamVisuals.Length; i++)
        {
            beamVisuals[i].SetActive(false);
        }
        meshRenderer.materials = materialsOff; // off
    }
}
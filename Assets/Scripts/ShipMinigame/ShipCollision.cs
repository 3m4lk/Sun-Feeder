using UnityEngine;

public class ShipCollision : MonoBehaviour
{
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
        print("asiuldofhj");
        if (collision.gameObject.tag == "MinigameHitter")
        {
            mgmanager.hit(collision.rigidbody.mass / 10f);
            switchLights(false);

            /*Vector3 astVelo = collision.rigidbody.linearVelocity;

            Vector3 randUpOrDown = new Vector2(0, Mathf.Sign(Random.Range(-4f, 4f)));

            GetComponent<Rigidbody2D>().AddForce(((transform.position - collision.transform.position).normalized + randUpOrDown).normalized * astVelo.magnitude * 0.65f * collision.rigidbody.mass * bumpMult);
            Vector3 asteroBump = (astVelo.normalized - ((transform.position - collision.transform.position).normalized) - randUpOrDown).normalized * collision.gameObject.GetComponent<Asteroid>().asteroidSpeed;
            collision.rigidbody.linearVelocity = asteroBump * 0.85f;//*/
        }
    }
    public void switchLights(bool mode)
    {
        print("LIGHTS SWITCHED TO " + mode);

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
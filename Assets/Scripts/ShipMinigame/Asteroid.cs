using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int baseCashAmount;

    public float asteroidSpeed = 10f;
    private Vector3 randRotation;
    public float randRotationMaxSpeed;
    //public Vector2 movementVector;
    //public Vector2 ogMovementVector;
    public Rigidbody2D ownRb;

    public Vector2 spawnDirection;

    public float attractionMult;
    //public float dot;
    //public Transform testPlat;

    private float randomScale;

    private ShipCollision ship;
    private uiMoverThingForOneSingularTest uiThing;

    public float lifetime;
    public Transform destroyParticles;
    private void Awake()
    {
        Destroy(gameObject, lifetime);

        uiThing = GameObject.Find("WorldspaceToScreenThing").GetComponent<uiMoverThingForOneSingularTest>();
        ship = GameObject.Find("GravShipGO").GetComponent<ShipCollision>();
        randomScale = Random.Range(0.6f, 2f);
        transform.localScale *= randomScale;

        ownRb.linearVelocity = spawnDirection * (asteroidSpeed * Random.Range(0.95f, 1.1f));

        randRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(-randRotationMaxSpeed, randRotationMaxSpeed);

        // commit spawnage
    }
    private void FixedUpdate()
    {
        transform.Rotate(randRotation * Time.fixedDeltaTime);
        //dot = Vector2.Dot(ownRb.linearVelocity.normalized, (testPlat.position - transform.position).normalized);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //print(collision.tag);
        if (collision.tag == "MinigameAttractor" || collision.tag == "MinigameHitter")
        {
            float speedAdd = 1f;
            float mrAttractionSpeed = 1f;
            if (Vector2.Dot(ownRb.linearVelocity.normalized, (ship.miningRig.position - transform.position).normalized) > 0.99f)
            {
                speedAdd = 1.025f;
            }

            if (collision.tag == "MinigameHitter")
            {
                mrAttractionSpeed = 2f;
                speedAdd = 1.0015f;
            } // mining rig

            ownRb.linearVelocity = Vector2.Lerp(ownRb.linearVelocity.normalized, (ship.miningRig.position - transform.position).normalized, ownRb.mass * attractionMult * mrAttractionSpeed * Time.fixedDeltaTime).normalized * Mathf.Max(ownRb.linearVelocity.magnitude * speedAdd, asteroidSpeed * 1.4f);
            //collision.GetComponentInParent<ShipCollision>().miningRig
            // lerp movement vector towards base
        } // if is in the attraction beam or around the mining rig
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GSMBase"))
        {
            int cashReward = Mathf.CeilToInt((float)baseCashAmount * randomScale * (ownRb.mass * 0.1f) * ship.mgmanager.miningRigRewardMult);
            //print("ADD CASH AND PLAY FANFARE: " + cashReward + ".");
            //ship.mManager.gameManager.canvasTransform

            RectTransform cashText = Instantiate(Resources.Load<GameObject>("cashText")).GetComponent<RectTransform>();
            cashText.SetParent(ship.mManager.minigameManager.maskTransform, false);
            cashText.localPosition = uiThing.getScreenPoint(ship.mManager.camManager.minigameCamera, transform.position); // = ship.mManager.camManager.minigameCamera.ScreenToWorldPoint(transform.position);
            cashText.GetComponent<MiningTextFloatUp>().amount = cashReward;
            cashText.gameObject.SetActive(true);

            ship.mManager.gameManager.addCash(cashReward);

            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        destroyParticles.parent = null;
        Destroy(destroyParticles.gameObject, 4f);
        destroyParticles.gameObject.SetActive(true);
    }
}

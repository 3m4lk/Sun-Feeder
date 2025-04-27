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

    public float attractionMult;
    public float dot;
    public Transform testPlat;

    private float randomScale;

    private ShipCollision ship;
    private void Awake()
    {
        ship = GameObject.Find("GravShipGO").GetComponent<ShipCollision>();
        randomScale = Random.Range(0.6f, 2f);
        transform.localScale *= randomScale;
        //ogMovementVector = Vector2.left;
        Vector2 movementVector = Vector2.left; // used to be og

        ownRb.linearVelocity = movementVector * asteroidSpeed;
        randRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(-randRotationMaxSpeed, randRotationMaxSpeed);

    }
    private void FixedUpdate()
    {
        transform.Rotate(randRotation * Time.fixedDeltaTime);
        dot = Vector2.Dot(ownRb.linearVelocity.normalized, (testPlat.position - transform.position).normalized);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //print(collision.tag);
        if (collision.tag == "MinigameAttractor")
        {
            float speedAdd = 1f;
            if (Vector2.Dot(ownRb.linearVelocity.normalized, (ship.miningRig.position - transform.position).normalized) > 0.999f)
            {
                speedAdd = 1.025f;
            }
            ownRb.linearVelocity = Vector2.Lerp(ownRb.linearVelocity.normalized, (ship.miningRig.position - transform.position).normalized, ownRb.mass * attractionMult * Time.fixedDeltaTime).normalized * Mathf.Max(ownRb.linearVelocity.magnitude * speedAdd, asteroidSpeed * 1.4f);
            //collision.GetComponentInParent<ShipCollision>().miningRig
            // lerp movement vector towards base
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GSMBase"))
        {
            int cashReward = Mathf.CeilToInt((float)baseCashAmount * randomScale * (ownRb.mass * 0.1f) * ship.mgmanager.miningRigRewardMult);
            print("ADD CASH AND PLAY FANFARE: " + cashReward + ".");
            Destroy(gameObject);
        }
    }
}

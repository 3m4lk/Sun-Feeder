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

    [Space]
    [Header("GravShip Visuals")]
    private bool isVisualOn;
    public GameObject ownOutline;

    [Space]
    public AnimationCurve appearCurve;
    public float appearTime;
    private float appearProgress;

    [Space]
    [Tooltip("0 -> 1 based on how dot product behaves")]
    public Gradient lineGradDire;
    [Tooltip("0 -> 1 based on how dot product behaves")]
    public Gradient lineGradDesi;
    [Tooltip("0 - ship -> asteroid;\n1 - current direction;\n2 - direction to rig")]
    public LineRenderer[] ownLines;
    public float vectorLineLength;
    private void Awake()
    {
        Destroy(gameObject, lifetime);

        uiThing = GameObject.Find("worldspaceToScreenThing").GetComponent<uiMoverThingForOneSingularTest>();
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

        if (isVisualOn)
        {
            appearProgress = Mathf.Min(appearProgress + Time.deltaTime, appearTime);
        }
        else
        {
            appearProgress = Mathf.Max(appearProgress - Time.deltaTime, 0);
        }

        Vector3 ownVelo = ownRb.linearVelocity.normalized;

        Vector3[] positions0 = new Vector3[2] { ship.transform.position, transform.position }; // ship -> asteroid
        Vector3[] positions1 = new Vector3[2] { transform.position, transform.position + ownVelo.normalized * vectorLineLength }; // current direction
        Vector3[] positions2 = new Vector3[2] { transform.position, transform.position + (ship.miningRig.position - transform.position).normalized * vectorLineLength }; // direction to rig

        Vector3[] positionsForDot = new Vector3[2] { ownVelo, ship.miningRig.position - transform.position };

        float lineDot = Vector3.Dot(positionsForDot[0].normalized, positionsForDot[1].normalized);

        if (!isVisualOn)
        {
            positions0 = new Vector3[2] { transform.position, transform.position }; // reset cause i said so
        }

        ownLines[0].SetPositions(positions0);
        ownLines[1].SetPositions(positions1);
        ownLines[2].SetPositions(positions2);

        Color32 colorDirection = lineGradDire.Evaluate(lineDot);
        Color32 colorDesired = lineGradDesi.Evaluate(lineDot);

        ownLines[1].startColor = colorDirection;
        ownLines[1].endColor = colorDirection;

        ownLines[2].startColor = colorDesired;
        ownLines[2].endColor = colorDesired;

        float scaleLength = appearCurve.Evaluate(appearProgress / appearTime);
        ownOutline.transform.localScale = Vector3.one * scaleLength;
        for (int i = 0; i < ownLines.Length; i++)
        {
            ownLines[i].transform.Rotate(-randRotation * Time.fixedDeltaTime);
            ownLines[i].transform.localScale = Vector3.one * scaleLength;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //print(collision.tag);
        if (collision.tag == "MinigameAttractor" || collision.tag == "MinigameHitter")
        {
            bool isShipColl = false;

            if (collision.name == "beamTrigger")
            {
                //print("GRAVSHIPBEAM");
                isShipColl = true;
            }

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.name);
        if (collision.name == "beamTrigger")
        {
            //print("GRAVSHIPBEAM");
            ownOutline.SetActive(true);
            isVisualOn = true;

            Color32 beamLvlColor = ship.mgmanager.shipBeamLevelColor.Evaluate(ship.beamLevel / 3f);
            ownLines[0].material.SetColor("_Color", beamLvlColor);// = beamLvlColor;
            //ownLines[0].startColor = beamLvlColor;
            //ownLines[0].endColor = beamLvlColor;
            ownOutline.GetComponent<MeshRenderer>().material.color = beamLvlColor;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "beamTrigger")
        {
            //print("GRAVSHIPBEAM (exit)");
            ownOutline.SetActive(false);
            isVisualOn = false;
        }
    }
    private void OnDestroy()
    {
        destroyParticles.parent = null;
        Destroy(destroyParticles.gameObject, 4f);
        destroyParticles.gameObject.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class gonmts
{
    public Material[] materials;
    public GameObject[] gameObjects;
}
[System.Serializable]
public class abLvl
{
    public GameObject common;
    public GameObject uncommon;
    public GameObject rare;

    public GameObject[] leftovers;
    public int leftoversChance;
}
public class MinigameManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    public bool isGSPDABought;
    public AnimationCurve gspdaPopUpCurve;
    public float gspdaTime;
    private float gspdaProgress;
    private bool gspdaFullyUp = false;
    public Transform gspdaButton;
    public Transform[] gspdaPosRefs;
    public GameObject gspdaThings;

    // on ship propulsion, rotate body ref on local y

    [Space]
    [Header("Ship stuff:")]
    public Rigidbody2D shipRb;
    public Transform shipBody;
    public Rigidbody2D shipTargetRotRb;
    public Transform shipSpinBody;
    private Vector2 movementInput;
    [Tooltip("5 Ship Speed levels:\n1 - 300;\n2 - 600;\n3 - 900;\n4 - 1200;\n5 - 1500.")]
    public float shipSpeed;
    public int speedLevel = 1;
    public float[] shipSpeedArray;
    public Vector2 shipWorldSize;
    public float shipSpinSpeed;
    public float shipBodySmoothSpeed;

    public float stunTimer;
    public float stunDefaultTime;

    public ShipCollision shipCollision;

    [Space]
    [Tooltip("higher mining yield upgrade")]
    public float miningRigRewardMult = 1f;
    public AnimationCurve miningRigMultCurve;
    public int miningRigLevel;
    public MeshRenderer miningRigMesh;
    public gonmts[] miningRigEvolutions;
    public float[] cogRandomSpeed;

    [Space]
    // farther areas give more asteroids with new ones of higher quality sprinkled here and there

    // There are talks about a new mining spot somewhere in the Asteroid Belt, which went unnoticed by prospectors for years. Discoverers mention larger amount of asteroids, as well as higher densities of <uncommon> and <rare>... But they won't hand out that information for free.

    public float shipSpinMinimumSpeed;

    [Space]
    [Header("Asteroids:")]
    public float asteroidSpawnCooldown = 14f;

    // Impact on the cooldown:
    // - random mult (varied, +-10%, applied to base value only, for more "interesting" spawning intervals)
    // - "relocation" upgrade (positive, )
    // - Asteroid Frenzy (positive, )
    // - remaining asteroid belt percentage (negative, 500% slower spawn cooldown)
    // - playtime mult (positive, the closer to endgame, the lower spawn time (up to 2x faster))

    public float asteroidCool;

    public int currentArea;
    public Vector2 areaMultRange;

    public Vector2 playtimeMultRange;

    public Vector2 remainingABMult;

    public abLvl[] levels;

    public LayerMask spawnCheckMask;

    [SerializeField]
    public Transform[] spawnPositions;
    public AnimationCurve positionDistribution;

    [Space]
    [Header("Minigame window:")]
    public AnimationCurve windowCurve;
    public float windowToggleTime;
    private float windowProgress;
    //public bool windowState; // true - opening; false - closing
    public float windowDirection = -1f;

    public Transform windowTransform;
    [Tooltip("0 - Down;\n1 - Up")]
    public Transform[] windowPositions;

    public float firstTimeCutsceneTime;
    private float firstTimeCutsceneProgress;

    public RectTransform maskTransform;

    [Space]
    public AnimationCurve asteroidCallerCooldownCurve;
    public AnimationCurve asteroidCallerScaleCurve;
    public float aCallerCooldown; // ticks down; how long will Player have to wait until another Asteroid Caller is available
    public float aCallerDuration;
    public float aCallerProgress; // ticks down; how long will Asteroid Caller persist
    public float aCallerSpeedMult = 0.075f;

    private float aCallerDurForFill;

    public Transform aCallerTransform;
    public Image aCallerIcon;
    public GameObject aCallerGO;

    public Gradient shipBeamLevelColor;

    private bool acUnlocked;

    public MeshRenderer beam4Renderer;
    public Vector2 beam4ScrollSpeed;
    private Vector2 beam4ScrollProgress;

    [Space]
    public AnimationCurve reblinkCurve;
    public float reblinkTime;
    [HideInInspector]
    public float reblinkProgress;

    public Transform[] allBeams;
    public float[] allBeamScales;

    public float beam4RotSpeed;
    private float beam4RotProgress;

    public AnimationCurve beam4ShakeCurve;
    public float beam4ShakeTime;
    private float beam4ShakeProgress;

    [Space]
    [Tooltip("0 - sunlight;\n1 - asteroid belt")]
    public AnimationCurve[] asteroRotationCurves;
    public Transform solLight;
    public ParticleSystem[] asteroParticles;

    [Space]
    public AnimationCurve acCircleDistCurve;
    public float acCircleDistTime;
    public float acCircleDistProgress;
    public LineRenderer[] acCircleDistLines;
    public Vector3[] acDefPointVectors;
    public GameObject acCircleHolder;
    public Gradient acColorGrad;

    [Space]
    public Transform[] positionsToAutograb;

    private int lastSpeedMode;

    // Asteroid types:
    // - Regular = 100
    // - Silver = 1,000 (Palladium)
    // - Gold = 30,000 ()
    // - Green Gems = 600,000 (Wrigglite Nightstone)
    // - Alien Purple Rock = 16,000,000 (Yakumenium)

    // chances (area):

    // common - 75%
    // uncommon - 23%
    // rare - 2%

    // leftover - place-dependent leftovers chance (chosen before the rarity picker, leftover gets chosen randomly)

    // SOMEWHERE MIDGAME A CHARITY ORG'S DISCOVERY FOR A CHILDREN'S SHELTER COUND BE FOUND AND SENT TO THE MINING RIG, ONLY WOULD SPAWN IF THE PLAYER IS IN THE ASTEROID FIELD

    // 1
    // regular (common)
    // regular (uncommon)
    // silver (rare)

    // 2
    // regular (common)
    // silver (uncommon)
    // gold (rare)

    // 3
    //      regular (leftovers, 15%)
    // silver (common)
    // gold (uncommon)
    // green gems (rare)

    // 4
    //      regular, silver (leftovers, 10%)
    // gold (common)
    // green gems (uncommon)
    // alien purple rocks (rare)

    // 5
    //      regular, silver, gold (leftovers, 8%)
    // green gems (common)
    // green gems (uncommon)
    // alien purple rocks (rare)
    private void Awake()
    {
        for (int i = 0; i < acCircleDistLines.Length; i++)
        {
            acCircleDistLines[i].positionCount = acDefPointVectors.Length;
        }
        acCircleDistProgress = acCircleDistTime;
    }

    private void FixedUpdate()
    {
        if (isGSPDABought)
        {
            // Asteroid Call
            if (acUnlocked)
            {
                aCallerProgress = Mathf.Max(aCallerProgress - Time.deltaTime, 0f);
                if (aCallerProgress == 0)
                {
                    aCallerCooldown = Mathf.Max(aCallerCooldown - Time.deltaTime, 0f);
                }
                aCallerTransform.localScale = Vector3.one * asteroidCallerScaleCurve.Evaluate(aCallerCooldown / aCallerDurForFill);
                aCallerIcon.fillAmount = 1f - (aCallerCooldown / aCallerDurForFill);
            }

            // GravShip
            bool oneTimeForBump = stunTimer != 0;
            stunTimer = Mathf.Max(stunTimer - Time.fixedDeltaTime, 0f);
            movementInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

            if (stunTimer == 0)
            {
                if (windowProgress != 0)
                {
                    shipRb.AddForce(movementInput * shipSpeed * Time.fixedDeltaTime);
                }
                if (oneTimeForBump)
                {
                    shipCollision.switchLights(true);
                }
            }

            Vector2 lookDir = -shipRb.linearVelocity.normalized;
            float angle = System.MathF.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f + 180f;
            shipTargetRotRb.rotation = angle;

            // Asteroid Spawning
            if (windowProgress != 0)
            {
                asteroidCool = Mathf.Max(asteroidCool - Time.fixedDeltaTime, 0);
                if (asteroidCool == 0)
                {
                    spawnAsteroid();
                } // spawn asteroid, apply next cooldown
            }
        }
    }
    private void Update()
    {
        if (isGSPDABought)
        {
            if (!gspdaFullyUp)
            {
                gspdaProgress = Mathf.Min(gspdaProgress + Time.deltaTime, gspdaTime); // GravShip PDA

                gspdaButton.position = Vector3.Lerp(gspdaPosRefs[0].position, gspdaPosRefs[1].position, gspdaPopUpCurve.Evaluate(gspdaProgress / gspdaTime));

                gspdaFullyUp = (gspdaProgress == gspdaTime);
            }

            reblinkProgress = Mathf.Min(reblinkProgress + Time.deltaTime, reblinkTime); // lights turn back on

            beam4ShakeProgress = Mathf.Repeat(beam4ShakeProgress + Time.deltaTime, beam4ShakeTime); // beam4 shaking idle animation

            bool isACCircleDone = (acCircleDistProgress == acCircleDistTime);
            acCircleDistProgress = Mathf.Min(acCircleDistProgress + Time.deltaTime, acCircleDistTime); // wave circles for asteroid caller

            /*if (allBeams[0].localScale.x != reblinkCurve.Evaluate(reblinkProgress / reblinkTime))
            {
                for (int i = 0; i < allBeams.Length; i++)
                {
                    allBeams[i].localScale = Vector3.one * reblinkCurve.Evaluate(reblinkProgress / reblinkTime) * allBeamScales[i];
                }
            }//*/

            acCircleHolder.SetActive(!isACCircleDone);

            if (!isACCircleDone)
            {
                print("yi8awferuh");

                Color32 acColor = acColorGrad.Evaluate(acCircleDistCurve.Evaluate(acCircleDistProgress / acCircleDistTime) / acCircleDistCurve.Evaluate(1f));

                for (int i = 0; i < acCircleDistLines.Length; i++)
                {
                    Vector3[] newPoses = new Vector3[acDefPointVectors.Length];
                    for (int a = 0; a < acDefPointVectors.Length; a++)
                    {
                        newPoses[a] = acDefPointVectors[a] * acCircleDistCurve.Evaluate(acCircleDistProgress / acCircleDistTime);
                        newPoses[a] *= (((float)i + 1f) / (float)acCircleDistLines.Length) * 0.5f;
                        newPoses[a] += Vector3.ClampMagnitude(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 1f) * 0.5f;
                        newPoses[a] += acCircleHolder.transform.position;
                    }
                    acCircleDistLines[i].SetPositions(newPoses);

                    acCircleDistLines[i].startColor = acColor;
                    acCircleDistLines[i].endColor = acColor;
                }
            }

            for (int i = 0; i < allBeams.Length; i++)
            {
                allBeams[i].localScale = Vector3.one * reblinkCurve.Evaluate(reblinkProgress / reblinkTime) * allBeamScales[i];
            }

            if (shipCollision.beamLevel == 3)
            {
                beam4ScrollProgress.x = Mathf.Repeat(beam4ScrollProgress.x + Time.deltaTime * beam4ScrollSpeed.x, 1f);
                beam4ScrollProgress.y = Mathf.Repeat(beam4ScrollProgress.y + Time.deltaTime * beam4ScrollSpeed.y, 1f);

                beam4Renderer.material.SetTextureOffset("_MainTex", beam4ScrollProgress);

                beam4RotProgress = Mathf.Repeat(beam4RotProgress + Time.deltaTime * beam4RotSpeed, 360f);
                allBeams[3].localRotation = Quaternion.Euler(beam4RotProgress, 0, 0);

                allBeams[3].localScale *= beam4ShakeCurve.Evaluate(beam4ShakeProgress / beam4ShakeTime);
            }

            if (shipRb.linearVelocity.magnitude != 0)
            {
                shipBody.rotation = Quaternion.Lerp(shipBody.rotation, shipTargetRotRb.transform.rotation, shipBodySmoothSpeed * Time.deltaTime);
                //shipSpinBody.Rotate(Mathf.Max(shipRb.linearVelocity.magnitude * shipSpinSpeed * shipSpeed, shipSpinMinimumSpeed) * Time.deltaTime * Vector3.up);
            }
            shipSpinBody.Rotate(Mathf.Max(shipRb.linearVelocity.magnitude * shipSpinSpeed * shipSpeed, shipSpinMinimumSpeed) * Time.deltaTime * Vector3.up);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //toggleWindow();
            }
            windowProgress = Mathf.Clamp(windowProgress + Time.deltaTime * windowDirection, 0f, windowToggleTime);

            windowTransform.gameObject.SetActive(windowProgress != 0);
            // stop minigame asteroids from spawning at that moment too

            windowTransform.position = Vector3.Lerp(windowPositions[0].position, windowPositions[1].position, windowCurve.Evaluate(windowProgress / windowToggleTime));
        }
    }
    void spawnAsteroid()
    {
        // apply cooldown
        asteroidCool = asteroidSpawnCooldown * Random.Range(0.95f, 1.1f); // random mult
        asteroidCool *= Mathf.Lerp(areaMultRange.x, areaMultRange.y, (float)currentArea / (float)levels.Length); // relocation
        asteroidCool *= Mathf.Lerp(playtimeMultRange.x, playtimeMultRange.y, mManager.gameManager.playtimePercentage); // playtime mult
        asteroidCool *= Mathf.Lerp(remainingABMult.x, remainingABMult.y, mManager.orbManager.asteroidBeltRemaining); // remaining asteroid belt percentage
        if (aCallerProgress != 0)
        {
            asteroidCool *= aCallerSpeedMult;
        } // asteroid frenzy

        // spawn asteroid
        GameObject currentBody = default;
        if (Random.Range(0f, 100f) < levels[currentArea].leftoversChance)
        {
            currentBody = Instantiate(levels[currentArea].leftovers[Random.Range(0, levels[currentArea].leftovers.Length)]);
        } // leftovers
        else
        {
            int rarityChance = Random.Range(0, 101);
            if (rarityChance <= 10)
            {
                currentBody = Instantiate(levels[currentArea].rare);
            } // rare
            else if (rarityChance <= 40)
            {
                currentBody = Instantiate(levels[currentArea].uncommon);
            } // uncommon
            else
            {
                currentBody = Instantiate(levels[currentArea].common);
            } // common
        } // asteroids

        // position
        Vector3 desPos = default;
        Vector3 desDire = default;
        bool canProceed = false;
        for (int pity = 0; pity < 4; pity++)
        {
            float randPerc = positionDistribution.Evaluate(Random.Range(0f, 1f)); // nice inverted bell curve
            Vector3 tDesPos = default;
            Vector3 tDesDire = default;
            for (int i = 0; i < spawnPositions.Length - 1; i++)
            {
                if (randPerc <= (float)(i + 1) / (float)(spawnPositions.Length - 1))
                {
                    tDesPos = Vector3.Lerp(spawnPositions[i].position, spawnPositions[i + 1].position, randPerc);
                    tDesDire = Vector3.Lerp(spawnPositions[i].up, spawnPositions[i + 1].up, randPerc);
                    break;
                } // assign position and direction
            }

            float randStrength = 0.1f;
            tDesDire = (tDesDire + (new Vector3(Random.Range(-randStrength, randStrength), Random.Range(-randStrength, randStrength), Random.Range(-randStrength, randStrength)))).normalized;

            // raycast check
            if (aCallerProgress == 0 || !Physics2D.Raycast(tDesPos, tDesDire, 200f, spawnCheckMask))
            {
                canProceed = true;
                desPos = tDesPos;
                desDire = tDesDire;
                break;
            }
        }
        if (canProceed)
        {
            currentBody.transform.position = desPos;
            currentBody.GetComponent<Asteroid>().spawnDirection = desDire;
            //print(desDire);
            currentBody.SetActive(true);
        } // can proceed
        else
        {
            Destroy(currentBody);
        } // can't proceed, despawn asteroid and leave
    }
    public void hit(float stunMult = 1f)
    {
        if (stunTimer == 0)
        {
            stunTimer = stunDefaultTime * stunMult; // more expensive asteroids will give a minor mult (weight), max is 1.3f
        }
    }
    public void advanceArea()
    {
        currentArea++;

        solLight.localRotation = Quaternion.Euler(asteroRotationCurves[0].Evaluate(currentArea / 4f), 80f, 0);
        for (int i = 0; i < asteroParticles.Length; i++)
        {
            asteroParticles[i].transform.localRotation = Quaternion.Euler(0, 180f, asteroRotationCurves[0].Evaluate(currentArea / 4f));
            asteroParticles[i].Stop();
            asteroParticles[i].Clear();
            asteroParticles[i].Play();

        }
    }
    public void advanceSpeed()
    {
        speedLevel++;
        print("adding 1 level, now at: " + speedLevel);
        shipSpeed = shipSpeedArray[speedLevel];

        /*if (speedLevel == 5)
        {

        } // disable button//*/
    }
    public void advanceBeam()
    {
        shipCollision.beamLevel++;
        shipCollision.switchLights(true);

        //print("adding 1 level, now at: " + shipCollision.beamLevel);

        /*if (shipCollision.beamLevel == 3)
        {

        } // disable button//*/
    }
    public void advanceMiningRigMult()
    {
        miningRigLevel++;
        miningRigRewardMult = miningRigMultCurve.Evaluate(miningRigLevel / 9);

        miningRigMesh.materials = miningRigEvolutions[miningRigLevel].materials;
        for (int i = 0; i < miningRigEvolutions[miningRigLevel].gameObjects.Length; i++)
        {
            miningRigEvolutions[miningRigLevel].gameObjects[i].SetActive(true);
            if (miningRigEvolutions[miningRigLevel].gameObjects[i].GetComponent<Animator>())
            {
                miningRigEvolutions[miningRigLevel].gameObjects[i].GetComponent<Animator>().SetBool("isOpposite", (Random.Range(0, 2) == 1));
                miningRigEvolutions[miningRigLevel].gameObjects[i].GetComponent<Animator>().speed = Random.Range(cogRandomSpeed[0], cogRandomSpeed[1]);
            }
        } // enable
    }
    public void toggleWindow()
    {
        if (isGSPDABought)
        {
            float ownDire = windowDirection;
            mManager.closeAllWindows();
            windowDirection = -ownDire;
            //mManager.toggleCam(windowDirection);

            mManager.gameManager.lockSpeed(false);
            if (windowDirection == 1)
            {
                lastSpeedMode = mManager.gameManager.getSpeedMode();
                mManager.gameManager.changeSpeed(3);
                mManager.gameManager.lockSpeed(true);
            }
            else
            {
                mManager.gameManager.changeSpeed(lastSpeedMode);
            }

            Input.ResetInputAxes();
        }
    }
    public void asteroidCaller()
    {
        if (aCallerCooldown == 0)
        {
            aCallerCooldown = asteroidCallerCooldownCurve.Evaluate(currentArea / (levels.Length - 1f));
            aCallerProgress = aCallerDuration;

            aCallerDurForFill = aCallerCooldown;

            asteroidCool = Random.Range(0f, aCallerSpeedMult);

            //print("asterocall");
            acCircleDistProgress = 0;
            acCircleHolder.transform.position = shipCollision.transform.position;
        }
    }
    public void aCallerInitiate()
    {
        aCallerGO.SetActive(true);
        acUnlocked = true;
        aCallerCooldown = asteroidCallerCooldownCurve.Evaluate(currentArea / (levels.Length - 1f)) * 0.3f;
        aCallerDurForFill = asteroidCallerCooldownCurve.Evaluate(currentArea / (levels.Length - 1f));
    }
    public void buyGravShipPDA()
    {
        isGSPDABought = true;
        gspdaProgress = 0;
        gspdaThings.SetActive(true);
        gspdaButton.gameObject.SetActive(true);
    }
}
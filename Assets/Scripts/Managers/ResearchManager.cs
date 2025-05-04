using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class cso
{
    public AnimationCurve motionCurve;
    public float motionTime;
    public float motionProgress;
    public Transform[] motionTargets;

    [Space]
    public Transform[] movedObjects;
}
[System.Serializable]
public class resLv
{
    public string name;
    public string fullName;
    public string description;
    public string unfinishedDescAdd;
    public string finishedDescAdd;
    [Tooltip("probably will go unused, most prolly, idk")]
    public string completionMessage;

    [Space]
    public bool useLevelExclusiveDescAdd;
    public string[] levelExclusiveDescAdd;

    [Space]
    [Tooltip("when ALL research is finished (or when it just finishes, in case of singular research stages)")]
    public bool isCompleted;

    [Space]
    public int price;
    public float duration;
    public float researchProgress;
    public Slider progressSlider;

    [Tooltip("for levellables only")]
    public AnimationCurve priceProgressionCurve;
    [Tooltip("for levellables only")]
    public AnimationCurve durationProgressionCurve;

    [Space]
    public bool isLevellable;
    public int currentLevel;
    public int maxLevel;
    public Slider levelSlider;

    [Space]
    //[Tooltip("Children:\n0 - New;\n1 - Affordable;\n2 - In progress;\n3 - Maxed out")]
    public GameObject[] researchUnlocks;

    [Space]
    public bool isResearching;

    public Image researchProgressor;
}
public class ResearchManager : MonoBehaviour
{
    public MainManager mManager;

    public resLv[] research;

    public TMP_Text researchName;
    public TMP_Text researchDesc;
    public TMP_Text researchPriceDuration;

    public Buttons[] researchButtons;
    public GameObject finalResearch;

    public cso[] researchPages;
    public float motionDirection = -1f;

    [Space]
    public CanvasGroup bgBubbles;

    public AnimationCurve bubblesRiseCurve;
    public float riseTime;
    private float riseProgress;

    [Tooltip("flicker.")]
    public AnimationCurve bubblesBlinkCurve;
    public float blinkTime;
    private float blinkProgress;

    public AnimationCurve bubblesFadeCurve;
    public float fadeTime;
    public float fadeProgress;

    public bool devMode;
    private void Awake()
    {
        changeText();
        for (int i = 0; i < research.Length; i++)
        {
            research[i].researchProgress = research[i].duration;
            researchButtons[i].researchIndex = i;

            if (research[i].isLevellable)
            {
                research[i].levelSlider = researchButtons[i].GetComponentInChildren<Slider>();
                research[i].levelSlider.maxValue = research[i].maxLevel;
            }
            research[i].researchProgressor = researchButtons[i].transform.GetChild(2).GetComponent<Image>();
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < research.Length; i++)
        {
            if (!research[i].isCompleted && research[i].isResearching)
            {
                research[i].researchProgress = Mathf.Min(research[i].researchProgress + Time.fixedDeltaTime * mManager.gameManager.gameSpeed, research[i].duration); // * researchSpeedMult from politics manager

                research[i].researchProgressor.fillAmount = 1f - (research[i].researchProgress / research[i].duration);

                if (research[i].researchProgress == research[i].duration)
                {
                    research[i].isResearching = false;
                    for (int a = 0; a < research[i].researchUnlocks.Length; a++)
                    {
                        research[i].researchUnlocks[a].SetActive(true);
                        //print("add an animation or something");
                    }

                    researchButtons[i].transform.GetChild(2).gameObject.SetActive(false);

                    if (research[i].isLevellable)
                    {
                        research[i].currentLevel++;
                        if (research[i].currentLevel < research[i].maxLevel)
                        {
                            research[i].duration *= research[i].durationProgressionCurve.Evaluate(research[i].currentLevel / research[i].maxLevel);
                            research[i].price = Mathf.CeilToInt(research[i].price * research[i].priceProgressionCurve.Evaluate(research[i].currentLevel / research[i].maxLevel));
                            researchButtons[i].locked = false;
                            onResearchFinish(i);
                            researchCheck(research[i].name, research[i].currentLevel);
                        }
                        else
                        {
                            completeResearch(i);
                            onResearchFinish(i);
                            researchCheck(research[i].name, research[i].currentLevel);
                        }
                    }
                    else
                    {
                        completeResearch(i);
                        onResearchFinish(i);
                        researchCheck(research[i].name);
                    }
                }
            }
        }
    }
    private void Update()
    {
        for (int i = 0; i < researchPages.Length; i++)
        {
            researchPages[i].motionProgress = Mathf.Clamp(researchPages[i].motionProgress + Time.deltaTime * motionDirection, 0f, researchPages[i].motionTime);
            for (int a = 0; a < researchPages[i].movedObjects.Length; a++)
            {
                researchPages[i].movedObjects[a].position = Vector3.Lerp(researchPages[i].motionTargets[0].position, researchPages[i].motionTargets[1].position, researchPages[i].motionCurve.Evaluate(researchPages[i].motionProgress / researchPages[i].motionTime));
            }
        }

        // bubble particles floating upwards control of some sorts
        riseProgress = Mathf.Repeat(riseProgress + Time.deltaTime, riseTime); // bubbles rising
        blinkProgress = Mathf.Repeat(blinkProgress + Time.deltaTime, blinkTime); // bubbles blinking in bg
        fadeProgress = Mathf.Clamp(fadeProgress + Time.deltaTime * motionDirection, 0f, fadeTime); // intro

        print(fadeProgress == 0f);

        bgBubbles.gameObject.SetActive(fadeProgress / fadeTime != 0);
        if (fadeProgress / fadeTime != 0f)
        {
            bgBubbles.alpha = bubblesFadeCurve.Evaluate(fadeProgress / fadeTime);
            bgBubbles.alpha *= bubblesBlinkCurve.Evaluate(blinkProgress / blinkTime);
            bgBubbles.transform.localPosition = Vector3.up * bubblesRiseCurve.Evaluate(riseProgress / riseTime);
        }
    }
    void researchCheck(string input, int currentLevel = -1)
    {
        switch (input)
        {
            case "gravShipPDA":
                mManager.minigameManager.buyGravShipPDA();
                break;
            case "miningRigEfficiency":
                mManager.minigameManager.advanceMiningRigMult();
                break;
            case "asteroidCaller":
                mManager.minigameManager.aCallerInitiate();
                break;
            case "betterThrusters":
                mManager.minigameManager.advanceSpeed();
                break;
            case "relocation":
                mManager.minigameManager.advanceArea();
                break;
            case "upgradedGravFields":
                mManager.minigameManager.advanceBeam();
                break;
            case "missionOperative":
                mManager.missionManager.addMissionSlot(currentLevel - 1);
                break;
            case "asteroidHurling":
                mManager.missionManager.unlockOperation(0, 0);
                break;
            case "verneGun":
                mManager.missionManager.unlockOperation(0, 1);
                break;
            case "500TONNukeEagleCharge":
                mManager.missionManager.unlockOperation(0, 2);
                break;
            case "asteroidSyphon":
                mManager.missionManager.unlockOperation(1, 0);
                break;
            case "gasVenting":
                mManager.missionManager.unlockOperation(2, 0);
                break;
            case "arcCannon":
                mManager.missionManager.unlockOperation(2, 1);
                break;
            case "reflectiveSolarSail":
                mManager.missionManager.unlockOperation(3, 0);
                break;
            case "hiredMiners":
                mManager.missionManager.unlockOperation(3, 1);
                break;
            case "3TONNukeStabCharge":
                mManager.missionManager.unlockOperation(3, 2);
                break;
            case "orbitalDisconnect":
                mManager.missionManager.unlockOperation(3, 3);
                break;
            case "celestialEngine":
                mManager.missionManager.unlockOperation(4, 0);
                break;
        }
    }
    public void allResearchCheck()
    {
        for (int i = 0; i < research.Length - 5; i++)
        {
            if (!research[i].isCompleted)
            {
                //print("Failed on " + i + " ( " + research[i].name + " )");
                return;
            }
        }
        if (!research[research.Length - 1].isCompleted)
        {
            //print("Failed on " + (research.Length - 1) + " ( " + research[(research.Length - 1)].name + " )");
            return;
        } // literally just Orbital Disconnect
        finalResearch.SetActive(true);
    }
    void onResearchFinish(int index)
    {
        changeText(index);
        allResearchCheck();

        if (research[index].isLevellable)
        {
            print("ADD LEVEL");
            research[index].levelSlider.value = research[index].currentLevel;

            if (research[index].currentLevel == research[index].maxLevel)
            {
                //print("MAXED OUT");
                research[index].levelSlider.gameObject.SetActive(false);
            }
        }
    } // must work for ALL types of research finishes, that is: finishing unlevelling research, levelling up levelled research, and finishing levelled research
    private void completeResearch(int index)
    {
        researchButtons[index].transform.GetChild(3).gameObject.SetActive(true);
        research[index].isCompleted = true;
    }
    public void changeText(int index = -1)
    {
        if (index != -1)
        {
            researchName.text = research[index].fullName;
            researchDesc.text = levelify(research[index].description, index);
            researchPriceDuration.text = "Cost: " + research[index].price + " Marks\nDuration: " + research[index].duration + " years";

            if (research[index].isCompleted)
            {
                researchDesc.text += levelify(research[index].finishedDescAdd, index);
                researchPriceDuration.text = "\n[MAXED OUT]";
            }
            else
            {
                researchDesc.text += levelify(research[index].unfinishedDescAdd, index);
                if (research[index].duration == 1)
                {
                    researchPriceDuration.text = "Cost: " + research[index].price + " Marks\nDuration: " + research[index].duration + " year";
                }
                if (research[index].isResearching)
                {
                    researchPriceDuration.text += "|" + index + "|<resPerc>";
                }
            }

            if (research[index].useLevelExclusiveDescAdd)
            {
                researchDesc.text += research[index].levelExclusiveDescAdd[research[index].currentLevel];
            }

            researchButtons[index].researchTime = research[index].duration;
            researchButtons[index].researchProgress = research[index].duration;

            // if is levellable, modify own research time and cost based on the curves

            return;
        }

        researchName.text = "";
        researchDesc.text = "";
        researchPriceDuration.text = "Cost:\nDuration:";
    }
    string levelify(string input, int index)
    {
        return input.Replace("<lvl>", research[index].currentLevel + "").Replace("<lvl+>", (research[index].currentLevel + 1) + "");
    }
    public bool buyResearch(int index)
    {
        if (mManager.gameManager.money >= research[index].price)
        {
            startResearch(index);
            return true;
        }
        return false;
    }
    public void startResearch(int index)
    {
        mManager.gameManager.addCash(-research[index].price);
        research[index].isResearching = true;
        research[index].researchProgress = 0;
        researchButtons[index].researchTime = research[index].duration;
    }
    public void togglePage()
    {
        float ownDire = motionDirection;
        mManager.closeAllWindows();
        motionDirection = -ownDire;
        mManager.toggleCam(motionDirection);
    }
}

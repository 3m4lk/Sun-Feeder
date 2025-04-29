using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public bool devMode;
    private void Awake()
    {
        changeText();
        for (int i = 0; i < research.Length; i++)
        {
            research[i].researchProgress = research[i].duration;
            researchButtons[i].researchIndex = i;
        }
    }
    private void Update()
    {
        for (int i = 0; i < research.Length; i++)
        {
            if (!research[i].isCompleted && research[i].isResearching)
            {
                research[i].researchProgress = Mathf.Min(research[i].researchProgress + Time.deltaTime * mManager.gameManager.gameSpeed, research[i].duration); // * researchSpeedMult from politics manager
                if (research[i].researchProgress == research[i].duration)
                {
                    research[i].isResearching = false;
                    for (int a = 0; a < research[i].researchUnlocks.Length; a++)
                    {
                        research[i].researchUnlocks[a].SetActive(true);
                        print("add an animation or something");
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
    void researchCheck(string input, int currentLevel = -1)
    {
        switch (input)
        {
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
            case "celestialEngine":
                mManager.missionManager.unlockOperation(4, 0);
                break;
        }
    }
    public void allResearchCheck()
    {
        for (int i = 0; i < research.Length - 4; i++)
        {
            if (!research[i].isCompleted)
            {
                return;
            }
        }
        finalResearch.SetActive(true);
    }
    void onResearchFinish(int index)
    {
        changeText(index);
        allResearchCheck();
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
}

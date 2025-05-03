using TMPro;
using UnityEngine;

[System.Serializable]
public class pAc
{
    public string name;
    public string fullName;
    public string description;

    [Space]
    [Tooltip("0 - Vox Populi;\n1 - Vox Aequalis;\n2 - Vox Coitionis")]
    [Range(0, 2)]
    public int movement;

    [Space]
    public float unlockRequirements;
    [Tooltip("whan above this threshold, action gets locked and cannot be used & chosen")]
    public float lockRequirements;

    [Space]
    public bool special;

    [Space]
    [Tooltip("value given per 1 year\n(initial calculation: impact / years to finish)\n(I - 30; II - 40; III - 50; IV - 40; V - 30; VI - 10)")]
    public float growth;
    [Tooltip("movement cap")]
    public float impact;

    [Space]
    public bool isActive;
    [Tooltip("can't be used if it's locked")]
    public bool isLocked;

    public GameObject ownButton; // might be unneeded in the future??

    public int buttonColor;

    public void lockToggle(bool input)
    {

    }
}
public class PoliticsManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    [Range(-100f, 100f)]
    public float politicalViews;
    public pAc[] actions;

    public float extremismThreshold = 70f;
    public float neutralismThreshold = 10f;

    public GameObject[] voxPopuliExtremism;
    public GameObject[] voxCoitionisExtremism;

    public float currentCap;
    [Tooltip("value given per 10 years")]
    public float currentGrowth;

    public float politicsMult = 1f;

    [Tooltip("slightly above aequalis -> extremism")]
    public AnimationCurve SGSChance;

    // SGS (Sudden Government Shift)

    [Space]
    [Tooltip("completely honest, i haven't a slightest idea why i even called it a \"pike\", i just didn't have any better idea and \"pike\" just sorta clicked for whatever reason??")]
    public Transform scalePike;
    public Transform[] scalePlates;
    public float maxPikeRotation;

    [Space]
    public AnimationCurve descBgCurve;
    public float descBgTime;
    private float descBgProgress;
    public Transform descBg;

    [Space]
    public polButton[] buttons;

    [Space]
    [Tooltip("0 - Populi;\n1 - Populi Extremist;\n2 - Coitionis;\n3 - Coitionis Extremist;\n4 - War")]
    public Color32[] buttonColors;

    public TMP_Text infoName, infoDesc;

    private float warLock;
    public int warTime;

    public TMP_Text modifiersText;
    public TMP_Text polStatusText;

    private bool isExtremist;
    private bool isAtWar;

    private float poliUpdateProgress;

    [Space]
    public AnimationCurve pikeCurve;
    public float pikeTime;
    private float pikeProgress;

    private float lastRot;
    private float targetRot;
    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].ownText.text = actions[i].fullName;
            buttons[i].buttonTag = actions[i].name;
            buttons[i].ownImage.color = buttonColors[actions[i].buttonColor];
            buttons[i].ownName = actions[i].fullName;
            buttons[i].ownDescription = actions[i].description;
        }
    }
    private void FixedUpdate()
    {
        poliUpdateProgress += Time.fixedDeltaTime * mManager.gameManager.gameSpeed;

        for (; poliUpdateProgress >= 1f; poliUpdateProgress--)
        {
            updatePolitical();
        }

        warLock = Mathf.Max(warLock - Time.fixedDeltaTime, 0f);

        /*if (politicalViews < -neutralismThreshold)
        {
            politicalViews = Mathf.Max(politicalViews + (currentGrowth / 10f) * politicsMult * mManager.gameManager.gameSpeed * Time.fixedDeltaTime, currentCap);
        } // vox populi
        else if (politicalViews > neutralismThreshold)
        {
            politicalViews = Mathf.Min(politicalViews + (currentGrowth / 10f) * politicsMult * mManager.gameManager.gameSpeed * Time.fixedDeltaTime, currentCap);
        } // vox coitionis//*/
    }
    private void Update()
    {
        pikeProgress = Mathf.Min(pikeProgress + Time.deltaTime, pikeTime);
        descBgProgress = Mathf.Repeat(descBgProgress + Time.deltaTime, descBgTime);

        descBg.localRotation = Quaternion.Euler(0, 0, descBgCurve.Evaluate(descBgProgress / descBgTime));

        float pikeRot = Mathf.Lerp(lastRot, targetRot, pikeCurve.Evaluate(pikeProgress / pikeTime));

        scalePike.localRotation = Quaternion.Euler(0, 0, -pikeRot);
        for (int i = 0; i < scalePlates.Length; i++)
        {
            scalePlates[i].localRotation = Quaternion.Euler(0, 0, pikeRot);
        }
    }
    public void doAction(string input, bool mode)
    {
        if (!actions[getActionIndex(input)].isLocked)
        {
            if (warLock != 0)
            {
                switch (input)
                {
                    default:
                        actions[getActionIndex(input)].isActive = mode;
                        break;
                    case "pWar":
                        // unwar is not allow
                        break;
                    case "cWar":
                        // unwar is not allow
                        break;
                }
            }
            else
            {
                actions[getActionIndex(input)].isActive = mode;
                switch (input)
                {
                    case "pWar":
                        warLock = warTime;
                        break;
                    case "cWar":
                        warLock = warTime;
                        break;
                }
            }
        }
        updateActions();
    }
    int getActionIndex(string input)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i].name == input)
            {
                return i;
            }
        }
        return -1;
    }
    public void SuddenGovernmentShift()
    {
        if (Random.Range(0, 100) <= Mathf.RoundToInt(SGSChance.Evaluate(Mathf.Abs(politicalViews) / 100f)))
        {
            print("success! apply opposite growth that lasts for some years");
        }
        else
        {
            print("failure! add a couple % described as \"people protesting the attempted changes\" :)");
        }
    }
    public void updateInfo(string title = default, string description = default, bool lockState = false)
    {
        infoName.text = title;
        infoDesc.text = description;
    }
    void updateActions()
    {
        currentGrowth = 0;
        currentCap = 0;
        modifiersText.text = "";
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i].isActive)
            {
                string[] modColors = new string[5] { "#FF5C80", "#FF0038", "#5C80FF", "#0038FF", "#000000" };
                modifiersText.text += "<color=" + modColors[actions[i].buttonColor] + ">+" + Mathf.Abs(actions[i].growth) + ": " + actions[i].fullName + "</color>\n";
                // add text color based on politics

                currentGrowth += actions[i].growth;
                currentCap += actions[i].impact;
                switch (actions[i].name)
                {
                    case "pWar":
                        politicalViews = -100f;
                        currentGrowth = -100f;
                        currentCap = -100f;
                        return; // War (populi)
                    case "cWar":
                        politicalViews = 100f;
                        currentGrowth = 100f;
                        currentCap = 100f;
                        return; // War (coitionis)
                }
            }
        }
    }
    void updatePolitical()
    {
        if (currentGrowth > 0)
        {
            if (Mathf.Abs(politicalViews) < Mathf.Abs(currentCap))
            {
                politicalViews = Mathf.Clamp(politicalViews + currentGrowth, -100f, currentCap);
            }
        } // coitionis
        else if (currentGrowth < 0)
        {
            if (Mathf.Abs(politicalViews) < Mathf.Abs(currentCap))
            {
                politicalViews = Mathf.Clamp(politicalViews + currentGrowth, currentCap, 100f);
            }
        } // populis
        else
        {
            politicalViews = Mathf.Clamp(politicalViews + (0.001f * -Mathf.Sign(politicalViews)), -100f, 100f);
            // do very slight growth in opposite direction (growth is equal 0)
        } // aequalis

        //politicalViews = Mathf.Clamp(politicalViews, -100f, 100f); // the ultimate clamp - don't touch it and don't alter politics beyond this very point

        if (!isExtremist && Mathf.Abs(politicalViews) >= extremismThreshold)
        {
            isExtremist = true;
            // spawn extremism news
        }
        else if (isExtremist && Mathf.Abs(politicalViews) < extremismThreshold)
        {
            isExtremist = false;
            // spawn no longer extremist news
        }

        // per action check for locking actions

        // update the scales
        lastRot = targetRot;
        targetRot = maxPikeRotation * politicalViews;
        pikeProgress = 0;

        polStatusText.text = ((int)politicalViews).ToString("0");
    } // called every year
}
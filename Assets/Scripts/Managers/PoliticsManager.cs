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
    [Tooltip("value given per 10 years")]
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
    [Range(-100f, 100f)]
    public float politicalViewsTest;
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
        if (politicalViews < -neutralismThreshold)
        {
            politicalViews = Mathf.Max(politicalViews + (currentGrowth / 10f) * politicsMult * mManager.gameManager.gameSpeed * Time.fixedDeltaTime, currentCap);
        } // vox populi
        else if (politicalViews > neutralismThreshold)
        {
            politicalViews = Mathf.Min(politicalViews + (currentGrowth / 10f) * politicsMult * mManager.gameManager.gameSpeed * Time.fixedDeltaTime, currentCap);
        } // vox coitionis

        // for now
        scalePike.localRotation = Quaternion.Euler(0, 0, maxPikeRotation * politicalViewsTest);
        for (int i = 0; i < scalePlates.Length; i++)
        {
            scalePlates[i].localRotation = Quaternion.Euler(0, 0, -maxPikeRotation * politicalViewsTest);
        }
    }
    private void Update()
    {
        descBgProgress = Mathf.Repeat(descBgProgress + Time.deltaTime, descBgTime);
        descBg.localRotation = Quaternion.Euler(0, 0, descBgCurve.Evaluate(descBgProgress / descBgTime));
    }
    public void addAction(string input)
    {
        pAc currAction = getAction(input);
        currentGrowth += currAction.growth;
        currentCap += currAction.impact;

        switch (input)
        {
            case "pWar":
                politicalViews = -100f;
                break; // War (populi)
            case "cWar":
                politicalViews = 100f;
                break; // War (coitionis)
        }
    }
    public void removeAction(string input)
    {
        pAc currAction = getAction(input);
        currentGrowth -= currAction.growth;
        currentCap -= currAction.impact;
    }
    public pAc getAction(string input)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i].name == input)
            {
                return actions[i];
            }
        }
        return null;
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
}
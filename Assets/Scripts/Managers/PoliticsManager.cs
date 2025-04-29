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

    public GameObject ownButton;

    public void lockToggle(bool input)
    {

    }
}
public class PoliticsManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    [Range(-100f, 100f)]
    public float views;
    public pAc[] actions;

    public float extremismThreshold = 70f;

    public GameObject[] voxPopuliExtremism;
    public GameObject[] voxCoitionisExtremism;

    public float currentCap;
    [Tooltip("value given per 10 years")]
    public float currentGrowth;

    public float politicsMult = 1f;

    [Tooltip("slightly above aequalis -> extremism")]
    public AnimationCurve SGSChance;

    // SGS (Sudden Government Shift)

    private void FixedUpdate()
    {
        if (views < 0)
        {
            views = Mathf.Max(views + (currentGrowth / 10f) * politicsMult * mManager.gameManager.gameSpeed * Time.fixedDeltaTime, currentCap);
        } // vox populi
        else
        {
            views = Mathf.Min(views + (currentGrowth / 10f) * politicsMult * mManager.gameManager.gameSpeed * Time.fixedDeltaTime, currentCap);
        }
    }
    public void addAction(string input)
    {
        pAc currAction = getAction(input);
        currentGrowth += currAction.growth;
        currentCap += currAction.impact;
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
        if (Random.Range(0, 100) <= Mathf.RoundToInt(SGSChance.Evaluate(Mathf.Abs(views) / 100f)))
        {
            print("success! apply opposite growth that lasts for some years");
        }
        else
        {
            print("failure! add a couple % described as \"people protesting the attempted changes\" :)");
        }
    }
}
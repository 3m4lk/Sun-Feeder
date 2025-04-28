using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class resLv
{
    public string name;
    public string fullName;
    public string description;
    [Tooltip("probably will go unused, most prolly, idk")]
    public string completionMessage;

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
    public AnimationCurve DurationProgressionCurve;

    [Space]
    public bool isLevellable;
    public int currentLevel;
    public int maxLevel;
    public Slider levelSlider;

    [Space]
    //[Tooltip("Children:\n0 - New;\n1 - Affordable;\n2 - In progress;\n3 - Maxed out")]
    public GameObject[] researchUnlocks;
}
public class ResearchManager : MonoBehaviour
{
    public resLv[] research;

    public TMP_Text researchName;
    public TMP_Text researchDesc;
    public TMP_Text researchPriceDuration;

    public Buttons[] researchButtons;
    private void Awake()
    {
        changeText();
    }
    public bool allResearchCheck()
    {
        for (int i = 0; i < research.Length - 4; i++)
        {
            if (!research[i].isCompleted)
            {
                return false;
            }
        }
        return true;
    }
    public void changeText(int index = -1)
    {
        if (index != -1)
        {
            researchName.text = research[index].fullName;
            researchDesc.text = research[index].description;
            researchPriceDuration.text = "Cost: " + research[index].price + "\nDuration: " + research[index].duration;
            researchButtons[index].researchTime = research[index].duration;
            researchButtons[index].researchProgress = research[index].duration;

            // if is levellable, modify own research time and cost based on the curves

            return;
        }

        researchName.text = "";
        researchDesc.text = "";
        researchPriceDuration.text = "Cost:\nDuration:";
    }
}

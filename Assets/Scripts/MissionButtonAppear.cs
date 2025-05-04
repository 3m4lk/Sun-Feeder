using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonAppear : MonoBehaviour
{
    public MissionManager missionManager;

    [Space]
    public string infoName;
    public string infoDesc;
    public string infoDescMinor;

    [Space]
    [Header("for the celestial body choice thing only")]
    public string buttonTextSingle;
    public string buttonTextMultiple;

    [Space]
    public int index;
    public int missionType = -1;

    public string buttonType;

    [Space]
    public Color32 defaultColor = Color.white;
    public Color32 hoverColor = new Color32(128, 128, 128, 255);
    public void onClick()
    {
        //missionManager.openTab(index, missionType);
        print("open thing for mission: ");

        switch (buttonType)
        {
            case "missionSlotSelect":
                if (!missionManager.missions[index].inProgress)
                {
                    missionManager.updateSlotArrow(transform);
                    // reset previously written data when backtracking

                    if (missionManager.selectedMissionTarget != -1)
                    {
                        for (int i = 0; i < missionManager.tabButtons[missionManager.selectedMissionType].bodies.Length; i++)
                        {
                            missionManager.tabButtons[missionManager.selectedMissionType].bodies[i].SetActive(false);
                        }

                        missionManager.resetSlot(index);
                    }

                    missionManager.selectedMissionType = -1;
                    missionManager.selectedMission = -1;
                    missionManager.selectedMissionTarget = -1;

                    missionManager.selectedMissionSlot = index;

                    missionManager.updateSlotIcons();

                    missionManager.openTab(1);
                }
                else
                {
                    missionManager.resetSlot(index);
                }
                break;
            case "missionBodyTypeSelect":
                //print("TAB2");
                missionManager.missions[missionManager.selectedMissionSlot].missionIcon.sprite = missionManager.missionTypeIcons[missionType];
                missionManager.missions[missionManager.selectedMissionSlot].progressFill.sprite = missionManager.missionTypeIcons[missionType];

                missionManager.selectedMissionType = missionType;
                missionManager.openTab(2, missionType);
                break;
            case "missionTypeSelect":
                missionManager.selectedMission = missionType;
                missionManager.openTab(3, missionType);
                break;
            case "missionTargetBodySelect":
                missionManager.selectedMissionTarget = index;
                missionManager.selectedMissionColor = GetComponentInChildren<TMP_Text>().color;
                missionManager.missions[missionManager.selectedMissionSlot].missionIcon.color = missionManager.selectedMissionColor;
                print("make mission initiable: " + missionType);
                break;
            case "missionStart":
                //print("start current mission");
                missionManager.startMission();
                break;
            case "goBack":
                missionManager.openTab(index);
                switch (index)
                {
                    case 0:
                        missionManager.updateSlotIcons();

                        missionManager.updateSlotArrow(missionManager.initialSlotArrowTarget);

                        missionManager.selectedMissionColor = Color.white;

                        missionManager.selectedMissionSlot = -1;
                        break; // return to page 0; reset selected mission slot
                    case 1:
                        missionManager.updateSlotIcons();

                        missionManager.selectedMissionType = -1;
                        break; // return to page 1; reset selected mission type (gas giant, planet, etc.)
                    case 2:
                        for (int i = 0; i < missionManager.tabButtons[missionManager.selectedMissionType].bodies.Length; i++)
                        {
                            missionManager.tabButtons[missionManager.selectedMissionType].bodies[i].SetActive(false);
                        }
                        missionManager.selectedMission = -1;
                        missionManager.selectedMissionTarget = -1;

                        missionManager.selectedMissionColor = Color.white;
                        missionManager.missions[missionManager.selectedMissionSlot].missionIcon.color = missionManager.selectedMissionColor;
                        break; // return to page 2; reset selected mission (arc cannon, rss, etc.) and mission target (Pluto, Venus etc.)
                }
                break;
        }
    }
    public void onEnter()
    {
        missionManager.infoBoxTexts[0].text = infoName;
        missionManager.infoBoxTexts[1].text = infoDesc;
        missionManager.infoBoxTexts[2].text = infoDescMinor;

        if (GetComponent<Image>())
        {
            GetComponent<Image>().color = hoverColor;
        }
        else if (GetComponentInChildren<Image>())
        {
            GetComponentInChildren<Image>().color = hoverColor;
        }

        switch (buttonType)
        {
            case "missionSlotSelect":
                missionManager.missionedBodyCamAnchorChange(index);
                // change camera target, if mission is undergoing
                break;
        }
    }
    public void onExit()
    {
        missionManager.infoBoxTexts[0].text = default;
        missionManager.infoBoxTexts[1].text = default;
        missionManager.infoBoxTexts[2].text = default;

        if (GetComponent<Image>())
        {
            GetComponent<Image>().color = defaultColor;
        }
        else if (GetComponentInChildren<Image>())
        {
            GetComponentInChildren<Image>().color = defaultColor;
        }
    }
    private string amountify(string input, int ownIndex, int amount)
    {
        //return ownIndex + input.Replace("<amount>", amount + "").Replace("<lastBody>", missionManager.mManager.celManager.bodyCluster[ownIndex].lastRemainingName);
        return input.Replace("<amount>", amount + "").Replace("<lastBody>", missionManager.mManager.celManager.bodyCluster[ownIndex].lastRemainingName);
    }
    public void cbMenuUpdateButtonText(int ownIndex, int amount)
    {
        switch (amount)
        {
            default:
                GetComponentInChildren<TMP_Text>().text = amountify(buttonTextMultiple, ownIndex, amount);
                break; // plural
            case 0:
                gameObject.SetActive(false);
                break; // none; disable
            case 1:
                GetComponentInChildren<TMP_Text>().text = amountify(buttonTextSingle, ownIndex, amount);
                break; // singular; add the last body's name
        }
    }
}
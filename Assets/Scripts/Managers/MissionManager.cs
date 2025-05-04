using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class mdbe
{
    public string name;
    public float duration;
    public int cost;
}
[System.Serializable]
public class tb
{
    public string name;
    public GameObject mainButton;
    public GameObject[] buttons;
    public bool[] isUnlocked;

    [Space]
    public GameObject[] bodies;
}
[System.Serializable]
public class mssn
{
    public string name;
    public bool inProgress;
    public float duration;
    public float progress;

    public Color32 missionIconColor = Color.white;

    public Image missionIcon;

    public Image progressFill;

    [Space]
    public int missionTypeInt;

    [Space]
    public Transform missionBody;
}
public class MissionManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    [Tooltip("0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants;\n3 - Minor Bodies;\n4 - Sol")]
    public tb[] tabButtons;

    public mssn[] missions = new mssn[5];

    public mdbe[] missionDatabase;

    [Space]
    public AnimationCurve windowCurve;
    public float windowTime;
    private float windowProgress;
    public Transform[] windowPositions;
    public Transform windowTransform;

    [Space]
    public AnimationCurve bgScreenCurve;
    public float bgScreenTime;
    private float bgScreenProgress;
    public Transform[] bgScreenPositions;
    public Transform bgScreenTransform;

    public float animDirection = 1f;

    [Space]
    [Header("Mission tabs, buttons and such")]
    public int currentTab;

    public Transform[] allTabs;
    public float tabMoveTime;
    public float[] tabMoveProgresses;
    public AnimationCurve tabMoveCurve;
    [Tooltip("0 - hidden;\n1 - selected")]
    public Transform[] tabPositions;

    public GameObject initialNoOperationsText;

    [Tooltip("0 - Name;\n1-  Description;\n2 - Minor Description")]
    public TMP_Text[] infoBoxTexts;

    public GameObject[] invitationTexts;

    public GameObject[] missionSlotGOs;

    public Sprite[] missionTypeIcons;
    public Sprite freeSlotIcon;

    [Space]
    public int selectedMissionSlot;
    public int selectedMissionType;
    public int selectedMission;
    public int selectedMissionTarget;
    public Color32 selectedMissionColor;

    [Space]
    public Transform currentSlotArrow;
    public Transform slotArrowTarget;
    public Transform initialSlotArrowTarget;
    public AnimationCurve slotArrowCurve;
    public float slotArrowTime;
    private float slotArrowProgress;
    private Vector3 arrowLastPosition;
    public void addMissionSlot(int index)
    {
        invitationTexts[0].SetActive(true);
        invitationTexts[1].SetActive(false);

        missionSlotGOs[index].SetActive(true);
    }
    public void unlockOperation(int type, int index)
    {
        tabButtons[type].isUnlocked[index] = true;
        //tabButtons[type].buttons[index].SetActive(true);
        if (!tabButtons[type].mainButton.activeSelf)
        {
            tabButtons[type].mainButton.SetActive(true);
            tabButtons[type].mainButton.transform.SetAsLastSibling();
            initialNoOperationsText.SetActive(false);
        }
    } // 0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol

    public missionVisProp[] missionProps = new missionVisProp[5];
    private void Awake()
    {
        slotArrowTarget = initialSlotArrowTarget;
        arrowLastPosition = initialSlotArrowTarget.position;

        animDirection = -1f;
    }
    private void Update()
    {
        windowProgress = Mathf.Clamp(windowProgress + Time.deltaTime * animDirection, 0f, windowTime);
        bgScreenProgress = Mathf.Clamp(bgScreenProgress + Time.deltaTime * animDirection, 0f, bgScreenTime);

        //windowTransform.gameObject.SetActive(windowProgress != 0);

        windowTransform.position = Vector3.Lerp(windowPositions[0].position, windowPositions[1].position, windowCurve.Evaluate(windowProgress / windowTime));
        bgScreenTransform.position = Vector3.Lerp(bgScreenPositions[0].position, bgScreenPositions[1].position, bgScreenCurve.Evaluate(bgScreenProgress / bgScreenTime));

        if (bgScreenProgress != 0)
        {
            slotArrowProgress = Mathf.Min(slotArrowProgress + Time.deltaTime, slotArrowTime);
            Vector3 slotArrowPos = Vector3.Lerp(arrowLastPosition, slotArrowTarget.position, slotArrowCurve.Evaluate(slotArrowProgress / slotArrowTime));
            slotArrowPos.y = currentSlotArrow.position.y;
            currentSlotArrow.position = slotArrowPos;

            for (int i = 0; i < allTabs.Length; i++)
            {
                float direction = 1f;
                if (i != currentTab)
                {
                    direction = -1f;
                }
                tabMoveProgresses[i] = Mathf.Clamp(tabMoveProgresses[i] + Time.deltaTime * direction, 0, tabMoveTime);

                allTabs[i].position = Vector3.Lerp(tabPositions[0].position, tabPositions[1].position, tabMoveCurve.Evaluate(tabMoveProgresses[i] / tabMoveTime));
            }
        }
    }
    private void FixedUpdate()
    {
        // mission progress updating
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].inProgress)
            {
                missions[i].progress = Mathf.Min(missions[i].progress + Time.fixedDeltaTime * mManager.gameManager.gameSpeed, missions[i].duration);

                missions[i].progressFill.fillAmount = missions[i].progress / missions[i].duration;

                missionAnimUpdate(missions[i].name, missions[i].progress / missions[i].duration);

                if (missions[i].progress == missions[i].duration)
                {
                    print("FINISH MISSION " + i + "!");
                    resetSlot(i);
                }
            }
        }
    }
    public void missionedBodyCamAnchorChange(int index)
    {
        //missions[index]
    }
    void missionAnimUpdate(string missionType, float missionProgress)
    {
        switch (missionType)
        {
            default:
                break;
        }
    }
    public void openTab(int index, int missionType = -1, string missionTag = default)
    {
        currentTab = index;

        switch (index)
        {
            case 2:
                print("toggle");
                for (int i = 0; i < tabButtons.Length; i++)
                {
                    for (int a = 0; a < tabButtons[i].buttons.Length; a++)
                    {
                        tabButtons[i].buttons[a].SetActive(false);
                    }
                }
                for (int i = 0; i < tabButtons[selectedMissionType].buttons.Length; i++)
                {
                    tabButtons[selectedMissionType].buttons[i].SetActive(tabButtons[selectedMissionType].isUnlocked[i]);
                }
                break; // selectedMissionType
            case 3:
                for (int i = 0; i < tabButtons[selectedMissionType].bodies.Length; i++)
                {
                    tabButtons[selectedMissionType].bodies[i].SetActive(true);
                }
                break; // mission type select
        }
    }
    public void updateSlotIcons()
    {
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].inProgress)
            {
                missions[i].missionIcon.sprite = missionTypeIcons[missions[i].missionTypeInt];
                missions[i].progressFill.sprite = missionTypeIcons[missions[i].missionTypeInt];
                missions[i].missionIcon.color = missions[i].missionIconColor;
            }
            else
            {
                missions[i].missionIcon.sprite = freeSlotIcon;
                missions[i].progressFill.sprite = freeSlotIcon;
                missions[i].missionIcon.color = Color.white;
            }
        }
    }
    public void startMission()
    {
        if (selectedMissionTarget != -1)
        {
            string missName = tabButtons[selectedMissionType].buttons[selectedMission].name;

            mdbe dbEntry = getMissionData(missName);

            if (mManager.gameManager.money >= dbEntry.cost)
            {
                if (selectedMissionType == 0 && selectedMissionTarget == 2)
                {
                    mManager.commanderManager.hurlEarthScenario();
                    return;
                }
                print("Current slot: " + selectedMissionSlot + "\nType: " + selectedMissionType + "\nMission: " + selectedMission + "\nTarget: " + selectedMissionTarget);

                mManager.gameManager.addCash(-dbEntry.cost);

                for (int i = 0; i < tabButtons[selectedMissionType].bodies.Length; i++)
                {
                    tabButtons[selectedMissionType].bodies[i].SetActive(false);
                }

                missions[selectedMissionSlot].name = missName; // set name
                                                                                                                    // set duration
                missions[selectedMissionSlot].progress = 0;
                missions[selectedMissionSlot].inProgress = true; // start mission
                missions[selectedMissionSlot].missionIconColor = selectedMissionColor;
                missions[selectedMissionSlot].missionTypeInt = selectedMissionType;

                missions[selectedMissionSlot].duration = dbEntry.duration;
                print("durtion multiplied by politics mult");
                // also add the actual price calculation things somewhere in the scripts above

                // set missioned body transform



                selectedMissionSlot = -1;
                selectedMissionType = -1;
                selectedMission = -1;
                selectedMissionTarget = -1;
                openTab(0);
                updateSlotArrow(initialSlotArrowTarget);
            }
            else
            {
                print("<color=#789922>>poor</color>");
            }
        }
    }
    public void resetSlot(int index)
    {
        missions[index].inProgress = false;
        missions[index].progress = 0;
        missions[index].progressFill.fillAmount = 0;

        updateSlotIcons();
    }
    public void updateSlotArrow(Transform target)
    {
        slotArrowTarget = target;
        slotArrowProgress = 0;
        arrowLastPosition = currentSlotArrow.position;
    }
    public mdbe getMissionData(string input)
    {
        for (int i = 0; i < missionDatabase.Length; i++)
        {
            if (missionDatabase[i].name == input)
            {
                mdbe output = new mdbe();
                output.name = missionDatabase[i].name;
                output.cost = missionDatabase[i].cost;
                output.duration = missionDatabase[i].duration;
                return output;
            }
        }
        return null;
    }
    public void toggleWindow()
    {
        float ownDire = animDirection;
        mManager.closeAllWindows();
        animDirection = -ownDire;
        mManager.toggleCam(animDirection);
    }
}
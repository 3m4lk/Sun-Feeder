using UnityEngine;

[System.Serializable]
public class tb
{
    public string name;
    public GameObject[] buttons;
    public bool[] isUnlocked;
}
public class MissionManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    [Tooltip("0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol")]
    public tb[] tabButtons;

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
    public void unlockOperation(int type, int index)
    {
        tabButtons[type].isUnlocked[index] = true;
        initialNoOperationsText.SetActive(false);
    } // 0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol
    private void Update()
    {
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
    public void openTab(int index, int missionType = -1)
    {
        currentTab = index;
        if (missionType != -1)
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                for (int a = 0; a < tabButtons[i].buttons.Length; a++)
                {
                    tabButtons[i].buttons[a].SetActive(false);
                }
            }
            for (int i = 0; i < tabButtons[missionType].buttons.Length; i++)
            {
                tabButtons[missionType].buttons[i].SetActive(tabButtons[missionType].isUnlocked[i]);
            }
        }
    }
}

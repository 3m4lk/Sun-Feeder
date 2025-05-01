using UnityEngine;

public class MissionButtonAppear : MonoBehaviour
{
    public MissionManager missionManager;

    [Space]
    public int index;
    public bool isMissionTab;
    public int missionType = -1;
    public void onClick()
    {
        missionManager.openTab(index, missionType);
    print("open thing for mission: ");
    }
    public void onEnter()
    {

    }
    public void onExit()
    {

    }
}

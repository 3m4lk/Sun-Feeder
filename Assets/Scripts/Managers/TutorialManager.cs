using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public MainManager mManager;

    [Tooltip("0 - GravShip PDA;\n" +
        "1 - ")]
    public bool[] tutChecks;

    public void checkTut(int index)
    {
        if (!tutChecks[index])
        {
            switch (index)
            {
                case 0:
                    mManager.popupManager.newPopup("tut14");
                    mManager.gameManager.money = 0;
                    mManager.gameManager.addCash(150);
                    GameObject[] allsteroids = GameObject.FindGameObjectsWithTag("asteroidChild");
                    for (int i = 0; i < allsteroids.Length; i++)
                    {
                        Destroy(allsteroids[i].transform.parent.gameObject);
                    }
                    break;
                case 1:
                    mManager.popupManager.wasMinigame = false;
                    mManager.closeAllWindows();
                    mManager.popupManager.newPopup("tut15");
                    break; // tutorial: invitation to politics
                case 2:
                    mManager.popupManager.newPopup("tut16");
                    break; // tutorial: in politics
                // 3: switched to true when tut18 pops up
                case 4:
                    if (tutChecks[3])
                    {
                        mManager.popupManager.newPopup("tut19");
                    }
                    else return;
                    break; // tutorial: exiting politics & invitation to missions
                case 5:
                    mManager.popupManager.newPopup("tut20");
                    break;
                case 6:
                    if (!mManager.researchManager.isResearch("reflectiveSolarSail"))
                    {
                        print("no research / not in the window");
                        return; // or if isn't in the window
                    }

                    mManager.popupManager.newPopup("tut21");
                    break; // tutorial: after reflective solar sail finishes & mission tab is opened
            }
            tutChecks[index] = true;
        }
    }
}

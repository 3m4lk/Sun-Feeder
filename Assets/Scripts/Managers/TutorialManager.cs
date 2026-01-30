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
                    break;
                case 1:
                    mManager.popupManager.wasMinigame = false;
                    mManager.closeAllWindows();
                    mManager.popupManager.newPopup("tut15");
                    break; // tutorial: invitation to politics
                case 2:
                    mManager.popupManager.newPopup("tut16");
                    break; // tutorial: in politics
            }
            tutChecks[index] = true;
        }
    }
}

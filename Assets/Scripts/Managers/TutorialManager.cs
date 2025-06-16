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
            }
            tutChecks[index] = true;
        }
    }
}

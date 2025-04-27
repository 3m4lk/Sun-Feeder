using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public MainManager mManager;
    public void gravshipPDA()
    {
        mManager.minigameManager.windowState = !mManager.minigameManager.windowState; // now do a cool animation!
    }
}

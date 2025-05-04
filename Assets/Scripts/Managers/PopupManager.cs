using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public MainManager mManager;
    private int lastSpeedMode;

    [Tooltip("used for tutorial only, most probably")]
    public string currentTag;

    public bool isActive;

    public GameManager ownPopup;
    [Tooltip("0 - title;\n1 - description;\n2 - proceed button")]
    public TMP_Text[] popupTexts;

    public void spawnPopup(string pTag, string title, string description, string buttonName = "Okay")
    {
        lastSpeedMode = mManager.gameManager.getSpeedMode();
        mManager.gameManager.lockSpeed(false); // precaution for minigame
        mManager.gameManager.changeSpeed(0);
        mManager.gameManager.lockSpeed(true);

        currentTag = pTag;
        popupTexts[0].text = title;
        popupTexts[1].text = description;
        popupTexts[2].text = buttonName;
    }
    public void closePopup()
    {
        mManager.gameManager.changeSpeed(lastSpeedMode);
        if (mManager.minigameManager.windowDirection == -1)
        {
            mManager.gameManager.lockSpeed(false);
        } // game speed doesn't unlock when minigame is up
    }
}

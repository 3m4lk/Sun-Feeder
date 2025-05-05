using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class pdt
{
    public string ownTag;
    public string title;
    public string description;
    public string buttonName;

    [Header("rule for colors: HSV S channel is ALWAYS 60, V ALWAYS at 100")]
    public Color32 windowColor = Color.HSVToRGB(240, 60, 100) + new Color32(0, 0, 0, 255);

    public Vector2 position;
    public Vector2 size = new Vector2(128, 92);

    [Space]
    public string onCloseOpen;

    public bool wasAlreadyOpened;

    public bool addBlocker;
    public Vector2 blockerPos;

    public GameObject[] objectsToEnable;
}
public class PopupManager : MonoBehaviour
{
    public MainManager mManager;
    public int lastSpeedMode;

    public pdt[] popups;
    private int currentPopupIndex;

    public bool isActive;

    public GameObject ownPopup;
    [Tooltip("0 - title;\n1 - description;\n2 - proceed button")]
    public TMP_Text[] popupTexts;

    [Space]
    public AnimationCurve popupAppearCurve;
    public float popupAppearTime;
    private float popupAppearProgress;
    private float popupDirection = -1f;

    public int testPopup;
    private bool wasMinigame;

    public AnimationCurve blockerCurve;
    public float blockerTime;
    private float blockerProgress = 0;

    public CanvasGroup blocker;

    [Space]
    public pdt popupDev;

    //public GameObject[] enableAll;
    private void Awake()
    {
        mManager.gameManager.changeSpeed(0);
        //spawnPopup(0);
        newPopup("tut0");
    }
    private void Update()
    {
        popupAppearProgress = Mathf.Clamp(popupAppearProgress + Time.deltaTime * popupDirection, 0f, popupAppearTime);
        ownPopup.transform.localScale = Vector3.one * popupAppearCurve.Evaluate(popupAppearProgress / popupAppearTime);

        blockerProgress = Mathf.Min(blockerProgress + Time.deltaTime, blockerTime);

        blocker.alpha = blockerCurve.Evaluate(blockerProgress / blockerTime);
    }
    public void newPopup(string puTag)
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (popups[i].ownTag == puTag)
            {
                if (popups[i].wasAlreadyOpened)
                {
                    return;
                }
                spawnPopup(i);
                return;
            }
        }
        print("<color=red>POPUP NOT FOUND</color>");
    }
    public void devTestPopup()
    {
        spawnPopup(testPopup);
    }
    void spawnPopup(int index)
    {
        switch (popups[index].ownTag)
        {
            case "tut6":
                mManager.gameManager.money = 0;
                //mManager.gameManager.addCash(110);
                print("zsfgd vaav ");
                break;
        }

        wasMinigame = false;
        if (mManager.minigameManager.windowDirection == 1f)
        {
            mManager.closeAllWindows();
            wasMinigame = true;
        } // close minigame window to prevent exploiting

        popups[index].wasAlreadyOpened = true;
        lastSpeedMode = mManager.gameManager.getSpeedMode();
        mManager.gameManager.lockSpeed(false); // precaution for minigame
        mManager.gameManager.changeSpeed(0);
        mManager.gameManager.lockSpeed(true);

        currentPopupIndex = index;
        popupTexts[0].text = popups[index].title;
        popupTexts[1].text = popups[index].description;
        popupTexts[2].text = popups[index].buttonName;

        ownPopup.transform.localPosition = popups[index].position;
        ownPopup.GetComponent<RectTransform>().sizeDelta = popups[index].size;

        popupDirection = 1f;
        popupAppearProgress = 0;
        ownPopup.GetComponent<Image>().color = popups[index].windowColor;

        ownPopup.SetActive(true);

        mManager.toggleCam(1f);

        for (int i = 0; i < popups[index].objectsToEnable.Length; i++)
        {
            popups[index].objectsToEnable[i].SetActive(true);
        }

        blocker.gameObject.SetActive(popups[index].addBlocker);
        if (popups[index].addBlocker)
        {
            blocker.transform.localPosition = popups[index].blockerPos;
            blockerProgress = 0;
        }
    }
    public void closePopup()
    {
        mManager.gameManager.lockSpeed(false);
        mManager.gameManager.changeSpeed(lastSpeedMode);

        if (wasMinigame)
        {
            wasMinigame = false;
            mManager.minigameManager.toggleWindow();
        }

        popupDirection = -1f;
        mManager.toggleCam(-1f);

        switch (popups[currentPopupIndex].ownTag)
        {
            default:
                break;
        } // exclusive stuff from popups

        if (popups[currentPopupIndex].onCloseOpen != "")
        {
            newPopup(popups[currentPopupIndex].onCloseOpen);
        }
    }
    public void disableBlocker()
    {
        blocker.gameObject.SetActive(false);
    }

    public void openByIndex()
    {

    }
}

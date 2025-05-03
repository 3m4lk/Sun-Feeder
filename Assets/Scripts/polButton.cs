using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class polButton : MonoBehaviour
{
    public PoliticsManager politics;

    public string buttonTag;

    public string ownName;
    public string ownDescription;

    public bool isLocked;
    public bool mode;

    public TMP_Text ownText;
    public Image ownImage;

    [Space]
    public AnimationCurve hoverCurve;
    public float hoverTime;
    private float hoverProgress;
    private float animDire = -1f;

    [Space]
    public AnimationCurve clickCurve;
    public float clickTime;
    private float clickProgress;
    private void Update()
    {
        hoverProgress = Mathf.Clamp(hoverProgress + Time.deltaTime * animDire, 0f, hoverTime);
        clickProgress = Mathf.Min(clickProgress + Time.deltaTime, clickTime);

        transform.localScale = Vector3.one * hoverCurve.Evaluate(hoverProgress / hoverTime);
        transform.localScale *= clickCurve.Evaluate(clickProgress / clickTime);
    }
    public void onEnter()
    {
        if (!isLocked)
        {
            animDire = 1f;
        }
        politics.updateInfo(ownName, ownDescription, isLocked);
    }
    public void onExit()
    {
        animDire = -1f;
        politics.updateInfo();
    }
    public void onClick()
    {
        if (!isLocked)
        {
            animDire = -1f;
            mode = !mode;
            if (mode)
            {
                politics.addAction(buttonTag);
                //return; // reckon if it's unneeded or nah?
            }
            else
            {
                politics.removeAction(buttonTag);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class mainScreenButtons : MonoBehaviour
{
    public MainManager mManager;
    public AnimationCurve iconScaleCurve;
    public float animTime;
    private float animProgress;
    private float animDirection = -1f;
    public Gradient colorDifference;
    public Image thingToScale;

    public int tutorialId = -1;

    private float gspdaTutTime = 1.5f;
    private void Update()
    {
        animProgress = Mathf.Clamp(animProgress + Time.deltaTime * animDirection, 0f, animTime);
        if (thingToScale)
        {
            thingToScale.transform.localScale = Vector3.one * iconScaleCurve.Evaluate(animProgress / animTime);
            thingToScale.color = colorDifference.Evaluate(animProgress / animTime);
        }

        if (gspdaTutTime < 1.5f)
        {
            gspdaTutTime += Time.deltaTime;
            if (gspdaTutTime >= 1.5f)
            {
                mManager.popupManager.newPopup("tut11");
            }
        }
    }
    public void onEnter()
    {
        animDirection = 1f;
    }
    public void onExit()
    {
        animDirection = -1f;
    }

    public void tutorialThings()
    {
        if (tutorialId > -1)
        {
            if (tutorialId == 11)
            {
                gspdaTutTime = 0f;
                return;
            }

            mManager.popupManager.newPopup("tut" + tutorialId);

            tutorialId = -1;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    [Tooltip("0 - Research;\n1 - ")]
    public int buttonMode;

    [Space]
    public int researchIndex;

    [Space]
    public AnimationCurve introScaleCurve;
    public float introTime; // this is going to be variable, and multiplied by the game speed (base low-tier research time should be like 8 years or so)
    public float introProgress;

    [Space]
    public AnimationCurve hoverScaleCurve;
    public bool isHovering;
    public float hoverTime;
    private float hoverProgress;

    [Space]
    public AnimationCurve downScaleCurve;
    public bool isDown;
    public float downTime;
    private float downProgress;

    [Space]
    public AnimationCurve positiveScaleCurve;
    public AnimationCurve negativeRotationCurve;
    public float negativeRotationStrength;
    public float clickTime;
    private float positiveProgress;
    private float negativeProgress;

    [Space]
    public AnimationCurve researchRotationCurve;
    public float researchTime; // this is going to be variable, and multiplied by the game speed (base low-tier research time should be like 8 years or so)
    public float researchProgress;

    [Space]
    public AnimationCurve idleRotationCurve;
    public AnimationCurve idleScaleCurve;

    public float idleScaleSpeedMult = 1f;

    public float idleAnimationTime;
    private float idleRotationProgress;
    private float idleScaleProgress;

    private bool gotHovered;
    private float idleScaleDirection;
    private float idleRotationDirection;

    public bool locked;

    public int tutExc = -1;

    [Tooltip("0 - idle;\n1 - hover;\n2 - click")]
    public Color32[] buttonColors;
    public Image ownImage;
    private void Awake()
    {
        if (buttonMode == 0)
        {
            introProgress = 0;

            researchProgress = researchTime;

            positiveProgress = clickTime;
            negativeProgress = clickTime;

            idleRotationProgress = Random.Range(0f, idleAnimationTime);
            idleScaleProgress = Random.Range(0f, idleAnimationTime);

            idleScaleDirection = 1f;
            idleRotationDirection = 1f;

            if (Random.Range(0, 2) == 0)
            {
                idleScaleDirection = -1f;
            }
            if (Random.Range(0, 2) == 1)
            {
                idleRotationDirection = -1f;
            }
            // two different randomizations, that's for a pretty good cause actually...
        }
    }
    public void onEnter()
    {
        if (locked) return;
        if (buttonMode == 1)
        {
            ownImage.color = buttonColors[1];
            return;
        }
        else if (buttonMode == 2)
        {
            ownImage.color = buttonColors[1];
            return;
        }

        isHovering = true;
        isDown = false;

        if (!gotHovered)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void researchEnter()
    {
        //if (locked) return;
        mManager.researchManager.changeText(researchIndex);
    }
    public void researchExit()
    {
        //if (locked) return;
        mManager.researchManager.changeText();
    }
    public void onExit()
    {
        if (locked) return;
        if (buttonMode == 1)
        {
            ownImage.color = buttonColors[0];
            return;
        }
        else if (buttonMode == 2)
        {
            ownImage.color = buttonColors[0];
            return;
        }

        isHovering = false;
        isDown = false;
    }
    public void onDown()
    {
        if (locked) return;
        if (buttonMode == 1)
        {
            ownImage.color = buttonColors[2];
            return;
        }
        else if (buttonMode == 2)
        {
            ownImage.color = buttonColors[2];
            return;
        }

        isDown = true;
    }
    public void onClick()
    {
        if (locked) return;
        if (buttonMode == 1)
        {
            ownImage.color = buttonColors[0];
            mManager.gameManager.changeSpeed(researchIndex);
            return;
        }
        else if (buttonMode == 2)
        {
            ownImage.color = buttonColors[0];
            mManager.popupManager.closePopup();
            return;
        }

        isDown = false;

        if (mManager.researchManager.buyResearch(researchIndex))
        {
            positiveProgress = 0;
            researchProgress = 0;
            locked = true;
            isHovering = false;
            transform.GetChild(2).gameObject.SetActive(true);
            //mManager.researchManager.changeText();

            if (tutExc > -1)
            {
                mManager.popupManager.newPopup("tut" + tutExc);
                tutExc = -1;
            }

            researchEnter();
        }
        else
        {
            negativeProgress = 0;
        }
    }
    private void Update()
    {
        if (buttonMode == 0)
        {
            // Intro
            introProgress = Mathf.Min(introProgress + Time.deltaTime, introTime);
            transform.localScale = Vector3.one * introScaleCurve.Evaluate(introProgress / introTime);

            // Research rotation speed multiplier

            researchProgress = mManager.researchManager.research[researchIndex].researchProgress;

            float researchRotSpeedMult = researchRotationCurve.Evaluate(researchProgress / researchTime);
            if (researchTime == 0)
            {
                researchRotSpeedMult = 1f;
            }

            // Idle
            idleRotationProgress = Mathf.Repeat(idleRotationProgress + Time.deltaTime * idleRotationDirection * researchRotSpeedMult, idleAnimationTime); // added research mult
            idleScaleProgress = Mathf.Repeat(idleScaleProgress + Time.deltaTime * idleScaleSpeedMult * idleScaleDirection * researchRotSpeedMult, idleAnimationTime);

            transform.localScale *= idleScaleCurve.Evaluate(idleScaleProgress / idleAnimationTime);
            transform.localRotation = Quaternion.Euler(0, 0, idleRotationCurve.Evaluate(idleRotationProgress / idleAnimationTime));

            // Hovering over
            if (isHovering)
            {
                hoverProgress = Mathf.Min(hoverProgress + Time.deltaTime, hoverTime);
            }
            else
            {
                hoverProgress = Mathf.Max(hoverProgress - Time.deltaTime, 0);
            }

            transform.localScale *= hoverScaleCurve.Evaluate(hoverProgress / hoverTime);

            // Clicking and holding
            if (isDown)
            {
                downProgress = Mathf.Min(downProgress + Time.deltaTime, downTime);
            }
            else
            {
                downProgress = Mathf.Max(downProgress - Time.deltaTime, 0);
            }

            transform.localScale *= downScaleCurve.Evaluate(downProgress / downTime);

            // After clicking (positive / negative outcome)

            positiveProgress = Mathf.Min(positiveProgress + Time.deltaTime, clickTime);
            negativeProgress = Mathf.Min(negativeProgress + Time.deltaTime, clickTime);

            transform.localScale *= positiveScaleCurve.Evaluate(positiveProgress / clickTime); // positive: scale only
            transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + negativeRotationStrength * negativeRotationCurve.Evaluate(negativeProgress / clickTime)); // negative: rotation onlynegativeProgress

            //idleRotationProgress *= negativeProgress / clickTime;

            // while researched, button's idle animation starts out very slowed dowm, but speeds up as research progresses (probably just a straight line curve, unless a different one works better; up to 1.5f speed)
        }
    }
}

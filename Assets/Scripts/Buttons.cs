using UnityEngine;

public class Buttons : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    public int researchIndex;

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

    public bool falseTest;

    public bool locked;
    private void Awake()
    {
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
    public void onEnter()
    {
        if (locked) return;
        isHovering = true;
        isDown = false;

        if (!gotHovered)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void researchEnter()
    {
        if (locked) return;
        mManager.researchManager.changeText(researchIndex);
    }
    public void researchExit()
    {
        if (locked) return;
        mManager.researchManager.changeText();
    }
    public void onExit()
    {
        if (locked) return;
        isHovering = false;
        isDown = false;
    }
    public void onDown()
    {
        if (locked) return;
        //isHovering = false;
        isDown = true;
    }
    public void onClick()
    {
        if (locked) return;
        //isHovering = false;
        isDown = false;

        if (falseTest)
        {
            negativeProgress = 0;
        }
        else
        {
            positiveProgress = 0;
            researchProgress = 0;
            locked = true;
            isHovering = false;
        }
    }
    private void Update()
    {
        // Research rotation speed multiplier

        researchProgress = Mathf.Min(researchProgress + Time.deltaTime * mManager.gameManager.gameSpeed, researchTime);

        float researchRotSpeedMult = researchRotationCurve.Evaluate(researchProgress / researchTime);

        // Idle
        idleRotationProgress = Mathf.Repeat(idleRotationProgress + Time.deltaTime * idleRotationDirection * researchRotSpeedMult, idleAnimationTime); // added research mult
        idleScaleProgress = Mathf.Repeat(idleScaleProgress + Time.deltaTime * idleScaleSpeedMult * idleScaleDirection * researchRotSpeedMult, idleAnimationTime);

        transform.localScale = Vector2.one * (idleScaleCurve.Evaluate(idleScaleProgress / idleAnimationTime));
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

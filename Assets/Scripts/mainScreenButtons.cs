using UnityEngine;
using UnityEngine.UI;

public class mainScreenButtons : MonoBehaviour
{
    public AnimationCurve iconScaleCurve;
    public float animTime;
    private float animProgress;
    private float animDirection = -1f;
    public Gradient colorDifference;
    public Image thingToScale;
    private void Update()
    {
        animProgress = Mathf.Clamp(animProgress + Time.deltaTime * animDirection, 0f, animTime);
        if (thingToScale)
        {
            thingToScale.transform.localScale = Vector3.one * iconScaleCurve.Evaluate(animProgress / animTime);
            thingToScale.color = colorDifference.Evaluate(animProgress / animTime);
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
}

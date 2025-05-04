using UnityEngine;

public class SolManager : MonoBehaviour
{
    public MainManager mManager;

    public AnimationCurve scaleCurve;
    public Gradient healthGradient;
    public MeshRenderer Sol;

    private void FixedUpdate()
    {
        Sol.transform.localScale = Vector3.one * scaleCurve.Evaluate(mManager.gameManager.playtimePercentage);
        Sol.material.SetColor("_Color", healthGradient.Evaluate(mManager.gameManager.playtimePercentage));// = healthGradient.Evaluate(1f - scaleCurve.Evaluate(mManager.gameManager.playtimePercentage) / scaleCurve.Evaluate(1f));
    }
}

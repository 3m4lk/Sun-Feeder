using UnityEngine;

public class SolManager : MonoBehaviour
{
    public MainManager mManager;

    public AnimationCurve scaleCurve;
    public Gradient healthGradient;
    public MeshRenderer Sol;

    [Header("above 100 for good ending, below for bad, above 400 for explosion ending")]
    [Header("Gas Giants: 500; Jupiter: 200; Saturn: 100; Uranus: 100; Neptune: 100")]
    [Header("Planets: ")]
    [Header("Dwarves: ")]
    public float SolHealth;

    private void FixedUpdate()
    {
        Sol.transform.localScale = Vector3.one * scaleCurve.Evaluate(mManager.gameManager.playtimePercentage);
        Sol.material.SetColor("_Color", healthGradient.Evaluate(mManager.gameManager.playtimePercentage));// = healthGradient.Evaluate(1f - scaleCurve.Evaluate(mManager.gameManager.playtimePercentage) / scaleCurve.Evaluate(1f));
    }
    public void addHealth(float amount)
    {
        SolHealth += amount;
    }
}

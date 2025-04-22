using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MainManager iManager;

    [Tooltip("1f - in real time (used as a funny easter egg only)")]
    public float gameSpeed = 1f;

    [Tooltip("distance from Sun to Earth; here, a multiplier")]
    public float AstronomicalUnit;
}

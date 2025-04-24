using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MainManager iManager;

    [Tooltip("1f - in real time (used as a funny easter egg only)")]
    //[Range((1f / 60f / 60f / 23.39444444444444444444f / 365.256363004f), 16f)] // realtime -> 16 YPS
    public float gameSpeed = 1f;

    [Tooltip("0 - Accurate;\n1 - 1 year per minute;\n2 - 0.1 year per second;\n3 - 1 year per second;\n4 - 3 years per second;\n5 - 10 years per second;\n6 - 16 years per second")]
    [Range(0, 6)]
    public int speedMode = 3;

    [Tooltip("distance from Sun to Earth; here, a multiplier")]
    public float AstronomicalUnit;
    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
    private void Update()
    {
        switch (speedMode)
        {
            case 0:
                gameSpeed = (1f / 60f / 60f / 23.39444444444444444444f / 365.256363004f);
                break;
            case 1:
                gameSpeed = (1f / 60f);
                break;
            case 2:
                gameSpeed = 0.1f;
                break;
            case 3:
                gameSpeed = 1f;
                break;
            case 4:
                gameSpeed = 3f;
                break;
            case 5:
                gameSpeed = 10f;
                break;
            case 6:
                gameSpeed = 16f;
                break;
        }
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Tooltip("1f - in real time (used as a funny easter egg only)")]
    public float gameSpeed = 1f;
    private void Awake()
    {
        instance = this;
    }
}

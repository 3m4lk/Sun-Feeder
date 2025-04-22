using UnityEngine;

[ExecuteInEditMode]
public class MainManager : MonoBehaviour
{
    public static MainManager instance { get; private set; }

    public OrbitManager orbManager;
    public GameManager gameManager;

    [ExecuteInEditMode]
    private void Awake()
    {
        instance = this;
        print("instanced!");
    }
}

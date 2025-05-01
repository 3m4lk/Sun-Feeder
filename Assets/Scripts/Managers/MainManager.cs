using UnityEngine;

[ExecuteInEditMode]
public class MainManager : MonoBehaviour
{
    public static MainManager instance { get; private set; }

    public OrbitManager orbManager;
    public GameManager gameManager;
    public CameraManager camManager;
    public MinigameManager minigameManager;
    public ResearchManager researchManager;
    public MissionManager missionManager;
    public CelBodyStatusManager celManager;
    public CommanderManager commanderManager;

    [ExecuteInEditMode]
    private void Awake()
    {
        instance = this;
        //print("instanced!");
    }
}

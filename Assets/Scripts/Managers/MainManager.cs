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
    public PoliticsManager politicsManager;
    public NewsManager newsManager;
    public PopupManager popupManager;

    [ExecuteInEditMode]
    private void Awake()
    {
        instance = this;
        //print("instanced!");
    }
    public void closeAllWindows()
    {
        minigameManager.windowDirection = -1f;
        researchManager.motionDirection = -1f;
        missionManager.animDirection = -1f;
        politicsManager.animDire = -1f;
    }
    public void toggleCam(float input)
    {
        camManager.toggleCameraControls(input);
    }
}

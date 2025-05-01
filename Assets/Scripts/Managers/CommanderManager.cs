using TMPro;
using UnityEngine;

public class CommanderManager : MonoBehaviour
{
    public MainManager mManager;

    public NameGenerator nameGen;
    public string commanderName;
    public float commanderLifetime;
    public AnimationCurve commanderLifetimeRange;
    [Tooltip("the longer you play, the higher Humans' lifespans will become, ")]
    public AnimationCurve commanderLifetimePlaytimeMult;

    public float commanderMournTime;
    private float commanderMournProgress;
    private bool assignNewCommander;

    public TMP_Text commanderText;
    private void Awake()
    {
        assignCommander();
    }
    private void FixedUpdate()
    {
        if (commanderMournProgress == 0)
        {
            if (assignNewCommander)
            {
                assignCommander();
            }
            else
            {
                commanderLifetime = Mathf.Max(commanderLifetime - Time.fixedDeltaTime * mManager.gameManager.gameSpeed, 0);
                if (commanderLifetime == 0)
                {
                    killCommander();
                }
            }
            commanderText.text = "Commander: " + commanderName + "\nYear ";
        }
        else
        {
            commanderMournProgress = Mathf.Max(commanderMournProgress - Time.fixedDeltaTime, 0);
            commanderText.text = "<color=#800000>Commander: " + commanderName + " (*)</color>\nYear ";
        }
    }
    public void killCommander()
    {
        commanderMournProgress = commanderMournTime;
        assignNewCommander = true;
    }
    public void assignCommander()
    {
        assignNewCommander = false;
        commanderName = nameGen.nameGenerate();
        commanderLifetime = commanderLifetimeRange.Evaluate(Random.Range(0f, 1f)) * commanderLifetimePlaytimeMult.Evaluate(mManager.gameManager.playtimePercentage);
    }
    public void hurlEarthScenario()
    {
        print("<color=red>Are you insane!?</color>");
        killCommander();
        // kill current Commander
    }
}

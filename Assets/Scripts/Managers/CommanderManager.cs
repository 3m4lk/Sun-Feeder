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

    public TMP_Text betrayalText;
    private Vector3 betrayalTextOGPos;
    public float betrayalTextTimer;
    public float betrayalTextRange;
    private void Awake()
    {
        assignCommander();
        betrayalTextOGPos = betrayalText.transform.position;
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
            commanderText.text = "Commander: " + commanderName + "\nYear " + mManager.gameManager.currentYear + "-ALL";
        }
        else
        {
            commanderMournProgress = Mathf.Max(commanderMournProgress - Time.fixedDeltaTime, 0);
            commanderText.text = "<color=#800000>Commander: " + commanderName + " (*)</color>\nYear " + mManager.gameManager.currentYear + "-ALL";
        }

        betrayalTextTimer = Mathf.Max(betrayalTextTimer - Time.fixedDeltaTime, 0);
        betrayalText.gameObject.SetActive(betrayalTextTimer != 0);
        if (betrayalTextTimer != 0)
        {
            betrayalText.transform.position = betrayalTextOGPos + Vector3.ClampMagnitude(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 1f) * betrayalTextRange;
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
        mManager.newsManager.betray();
        betrayalTextTimer = 3f;
        betrayalText.gameObject.SetActive(true);
        //print("<color=red>Are you insane!?</color>");
        betrayalText.text = new string[] { "<color=red>ARE YOU INSANE?!?</color>", "<color=red>WHAT ARE YOU DOING?!?</color>" }[Random.Range(0, 2)];
        killCommander();
        // kill current Commander
    }
}

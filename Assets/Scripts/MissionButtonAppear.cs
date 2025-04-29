using UnityEngine;
using UnityEngine.UI;

public class MissionButtonAppear : MonoBehaviour
{
    public MissionManager missionManager;

    [Space]
    public int index;

    [Space]
    public AnimationCurve scaleCurve;
    public float scaleTime;
    private float scaleProgress;
    public float direction;
    private Vector2 ogSize;

    public MissionButtonAppear[] clickEnable;
    public MissionButtonAppear[] clickDisable;
    public VerticalLayoutGroup vert;

    [Space]
    public bool isSubmenu;

    /*[Space]
    public AnimationCurve testCurve;
    public float testTime;
    private float testProgress;//*/
    void Update()
    {
        /*scaleProgress = Mathf.Clamp(scaleProgress + direction * Time.deltaTime, 0f, scaleTime);
        transform.localScale = new Vector3(1, scaleCurve.Evaluate(scaleProgress / scaleTime), 1);
        GetComponent<RectTransform>().sizeDelta = new Vector2(ogSize.x, ogSize.y * (scaleProgress / scaleTime));//*/

        //testProgress = Mathf.Repeat(testProgress + direction * Time.deltaTime, testTime);
        //transform.localScale = new Vector3(1, testCurve.Evaluate(testProgress / testTime), 1);
    }
    private void Awake()
    {
        ogSize = GetComponent<RectTransform>().sizeDelta;
        scaleProgress = 0;
        direction = 1f;
        if (isSubmenu)
        {
            direction = -1f;
        }
    }
    private void OnEnable()
    {
        if (isSubmenu)
        {
            scaleProgress = 0;
        }
    }
    public void onClick()
    {
        if (!isSubmenu)
        {
            for (int i = 0; i < clickEnable.Length; i++)
            {
                clickEnable[i].direction = 1f;
            }
            for (int i = 0; i < clickDisable.Length; i++)
            {
                clickDisable[i].direction = -1f;
            }

            missionManager.openTab(index, isSubmenu);
            return;
        }
        print("open thing for mission: " + index);
    }
    public void onEnter()
    {

    }
    public void onExit()
    {

    }
}

using TMPro;
using UnityEngine;

public class MiningTextFloatUp : MonoBehaviour
{
    public int amount;
    private float currentHeight;
    public float riseSpeed;
    public float deathTime;
    public float deathProg;
    public TMP_Text ownText;

    public AnimationCurve scaleCurve;

    public float garbleTime;
    private bool isClear;
    private string textFinal;
    private string randomChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM,./;'\\[]`1234567890-=<>?:\"|{}~!@#$%^&*()_+ ";
    private void Awake()
    {
        Destroy(gameObject, deathTime);
        currentHeight = transform.localPosition.y;

        string output = amount + "";
        if (amount >= 1000000)
        {
            output = (Mathf.Floor(amount / 10000) / 100f) + "M"; // 1.234M
        } // millions
        else if (amount > 1000)
        {
            output = (amount / 1000f) + "k"; // 1.234k
        } // thousands
        //ownText.text = "+" + output;

        textFinal = output;

        string textGarble = "+";
        for (int i = 0; i < textFinal.Length; i++)
        {
            textGarble += randomChars[Random.Range(0, randomChars.Length)];
        }
        ownText.text = textGarble;

        ownText.fontSize = scaleCurve.Evaluate(0) * 16f;
    }
    void FixedUpdate()
    {
        if (!isClear)
        {
            garbleTime = Mathf.Max(garbleTime - Time.fixedDeltaTime, 0);

            if (garbleTime != 0)
            {
                string textGarble = "+";
                for (int i = 0; i < textFinal.Length; i++)
                {
                    textGarble += randomChars[Random.Range(0, randomChars.Length)];
                }
                ownText.text = textGarble;
            }
            else
            {
                ownText.text = textFinal.Replace("173", "<color=#d6c3a0>1</color><color=#670000>7</color><color=#00fc01>3</color>").Replace("31337", "<color=#00FF00>31337</color>").Replace("1337", "<color=#00FF00>1337</color>");
            }
        }
        currentHeight += riseSpeed * Time.fixedDeltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x, currentHeight, transform.localPosition.z);

        deathProg = Mathf.Min(deathProg + Time.fixedDeltaTime, deathTime);

        ownText.fontSize = scaleCurve.Evaluate(deathProg / deathTime) * 16f;
        //transform.localScale = Vector3.one * scaleCurve.Evaluate(deathProg / deathTime);
    }
}
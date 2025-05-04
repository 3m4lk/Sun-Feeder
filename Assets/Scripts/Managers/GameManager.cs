using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MainManager mManager;

    [Tooltip("1f - in real time (used as a funny easter egg only)")]
    //[Range((1f / 60f / 60f / 23.39444444444444444444f / 365.256363004f), 16f)] // realtime -> 16 YPS
    public float gameSpeed = 1f;

    //[Tooltip("0 - Accurate;\n1 - 1 year per minute;\n2 - 0.1 year per second;\n3 - 1 year per second;\n4 - 3 years per second;\n5 - 10 years per second;\n6 - 24 years per second")]
    [Tooltip("0 - Accurate;\n1 - 0.1 year per second;\n2 - 1 year per second;\n3 - 10 years per second;\n4 - 24 years per second;\n5 - 48 years per second")]
    [Range(0, 5)]
    private int speedMode = 3;

    [Tooltip("distance from Sun to Earth; here, a multiplier")]
    public float AstronomicalUnit;

    [Tooltip("in years")]
    public float totalPlaytime;
    public float playtimePercentage;

    public Transform canvasTransform;

    [Space]
    [SerializeField]
    public float money;
    public TMP_Text cashText;

    public int valTest;
    public string formatTest;
    public string format = "0";

    [Space]
    public AnimationCurve cashSmoothCurve;
    public float cashSmoothTime;
    private float cashSmoothProgress;
    private int lastCashPoint;
    private int smoothCash;

    [Space]
    public AnimationCurve cashAddCurve;
    public Gradient cashAddGradient;
    private float cashAddProgress;
    //public float trueProgress;

    [Space]
    public AnimationCurve cashScaleCurve;
    private float cashScaleProgress;
    public Slider calamitySlider;
    public TMP_Text calamityText;

    public int currentYear = 82081;

    public int[] yearsRange = new int[2] { 82021, 87121 };

    public string[] speedSpeeds;
    public TMP_Text speedText;

    [Space]
    [Header("                   Vox Populi <- Vox Aequalis -> Vox Coitionis")]
    [Space]
    public float politicsPerc;
    public AnimationCurve priceMultCurve;
    public AnimationCurve speedMultCurve;
    public float priceMult;
    public float speedMult;
    public TMP_Text polMultsText;

    [SerializeField]
    private bool isLocked;

    public bool[] doneAlerts = new bool[5];

    private float dTap;
    private void Awake()
    {
        //print(QualitySettings.vSyncCount);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        changeSpeed(1);
    }
    private void Update()
    {
        dTap = Mathf.Max(dTap - Time.deltaTime, 0f);
        cashSmoothProgress = Mathf.Min(cashSmoothProgress + Time.deltaTime, cashSmoothTime);
        smoothCash = Mathf.FloorToInt(Mathf.Lerp(lastCashPoint, money, cashSmoothCurve.Evaluate(cashSmoothProgress / cashSmoothTime)));
        cashScaleProgress = Mathf.Max(cashScaleProgress - Time.deltaTime, 0f);

        if (cashAddProgress > 0)
        {
            cashAddProgress = Mathf.Max(cashAddProgress - Time.deltaTime, 0f);
        } // positive
        else if (cashAddProgress < 0)
        {
            cashAddProgress = Mathf.Min(cashAddProgress + Time.deltaTime, 0f);
        } // negative

        cashText.text = formatCash(Mathf.RoundToInt(smoothCash));
        //trueProgress = (cashAddProgress / cashSmoothTime);
        //cashText.color = cashAddGradient.Evaluate(cashAddCurve.Evaluate(0.5f + (trueProgress * 2f)));
        cashText.color = cashAddGradient.Evaluate(cashAddCurve.Evaluate(0.5f + ((cashAddProgress / cashSmoothTime) * 2f)));
        //trueProgress = cashAddCurve.Evaluate(0.5f + (trueProgress * 2f));
        cashText.rectTransform.localScale = Vector3.one * cashScaleCurve.Evaluate(cashScaleProgress / cashSmoothTime);

        formatTest = valTest.ToString(format);

        totalPlaytime = Mathf.Min(totalPlaytime + gameSpeed * Time.deltaTime, 5040f);
        playtimePercentage = totalPlaytime / 5040f;

        calamitySlider.value = 100f - playtimePercentage * 100f;
        calamityText.text = "Sol: " + calamitySlider.value.ToString("0.0") + "%";

        currentYear = Mathf.FloorToInt(Mathf.Lerp(yearsRange[0], yearsRange[1], playtimePercentage));

        solAlerts();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mManager.closeAllWindows();

            if (dTap > 0 && mManager.camManager.currentAnchor != mManager.camManager.Sol)
            {
                mManager.setCameraAnchor(mManager.camManager.Sol);
            }
            else
            {
                dTap = 0.5f;
            }
        }
    }
    void solAlerts()
    {
        float perc = 1f - playtimePercentage;
        int alertTag = default;
        bool proceed = false;
        if (perc <= 0.01f && !doneAlerts[0])
        {
            proceed = true;
            doneAlerts[0] = true;
            alertTag = 99;
        }
        else if (perc <= 0.05f && !doneAlerts[1])
        {
            proceed = true;
            doneAlerts[1] = true;
            alertTag = 95;
        }
        else if (perc <= 0.25f && !doneAlerts[2])
        {
            proceed = true;
            doneAlerts[2] = true;
            alertTag = 75;
        }
        else if (perc <= 0.50f && !doneAlerts[3])
        {
            proceed = true;
            doneAlerts[3] = true;
            alertTag = 50;
        }
        else if (perc <= 0.75f && !doneAlerts[4])
        {
            proceed = true;
            doneAlerts[4] = true;
            alertTag = 25;
        }
        else if (perc == 0f)
        {
            print("Game End");
        }

        if (proceed)
        {
            mManager.popupManager.newPopup("solWarn" + alertTag);
        }
    }
    public void addCash(float amount)
    {
        if (amount == 0) return;

        cashAddProgress = Mathf.Sign(amount) * cashSmoothTime;
        cashScaleProgress = cashSmoothTime;

        lastCashPoint = smoothCash;
        cashSmoothProgress = 0;

        if (amount < 0)
        {
            print("negative");
            money += amount; // don't have to worry about overflowing the other way
        }
        else
        {
            print("positive");
            if (money + amount <= 0)
            {
                money = int.MaxValue;
                print("YOU GOT A SHITTON OF CASH M8!");
            }
            else
            {
                money += amount;
            }
        }
        // add some cosmetic scaling or something
        // also update cash text display
    }
    public string formatCash(int input)
    {
        string[] units = new string[] { "", "k", "M", "B", "T", "Q" };

        int unitIndex = 0;

        // <1000 // any
        // >1000 // kilo
        // >1000000 // milly
        // >1000000000 // billy
        // >1000000000000 // trilly
        // >1000000000000000 // quad

        if (input >= 1000000000000000) unitIndex = 5;
        else if (input >= 1000000000000) unitIndex = 4;
        else if (input >= 1000000000) unitIndex = 3;
        else if (input >= 1000000) unitIndex = 2;
        else if (input >= 1000) unitIndex = 1;

        if (unitIndex == 0)
        {
            return input + " mk";
        }
        else
        {
            if (input == int.MaxValue)
            {
                return "EVERY SINGLE" + " mk";
            }
            else
            {
                float divAmount = 1f;
                switch (unitIndex)
                {
                    case 1:
                        divAmount = 1000f;
                        break;
                    case 2:
                        divAmount = 1000000f;
                        break;
                    case 3:
                        divAmount = 1000000000f;
                        break;
                    case 4:
                        divAmount = 1000000000000f;
                        break;
                    case 5:
                        divAmount = 1000000000000000f;
                        break;
                }

                return (money / (divAmount)).ToString("0.00") + units[unitIndex] + " mk"; // >unintins
            }
        }
    }
    public void changeSpeed(int input)
    {
        if (!isLocked)
        {
            speedMode = input;
            switch (speedMode)
            {
                /*case 0:
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
                    gameSpeed = 24f;
                    break;//*/

                case 0:
                    gameSpeed = (1f / 60f / 60f / 23.39444444444444444444f / 365.256363004f);
                    break;
                case 1:
                    gameSpeed = 0.1f;
                    break;
                case 2:
                    gameSpeed = 1f;
                    break;
                case 3:
                    gameSpeed = 10f;
                    break;
                case 4:
                    gameSpeed = 24f;
                    break;
                case 5:
                    gameSpeed = 48f;
                    break;
            }
            speedText.text = "Speed: " + speedSpeeds[input];
        }
    }
    public void lockSpeed(bool mode)
    {
        isLocked = mode;
    }
    public int getSpeedMode()
    {
        return speedMode;
    }
    public void updatePoliticsStats(float input)
    {
        politicsPerc = (input + 100f) / 200f;
        priceMult = priceMultCurve.Evaluate(politicsPerc);
        speedMult = speedMultCurve.Evaluate(politicsPerc);
        //float priceMultJustForTheText = priceMultCurve.Evaluate(1f - politicsPerc);

        string[] speedWords = new string[] { " Slower", "", " Faster" };
        string[] priceWords = new string[] { " Higher", "", " Lower" };

        int speedInt = 2;
        int priceInt = 1;

        if (speedMult > 1)
        {
            speedInt = 2;
        }
        else if (speedMult < 1)
        {
            speedInt = 0;
        }

        if (priceMult > 1)
        {
            priceInt = 2;
        }
        else if (priceMult < 1)
        {
            priceInt = 0;
        }

        polMultsText.text = "Prices: x" + priceMult.ToString("0.0") + priceWords[priceInt] + "\nSpeed: x" + speedMult.ToString("0.0") + speedWords[speedInt];
    }
}
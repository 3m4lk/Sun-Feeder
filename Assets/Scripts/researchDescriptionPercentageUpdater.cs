using TMPro;
using UnityEngine;

public class researchDescriptionPercentageUpdater : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    public TMP_Text durationPriceText;

    private string lastPerc;
    private int index;
    private void Awake()
    {
        lastPerc = "Terra Incognita";
        index = -1;
    }
    private void Update()
    {
        if (durationPriceText.text.Contains("<resPerc>"))
        {
            index = int.Parse(durationPriceText.text.Split("|")[1].Replace(((char)13).ToString(), ""));
            float perc = (mManager.researchManager.research[index].researchProgress / mManager.researchManager.research[index].duration) * 100f;
            lastPerc = " [" + perc.ToString("0.0") + "%]";
            durationPriceText.text = durationPriceText.text.Replace("|" + index + "|<resPerc>", lastPerc);
        }
        else if (durationPriceText.text.Contains(lastPerc))
        {
            float perc = (mManager.researchManager.research[index].researchProgress / mManager.researchManager.research[index].duration) * 100f;
            string lasterPerc = " [" + perc.ToString("0.0") + "%]";
            durationPriceText.text = durationPriceText.text.Replace(lastPerc, lasterPerc);

            /*if (lastPerc == "[100.0%]")
            {
                index = -1;
                lastPerc = "Terra Incognita";
                return;
            }//*/

            lastPerc = lasterPerc; // what the fuck malk?
        }
    }
}

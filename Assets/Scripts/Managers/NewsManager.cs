using TMPro;
using UnityEngine;

//[ExecuteInEditMode]
public class NewsManager : MonoBehaviour
{
    public MainManager mManager;

    public string[] newsRandom;
    public GameObject newsPanel;

    public RectTransform contentTransform;

    public int newsAmount = 14;

    private float scrollProgress;
    public float scrollSpeed;

    [Space]
    public int betrayalTimes;

    [Header("addon at end on extremism")]
    public string pAddon;
    public string cAddon;

    [Header("exclusive news on extremism")]
    public string[] pExtremist;
    public string[] cExtremist;

    [Header("exclusive news on trying to betray like 5 times")]
    public string betrayalNews;
    private bool hasBetrayed;

    public int extremistNewsChance;

    public string[] warNews;
    public int warNewsChance;

    public string[] coldNews;
    public int coldNewsChance;

    public string warDeclarationNews;

    private bool declaredWar; // if alr did the news
    public bool doWarNews; // set from outside, whether war should be declared

    public bool startExtremismPopuli;
    public bool startExtremismCoitionis;

    private bool didExtremismPopuli;
    private bool didExtremismCoitionis;

    public string extremismStartNews;

    private void Start()
    {
        if (Application.isPlaying)
        {
            //print("READY");
            for (int i = 0; i < newsAmount; i++)
            {
                newNews();
            }
        }
    }
    private void Update()
    {
        scrollProgress -= scrollSpeed;
        for (; scrollProgress <= 0; scrollProgress++)
        {
            //print(contentTransform.anchoredPosition.x);
            contentTransform.anchoredPosition -= new Vector2(1, 0);
            if (contentTransform.anchoredPosition.x <= -contentTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x)
            {
                contentTransform.anchoredPosition = Vector3.zero;
                Destroy(contentTransform.GetChild(0).gameObject);

                if (doWarNews != declaredWar && !declaredWar)
                {
                    declaredWar = true;
                    newNews(warDeclarationNews);
                } // declare war
                else if (startExtremismCoitionis && !didExtremismCoitionis)
                {
                    startExtremismCoitionis = false;
                    didExtremismCoitionis = true;
                    newNews("<color=#0038FF>" + extremismStartNews.Replace("<polSide>", "Vox Coitionis") + "</color>");
                } // declare extremism C
                else if (startExtremismPopuli && !didExtremismPopuli)
                {
                    startExtremismPopuli = false;
                    didExtremismPopuli = true;
                    newNews("<color=#FF0038>" + extremismStartNews.Replace("<polSide>", "Vox Populi") + "</color>");
                } // declare extremism P
                else if (mManager.gameManager.playtimePercentage >= 0.5f && Random.Range(0, coldNewsChance) == 0)
                {
                    newNews("<color=yellow>" + coldNews[Random.Range(0, coldNews.Length)] + "</color>");
                } // Cold News
                else if (betrayalTimes >= 5 && !hasBetrayed)
                {
                    newNews(betrayalNews);
                    hasBetrayed = true;
                } // Betrayal News
                else if (mManager.politicsManager.politicalViews == 100f && Random.Range(0, warNewsChance) == 0)
                {
                    newNews(warNews[Random.Range(0, warNews.Length)]);
                } // War News (for now for 100%-ing it only) // mManager.politicsManager.warTime > 0 && Random.Range(0, warNewsChance) == 0
                else if (Mathf.Abs(mManager.politicsManager.politicalViews) >= mManager.politicsManager.extremismThreshold && Random.Range(0, extremistNewsChance) == 0)
                {
                    string newsToAdd = default;
                    switch (mManager.politicsManager.currentAlignment)
                    {
                        case "Vox Coitionis":
                            // add from poll
                            if (Random.Range(0, extremistNewsChance) == 1)
                            {
                                newsToAdd = cExtremist[Random.Range(0, cExtremist.Length)];
                            }
                            else
                            {
                                newsToAdd = newsRandom[Random.Range(0, newsRandom.Length)];
                            }

                            if (Random.Range(1f, 100f) <= Mathf.Abs(mManager.politicsManager.politicalViews))
                            {
                                newsToAdd += " " + cAddon;
                            }

                            // add addon to end

                            break;
                        case "Vox Populi":

                            if (Random.Range(0, extremistNewsChance) == 1)
                            {
                                newsToAdd = pExtremist[Random.Range(0, pExtremist.Length)];
                            }
                            else
                            {
                                newsToAdd = newsRandom[Random.Range(0, newsRandom.Length)];
                            }

                            if (Random.Range(1f, 100f) <= Mathf.Abs(mManager.politicsManager.politicalViews))
                            {
                                newsToAdd += " " + pAddon;
                            }
                            // add from poll

                            // add addon to end

                            break;
                    }
                    newNews(newsToAdd);
                } // Extremist news
                else
                {
                    newNews();
                } // Regular News
            }
        }
    }
    void newNews(string input = default)
    {
        // check behaviors
        string news = default;
        news = newsRandom[Random.Range(0, newsRandom.Length)];

        if (input != default)
        {
            news = input;
        }

        RectTransform newPanel = Instantiate(newsPanel).GetComponent<RectTransform>();

        newPanel.GetComponent<TMP_Text>().text = " " + news + " // ";

        newPanel.SetParent(contentTransform, false);
    }
    public void betray()
    {
        betrayalTimes++;
    }
}

using TMPro;
using UnityEngine;

[System.Serializable]
public class nLst
{
    public string[] news;
}
//[ExecuteInEditMode]
public class NewsManager : MonoBehaviour
{
    public MainManager mManager;

    //private string[] newsRandom;
    private nLst[] randomNews;
    private int[] randomNewsShuffle;
    private int currentNewsCollection;
    public GameObject newsPanel;

    private string[] jokeNews;
    private int lastJoke;

    private string[] drunkNews;
    private int lastDrunk;

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
    private string[] pExtremist;
    private string[] cExtremist;

    [Header("exclusive news on trying to betray like 5 times")]
    public string betrayalNews;
    private bool hasBetrayed;

    public int extremistNewsChance;

    private string[] warNews;
    public int warNewsChance;

    private string[] coldNews;
    public int coldNewsChance;

    public string warDeclarationNews;

    private bool declaredWar; // if alr did the news
    public bool doWarNews; // set from outside, whether war should be declared

    public bool startExtremismPopuli;
    public bool startExtremismCoitionis;

    private bool didExtremismPopuli;
    private bool didExtremismCoitionis;

    public string extremismStartNews;

    private float lastNCWidth;

    private void Start()
    {
        //newsRandom = Resources.Load<TextAsset>("News/newsRandom").text.ToString().Replace(((char)13).ToString(), "").Split("\n");

        randomNews = new nLst[10];
        randomNewsShuffle = new int[randomNews.Length];
        for (int i = 0; i < randomNews.Length; i++)
        {
            randomNews[i] = new nLst();
            randomNews[i].news = Resources.Load<TextAsset>("News/Common/news" + i).text.ToString().Replace(((char)13).ToString(), "").Split("\n");
            randomNewsShuffle[i] = i;
        }
        randomNewsShuffle = shuffle(randomNewsShuffle);
        currentNewsCollection = 0;

        jokeNews = Resources.Load<TextAsset>("News/jokes").text.ToString().Replace(((char)13).ToString(), "").Split("\n");
        pExtremist = Resources.Load<TextAsset>("News/extremistP").text.ToString().Replace(((char)13).ToString(), "").Split("\n");
        cExtremist = Resources.Load<TextAsset>("News/extremistC").text.ToString().Replace(((char)13).ToString(), "").Split("\n");
        warNews = Resources.Load<TextAsset>("News/war").text.ToString().Replace(((char)13).ToString(), "").Split("\n");
        coldNews = Resources.Load<TextAsset>("News/cold").text.ToString().Replace(((char)13).ToString(), "").Split("\n");
        drunkNews = Resources.Load<TextAsset>("News/drunk").text.ToString().Replace(((char)13).ToString(), "").Split("\n");

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
        if (contentTransform.sizeDelta.x != lastNCWidth)
        {
            lastNCWidth = contentTransform.sizeDelta.x;
            print("WIDTH CHANGED");

            /*if (lastNCWidth < contentTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x + 640f)
            {

            } // add new news headline to the end (eh, not for now)//*/
        }

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
                                newsToAdd = randomNews[currentNewsCollection].news[Random.Range(0, randomNews[currentNewsCollection].news.Length)];
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
                                newsToAdd = randomNews[currentNewsCollection].news[Random.Range(0, randomNews[currentNewsCollection].news.Length)];
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
        //news = newsRandom[Random.Range(0, newsRandom.Length)];
        news = randomNews[currentNewsCollection].news[Random.Range(0, randomNews[currentNewsCollection].news.Length)];
        if (Random.Range(0f, 100f) < 17.5f)
        {
            if (Random.Range(0, 3) != 1)
            {
                int rand = Random.Range(0, jokeNews.Length);
                if (rand == lastJoke) { rand = (int)Mathf.Repeat(rand + 1, jokeNews.Length); }
                news = jokeNews[rand];
                lastJoke = rand;
            } // joke news
            else
            {
                int rand = Random.Range(0, drunkNews.Length);
                if (rand == lastDrunk) { rand = (int)Mathf.Repeat(rand + 1, drunkNews.Length); }
                news = drunkNews[rand];
                lastDrunk = rand;
            } // drunk news
        } // change it to a chance that becomes lower the further in the game you are, starting at about 17.5% and going down to about 2%
        currentNewsCollection = (int)Mathf.Repeat(currentNewsCollection + 1, randomNewsShuffle.Length);
        if (currentNewsCollection == 0) { randomNewsShuffle = shuffle(randomNewsShuffle); }

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
    int[] shuffle(int[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            int temp = input[i], r = Random.Range(0, input.Length);
            input[i] = input[r];
            input[r] = temp;
        }
        return input;
    }
}

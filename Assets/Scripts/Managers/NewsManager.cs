using TMPro;
using UnityEngine;

//[ExecuteInEditMode]
public class NewsManager : MonoBehaviour
{
    public string[] newsRandom;
    public GameObject newsPanel;

    public RectTransform contentTransform;

    public int newsAmount = 14;

    private float scrollProgress;
    public float scrollSpeed;

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
                newNews();
            }
        }
    }
    void newNews()
    {
        // check behaviors
        string news = default;
        news = newsRandom[Random.Range(0, newsRandom.Length)];

        RectTransform newPanel = Instantiate(newsPanel).GetComponent<RectTransform>();

        newPanel.GetComponent<TMP_Text>().text = " " + news + " // ";

        newPanel.parent = contentTransform;
    }
    public void editSetup()
    {

    }
}

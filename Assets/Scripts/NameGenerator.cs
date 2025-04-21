using UnityEngine;

[System.Serializable]
public class gen
{
    [Tooltip("to make dev easier")]
    public string name;

    [Header("Names")]

    public string[] n1stSyllables;
    public string[] nMidSyllables;
    public string[] nEndSyllables;
    public string[] nOneSyllableNames;

    [Header("Surnames")]

    public string[] sn1stSyllables;
    public string[] snMidSyllables;
    public string[] snEndSyllables;
    public string[] snOneSyllableSurnames;

    public int maxNameSyllables;
    public int maxSurnameSyllables;

    [Tooltip("Percentage")]
    public float secondNameChance;
    [Tooltip("Percentage")]
    public float thirdNameChance;
}
public class NameGenerator : MonoBehaviour
{
    public gen[] generators;
    public int testGen;

    public string output;
    public void nameGenerate(int generator)
    {
        output = "";
        gen curr = generators[generator];

        // name

        int nameSyllAmount = Random.Range(1, curr.maxNameSyllables + 1);
        int surnameSyllAmount = Random.Range(1, curr.maxSurnameSyllables + 1);

        float nameChance = Random.Range(0f, 100f);

        if (nameSyllAmount > 1)
        {
            for (int i = 0; i < nameSyllAmount; i++)
            {
                if (i == 0)
                {
                    output += curr.n1stSyllables[Random.Range(0, curr.n1stSyllables.Length)];
                } // start
                else if (i == nameSyllAmount - 1)
                {
                    output += curr.nEndSyllables[Random.Range(0, curr.nEndSyllables.Length)];
                } // end
                else
                {
                    output += curr.nMidSyllables[Random.Range(0, curr.nMidSyllables.Length)];
                } // middle
            }
        }
        else
        {
            output += curr.nOneSyllableNames[Random.Range(0, curr.nOneSyllableNames.Length)];
        }
        output += " ";

        if (nameChance < curr.secondNameChance + curr.thirdNameChance)
        {
            // i don't care for the ugliness of this code, i'm giving myself only 1 week to finish this project so ion waste much time on the Current Thing so ion wanna much of my time to write fancy functions lmaooooooooooo
            if (nameSyllAmount > 1)
            {
                for (int i = 0; i < nameSyllAmount; i++)
                {
                    if (i == 0)
                    {
                        output += curr.n1stSyllables[Random.Range(0, curr.n1stSyllables.Length)];
                    } // start
                    else if (i == nameSyllAmount - 1)
                    {
                        output += curr.nEndSyllables[Random.Range(0, curr.nEndSyllables.Length)];
                    } // end
                    else
                    {
                        output += curr.nMidSyllables[Random.Range(0, curr.nMidSyllables.Length)];
                    } // middle
                }
            }
            else
            {
                output += curr.nOneSyllableNames[Random.Range(0, curr.nOneSyllableNames.Length)];
            }
            output += " ";

            if (nameChance < curr.thirdNameChance)
            {
                // stay mad Koki, lmfao
                if (nameSyllAmount > 1)
                {
                    for (int i = 0; i < nameSyllAmount; i++)
                    {
                        if (i == 0)
                        {
                            output += curr.n1stSyllables[Random.Range(0, curr.n1stSyllables.Length)];
                        } // start
                        else if (i == nameSyllAmount - 1)
                        {
                            output += curr.nEndSyllables[Random.Range(0, curr.nEndSyllables.Length)];
                        } // end
                        else
                        {
                            output += curr.nMidSyllables[Random.Range(0, curr.nMidSyllables.Length)];
                        } // middle
                    }
                }
                else
                {
                    output += curr.nOneSyllableNames[Random.Range(0, curr.nOneSyllableNames.Length)];
                }
                output += " ";

            } // third name
        } // second name

        if (surnameSyllAmount > 1)
        {
            for (int i = 0; i < surnameSyllAmount; i++)
            {
                if (i == 0)
                {
                    output += curr.sn1stSyllables[Random.Range(0, curr.sn1stSyllables.Length)];
                } // start
                else if (i == surnameSyllAmount - 1)
                {
                    output += curr.snEndSyllables[Random.Range(0, curr.snEndSyllables.Length)];
                } // end
                else
                {
                    output += curr.snMidSyllables[Random.Range(0, curr.snMidSyllables.Length)];
                } // middle
            }
        }
        else
        {
            output += curr.snOneSyllableSurnames[Random.Range(0, curr.snOneSyllableSurnames.Length)];
        }
    }
}

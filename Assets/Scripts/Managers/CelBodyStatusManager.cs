using UnityEngine;

[System.Serializable]
public class bClust
{
    public string name;
    public string displayName;
    public bool isSatellite;

    [Space]
    public string satelliteName;
    public bool displayOwnNameWhenLast;

    [Tooltip("Last body's name of a cluster will be displayed if no other satellites are present")]
    public string lastRemainingName;

    [Space]
    public int amount;

    [Space]
    public Transform[] members;
}
public class CelBodyStatusManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    public bClust[] bodyCluster;
    public MissionButtonAppear[] buttons;

    public float amountDisplaySize;
    private void Awake()
    {
        for (int i = 0; i < bodyCluster.Length; i++)
        {
            buttons[i].cbMenuUpdateButtonText(i, bodyCluster[i].amount);
        }
    }
    public void openOptions(int input)
    {
        for (int i = 0; i < bodyCluster.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        } // disable all buttons

        // enable all buttons of cluster with alive status; go through each body in orbitManager and check if it's the desired index & if it's alive

    } // when selected body type for mission (planet, dwarf etc.), pull up all the buttons in the cluster
    public void clusterAmountUpdate(int index, int amount)
    {
        if (index < bodyCluster.Length)
        {
            print("add amount to cluster[index]");
            bodyCluster[index].amount = Mathf.Max(bodyCluster[index].amount + amount, 0);
        }
        // after that, update button text (by calling the function within the button itself, the only data this function needs is its own index, and amount)
    }
    public Transform getBody(string clusterName, string bodyName = default, int inputIndex = -1)
    {
        int clustIndex = -1;
        for (int i = 0; i < bodyCluster.Length; i++)
        {
            if (bodyCluster[i].name == clusterName)
            {
                clustIndex = i;
                break;
            }
        }

        if (inputIndex != -1)
        {
            return bodyCluster[clustIndex].members[inputIndex];
        } // get chosen body by index
        else if (bodyName != default)
        {
            for (int i = 0; i < bodyCluster[clustIndex].members.Length; i++)
            {
                if (bodyCluster[clustIndex].members[i].name == bodyName)
                {
                    return bodyCluster[clustIndex].members[i];
                }
            }
        } // get chosen body by name
        else
        {
            return bodyCluster[clustIndex].members[Random.Range(0, bodyCluster[clustIndex].members.Length)];
        } // get random one
        return null;
    }
}

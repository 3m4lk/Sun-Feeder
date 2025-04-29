using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MainManager mManager;

    [Space]
    [Tooltip("0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol")]
    public GameObject[] operationTabs;

    [Space]
    [Tooltip("Solar Engine")]
    public GameObject operationSol;
    [Tooltip("0 - asteroHurl;\n1 - verne;\n2 - 300ton")]
    public GameObject[] operationsPlanets;
    [Tooltip("Asterosling")]
    public GameObject operationKuiperBelt;
    [Tooltip("0 - gasVent;\n1 - arcCannon")]
    public GameObject[] operationsGasGiants;
    [Tooltip("0 - reflSolSail;\n1 - 3ton;\n2 - mining")]
    public GameObject[] operationsMinorBodies;

    [Tooltip("0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol")]
    public void unlockOperation(int type, int index) // 0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol
    {
        operationTabs[type].SetActive(true);

        switch (type)
        {
            case 0:
                operationsPlanets[index].SetActive(true);
                break; // Planets
            case 1:
                operationKuiperBelt.SetActive(true);
                break; // Kuiper Belt
            case 2:
                operationsGasGiants[index].SetActive(true);
                break; // Gas Giants
            case 3:
                operationsMinorBodies[index].SetActive(true);
                break; // Minor Bodies
            case 4:
                operationSol.SetActive(true);
                break; // Sol
        }
    } // 0 - Planets;\n1-  Kuiper Belt;\n2 - Gas Giants\n3 - Minor Bodies;\n4 - Sol
    public void openTab(int index, bool isSubmenu)
    {
        /*for (int a = 0; a < operationsPlanets.Length; a++)
        {
            operationsPlanets[a].GetComponent<MissionButtonAppear>().direction = -1f;
        }
        operationKuiperBelt.GetComponent<MissionButtonAppear>().direction = -1f;
        for (int a = 0; a < operationsGasGiants.Length; a++)
        {
            operationsGasGiants[a].GetComponent<MissionButtonAppear>().direction = -1f;
        }
        for (int a = 0; a < operationsMinorBodies.Length; a++)
        {
            operationsMinorBodies[a].GetComponent<MissionButtonAppear>().direction = -1f;
        }
        operationSol.GetComponent<MissionButtonAppear>().direction = -1f;

        switch (index)
        {
            case 0:
                for (int a = 0; a < operationsPlanets.Length; a++)
                {
                    operationsPlanets[a].GetComponent<MissionButtonAppear>().direction = 1f;
                }
                break; // Planets
            case 1:
                operationKuiperBelt.GetComponent<MissionButtonAppear>().direction = 1f;
                break; // Kuiper Belt
            case 2:
                for (int a = 0; a < operationsGasGiants.Length; a++)
                {
                    operationsGasGiants[a].GetComponent<MissionButtonAppear>().direction = 1f;
                }
                break; // Gas Giants
            case 3:
                for (int a = 0; a < operationsMinorBodies.Length; a++)
                {
                    operationsMinorBodies[a].GetComponent<MissionButtonAppear>().direction = 1f;
                }
                break; // Minor Bodies
            case 4:
                operationSol.GetComponent<MissionButtonAppear>().direction = 1f;
                break; // Sol
        }//*/
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NameGenerator))]
public class NameGeneratorButton : Editor
{
    public override void OnInspectorGUI()
    {
        NameGenerator nameGen = (NameGenerator)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Test Generate Name"))
        {
            nameGen.nameGenerate(nameGen.testGen);
        }
    }
}
[CustomEditor(typeof(SolSystemSetup))]
public class SolSystemSetupButton : Editor
{
    public override void OnInspectorGUI()
    {
        SolSystemSetup SolSetup = (SolSystemSetup)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Setup Sol System"))
        {
            SolSetup.setup();
        }
    }
}
[CustomEditor(typeof(OrbitManager))]
public class OrbitManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        OrbitManager orbitManager = (OrbitManager)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Recalculate Celestial Body Positions"))
        {
            orbitManager.recalcPositions();
        }
        if (GUILayout.Button("Randomize Celestial Body Positions"))
        {
            orbitManager.randomPositions();
        }
        if (GUILayout.Button("Reset Celestial Body Positions"))
        {
            orbitManager.resetPositions();
        }
        if (GUILayout.Button("Set All Celestial Bodies To Alive"))
        {
            for (int i = 0; i < orbitManager.bodies.Length; i++)
            {
                orbitManager.bodies[i].isAlive = true;
            }
        }
    }
}
[CustomEditor(typeof(quickVisSetupTool))]
public class quickVisSetupToolButton : Editor
{
    public override void OnInspectorGUI()
    {
        quickVisSetupTool qVT = (quickVisSetupTool)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Setup Celestial Body Visuals"))
        {
            qVT.setupVisuals();
        }
        if (GUILayout.Button("Find Farthest Body"))
        {
            qVT.findFarthest();
        }
        if (GUILayout.Button("Find Closest Body"))
        {
            qVT.findClosest();
        }
        if (GUILayout.Button("Find All Bodies Over Distance"))
        {
            qVT.amountOfAllUnderDistance();
        }
    }
}
[CustomEditor(typeof(CameraManager))]
public class cameraManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        CameraManager cManager = (CameraManager)target;
        base.OnInspectorGUI();
        if (cManager.testerage)
        {
            if (GUILayout.Button("Boo!"))
            {
                Debug.Log("yada yada, ze~");
            }
        }
        if (GUILayout.Button("Change Anchor"))
        {
            cManager.changeAnchor(cManager.newAnchor);
        }
    }
}
[CustomEditor(typeof(ResearchManager))]
public class researchManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        ResearchManager rManager = (ResearchManager)target;
        base.OnInspectorGUI();
        if (rManager.devMode)
        {
            if (GUILayout.Button("Set All Durations To 1"))
            {
                for (int i = 0; i < rManager.research.Length; i++)
                {
                    rManager.research[i].duration = 1;
                }
            }
            if (GUILayout.Button("Set All Curves To First One's"))
            {
                for (int i = 0; i < rManager.research.Length; i++)
                {
                    rManager.research[i].priceProgressionCurve = rManager.research[0].priceProgressionCurve;
                    rManager.research[i].durationProgressionCurve = rManager.research[0].durationProgressionCurve;
                }
            }
        }
    }
}
[CustomEditor(typeof(MinigameManager))]
public class MinigameManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        MinigameManager mgManager = (MinigameManager)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Get All Transforms' Positions Into The Array"))
        {
            mgManager.acDefPointVectors = new Vector3[mgManager.positionsToAutograb.Length];
            for (int i = 0; i < mgManager.positionsToAutograb.Length; i++)
            {
                Vector3 thePos = mgManager.positionsToAutograb[i].localPosition;
                thePos.y = thePos.z;
                thePos.z = 0;
                mgManager.acDefPointVectors[i] = thePos;
            }
        }
    }
}
[CustomEditor(typeof(PopupManager))]
public class PopupManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        PopupManager ppManager = (PopupManager)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Test Popup"))
        {
            ppManager.devTestPopup();
        }
    }
}
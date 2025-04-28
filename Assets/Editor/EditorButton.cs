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
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

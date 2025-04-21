using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NameGenerator))]
public class SaveEditorButton : Editor
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

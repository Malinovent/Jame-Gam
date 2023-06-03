using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Node node = (Node)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Update Color"))
        {
            node.UpdateSpriteColor();
        }
    }
}

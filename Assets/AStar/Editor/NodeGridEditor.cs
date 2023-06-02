using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeGrid))]
public class NodeGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NodeGrid nodeGrid = (NodeGrid)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Nodes"))
        {
            nodeGrid.CreateGrid();
        }

        if (GUILayout.Button("Update all node colors"))
        {
            nodeGrid.UpdateAllNodeColors();
        }

        if (GUILayout.Button("Toggle all node sprite renderers"))
        {
            nodeGrid.ToggleAllNodeSpriteRenderers();
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]
public class TileSlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Serve para atualizar o estado do objeto para vc trabalhar com valores atualizados
        serializedObject.Update();
        base.OnInspectorGUI();

        float buttonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;

        // Row 1
        // --------------------------------
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Field", GUILayout.Width(buttonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileField;

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Road", GUILayout.Width(buttonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileRoad;

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        // Row 2
        // --------------------------------
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Sideway", GUILayout.Width(buttonWidth * 2)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileSidway;

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        GUILayout.EndHorizontal();

        // Row 3
        // --------------------------------
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Inner Corner", GUILayout.Width(buttonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileInnerCorner;

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Outer Corner", GUILayout.Width(buttonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileOuterCorner;

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();
    }
}

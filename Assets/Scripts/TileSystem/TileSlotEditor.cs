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

        if (GUILayout.Button("My First Button"))
        {
            foreach (var obj in targets)
            {
                ((TileSlot)obj).ButtonCheck();
            }
        }
    }
}

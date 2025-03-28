using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemGlobalData))]
public class ButtonToUpdateList : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemGlobalData itemGlobalData = (ItemGlobalData)target;

        if(GUILayout.Button("Update ID item in list"))
        {
            itemGlobalData.Create();
        }
    }
}

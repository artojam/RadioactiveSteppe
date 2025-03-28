using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

[CustomEditor(typeof(ArtifactItem))]
public class ArtifactEditor : Editor
{
    private List<string> PlayerVars = new List<string>();

    private void CollFields(System.Type _type, string _prefix)
    {
        FieldInfo[] fields = _type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            if(!field.FieldType.IsPrimitive && field.FieldType != typeof(string))
            {
                CollFields(field.FieldType, _prefix + field.Name + ".");
            }
            else PlayerVars.Add(_prefix + field.Name);
        }
    }

    private void UpdatePlayerVar()
    {
        PlayerVars.Clear();
        CollFields(typeof(Player), "");
    }

    public override void OnInspectorGUI()
    {
        // �������� ������ �� ������� ������ (Artifact)
        ArtifactItem artifact = (ArtifactItem)target;

        UpdatePlayerVar();

        // ���������� ����������� ���� ����������
        DrawDefaultInspector();

        // ������ ��� ���������� ������ �������
        if (GUILayout.Button("Add Effect"))
        {
            // ������� ����� ������ � ��������� ��� � ������ ��������
            EffectArtifact newEffect = new EffectArtifact();

            // ������������� ������ �������� �� ������ playerVariables ��� ��� ���������� �� ���������
            if (PlayerVars.Count > 0)
            {
                newEffect.VarName = PlayerVars[0];
            }

            artifact.Effects.Add(newEffect);
        }

        // ���������� ��� ������� � ���������� �� � ����������
        for (int i = 0; i < artifact.Effects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // ���������� ������ ��� ������ ����������
            int selectedIndex = PlayerVars.IndexOf(artifact.Effects[i].VarName);
            if (selectedIndex < 0) selectedIndex = 0;

            selectedIndex = EditorGUILayout.Popup(selectedIndex, PlayerVars.ToArray());

            artifact.Effects[i].VarName = PlayerVars[selectedIndex];

            // ���� ��� ����� �������� (��������� ����)
            artifact.Effects[i].Value = int.Parse(EditorGUILayout.TextField(artifact.Effects[i].Value.ToString()));

            // ������ ��� �������� �������
            if (GUILayout.Button("Remove"))
            {
                artifact.Effects.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        // ��������� ���������, ���� ��� ���� �������
        if (GUI.changed)
        {
            EditorUtility.SetDirty(artifact);
        }
    }
}

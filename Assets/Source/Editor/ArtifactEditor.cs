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
        // Получаем ссылку на целевой объект (Artifact)
        ArtifactItem artifact = (ArtifactItem)target;

        UpdatePlayerVar();

        // Отображаем стандартные поля инспектора
        DrawDefaultInspector();

        // Кнопка для добавления нового эффекта
        if (GUILayout.Button("Add Effect"))
        {
            // Создаем новый эффект и добавляем его в список эффектов
            EffectArtifact newEffect = new EffectArtifact();

            // Устанавливаем первое значение из списка playerVariables как имя переменной по умолчанию
            if (PlayerVars.Count > 0)
            {
                newEffect.VarName = PlayerVars[0];
            }

            artifact.Effects.Add(newEffect);
        }

        // Перебираем все эффекты и отображаем их в инспекторе
        for (int i = 0; i < artifact.Effects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Выпадающий список для выбора переменной
            int selectedIndex = PlayerVars.IndexOf(artifact.Effects[i].VarName);
            if (selectedIndex < 0) selectedIndex = 0;

            selectedIndex = EditorGUILayout.Popup(selectedIndex, PlayerVars.ToArray());

            artifact.Effects[i].VarName = PlayerVars[selectedIndex];

            // Поле для ввода значения (текстовое поле)
            artifact.Effects[i].Value = int.Parse(EditorGUILayout.TextField(artifact.Effects[i].Value.ToString()));

            // Кнопка для удаления эффекта
            if (GUILayout.Button("Remove"))
            {
                artifact.Effects.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        // Сохраняем изменения, если они были сделаны
        if (GUI.changed)
        {
            EditorUtility.SetDirty(artifact);
        }
    }
}

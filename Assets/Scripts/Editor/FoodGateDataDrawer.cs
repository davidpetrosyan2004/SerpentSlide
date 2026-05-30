using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FoodGateData))]
[CanEditMultipleObjects]
public class FoodGateDataDrawer : Editor
{
    private FoodGateData Data => (FoodGateData)target;
    private FoodGateData.CellType selectedType;
    private FoodGateData.ColorType selectedColor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        DrawSizeFields();
        EditorGUILayout.Space();

        DrawButtons();
        EditorGUILayout.Space();

        if (IsBoardValid())
        {
            DrawBoard();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(Data);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSizeFields()
    {
        SerializedProperty columnsProp = serializedObject.FindProperty("columns");
        SerializedProperty rowsProp = serializedObject.FindProperty("rows");

        EditorGUILayout.PropertyField(columnsProp);
        EditorGUILayout.PropertyField(rowsProp);

        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();

            Undo.RecordObject(Data, "Resize Grid");
            Data.CreateNewBoard();
            EditorUtility.SetDirty(Data);
        }

        selectedType = (FoodGateData.CellType)EditorGUILayout.EnumPopup(
            "Cell Type",
            selectedType
        );
        selectedColor = (FoodGateData.ColorType)EditorGUILayout.EnumPopup(
            "Cell Color",
            selectedColor
        );
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Clear Board"))
        {
            Data.Clear();
        }
    }

    private bool IsBoardValid()
    {
        return Data.board != null &&
               Data.board.Length == Data.rows &&
               Data.columns > 0 &&
               Data.rows > 0;
    }

    private void DrawBoard()
    {
        float size = 25f;

        for (int col = 0; col < Data.columns; col++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int row = 0; row < Data.rows; row++)
            {
                var cell = Data.board[row].column[col];

                Color old = GUI.color;
                GUI.color = GetColor(cell.colorType);

                if (GUILayout.Button("", GUILayout.Width(size), GUILayout.Height(size)))
                {
                    Undo.RecordObject(Data, "Paint Cell");

                    cell.type = selectedType;
                    cell.colorType = selectedColor;

                    EditorUtility.SetDirty(Data);
                }

                GUI.color = old;

                GUI.Label(
                    GUILayoutUtility.GetLastRect(),
                    cell.type.ToString(),
                    new GUIStyle
                    {
                        alignment = TextAnchor.MiddleCenter,
                        normal = { textColor = Color.white }
                    }
                );
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private Color GetColor(FoodGateData.ColorType type)
    {
        switch (type)
        {
            case FoodGateData.ColorType.None: return Color.gray;
            case FoodGateData.ColorType.Purple: return Color.purple;
            case FoodGateData.ColorType.Blue: return Color.blue;
            case FoodGateData.ColorType.Green: return Color.green;
            case FoodGateData.ColorType.Yellow: return Color.yellow;
            case FoodGateData.ColorType.Orange: return Color.Lerp(Color.red, Color.yellow, 0.5f);
            case FoodGateData.ColorType.Pink: return Color.pink;
            default: return Color.gray;
        }
    }
}
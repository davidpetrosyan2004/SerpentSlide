using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SnakeData))]
[CanEditMultipleObjects]
public class SnakeDataDrawer : Editor
{
    private SnakeData Data => (SnakeData)target;

    private SnakeData.CellType selectedType;
    private SnakeData.ColorType selectedColor;
    private int selectedIndexBody;
    private int selectedRotation;

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

        selectedType = (SnakeData.CellType)EditorGUILayout.EnumPopup(
            "BodyPart",
            selectedType
        );

        selectedColor = (SnakeData.ColorType)EditorGUILayout.EnumPopup(
            "HeadColor",
            selectedColor
        );

        selectedIndexBody = EditorGUILayout.IntPopup(
            "BodyIndex",
            selectedIndexBody,
            new string[] { "0", "1", "2", "3", "4", "5", "6" },
            new int[] { 0, 1, 2, 3, 4, 5, 6 }
        );

        selectedRotation = EditorGUILayout.IntPopup(
            "Rotation",
            selectedRotation,
            new[] { "0", "60", "-60", "-120", "120", "180" },
            new[] { 0, 60, -60, -120, 120, 180 }
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
                GUI.color = GetColor(cell.color);

                if (GUILayout.Button("", GUILayout.Width(size), GUILayout.Height(size)))
                {
                    Undo.RecordObject(Data, "Paint Cell");

                    cell.type = selectedType;
                    cell.indexBody = selectedIndexBody;
                    cell.color = selectedColor;
                    cell.rotation = selectedRotation;

                    EditorUtility.SetDirty(Data);
                }

                GUI.color = old;

                GUI.Label(
                    GUILayoutUtility.GetLastRect(),
                    cell.indexBody.ToString(),
                    new GUIStyle
                    {
                        alignment = TextAnchor.MiddleCenter,
                        normal = { textColor = Color.black }
                    }
                );
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private Color GetColor(SnakeData.ColorType type)
    {
        switch (type)
        {
            case SnakeData.ColorType.None: return Color.gray;
            case SnakeData.ColorType.Red: return Color.red;
            case SnakeData.ColorType.Blue: return Color.blue;
            case SnakeData.ColorType.Green: return Color.green;
            case SnakeData.ColorType.Yellow: return Color.yellow;
            case SnakeData.ColorType.Orange: return Color.Lerp(Color.red, Color.yellow, 0.5f);
            default: return Color.gray;
        }
    }
}
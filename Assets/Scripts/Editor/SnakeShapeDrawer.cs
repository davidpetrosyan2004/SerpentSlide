//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(SnakeData))]
//[CanEditMultipleObjects]
//public class SnakeDataDrawer : Editor
//{
//    private SnakeData Data => (SnakeData)target;
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUI.BeginChangeCheck();

//        DrawSizeFields();
//        EditorGUILayout.Space();

//        DrawButtons();
//        EditorGUILayout.Space();

//        if (IsBoardValid())
//        {
//            DrawBoard();
//        }

//        if (EditorGUI.EndChangeCheck())
//        {
//            EditorUtility.SetDirty(Data);
//        }

//        serializedObject.ApplyModifiedProperties();
//    }

//    private void DrawSizeFields()
//    {
//        SerializedProperty columnsProp = serializedObject.FindProperty("columns");
//        SerializedProperty rowsProp = serializedObject.FindProperty("rows");
//        SerializedProperty lengthProp = serializedObject.FindProperty("length");
//        SerializedProperty spawnPosProp = serializedObject.FindProperty("spawnPos");

//        EditorGUILayout.PropertyField(columnsProp);
//        EditorGUILayout.PropertyField(rowsProp);
//        EditorGUILayout.PropertyField(lengthProp);
//        EditorGUILayout.PropertyField(spawnPosProp);

//        if (serializedObject.hasModifiedProperties)
//        {
//            serializedObject.ApplyModifiedProperties();

//            Undo.RecordObject(Data, "Resize Grid");
//            Data.CreateNewBoard();
//            EditorUtility.SetDirty(Data);
//        }
//    }

//    private void DrawButtons()
//    {
//        if (GUILayout.Button("Clear Board"))
//        {
//            Data.Clear();
//        }
//    }

//    private bool IsBoardValid()
//    {
//        return Data.board != null &&
//               Data.board.Length == Data.rows &&
//               Data.columns > 0 &&
//               Data.rows > 0;
//    }

//    private void DrawBoard()
//    {
//        float size = 25f;

//        for (int row = 0; row < Data.rows; row++)
//        {
//            EditorGUILayout.BeginHorizontal();

//            for (int col = 0; col < Data.columns; col++)
//            {
//                bool cell = Data.board[row].column[col];

//                Color oldColor = GUI.backgroundColor;

//                GUI.backgroundColor = cell ? Color.white : Color.gray;

//                if (GUILayout.Button("", GUILayout.Width(size), GUILayout.Height(size)))
//                {
//                    Data.board[row].column[col] = !Data.board[row].column[col];
//                }

//                GUI.backgroundColor = oldColor;
//            }

//            EditorGUILayout.EndHorizontal();
//        }
//    }
//}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SnakeData))]
[CanEditMultipleObjects]
public class SnakeDataDrawer : Editor
{
    private SnakeData Data => (SnakeData)target;
    private SnakeData.CellType selectedType;
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
                GUI.color = GetColor(cell.type);

                if (GUILayout.Button("", GUILayout.Width(size), GUILayout.Height(size)))
                {
                    Data.board[row].column[col].type = selectedType;
                }

                GUI.color = old;

                GUI.Label(
                    GUILayoutUtility.GetLastRect(),
                    cell.type.ToString(),
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

    private Color GetColor(SnakeData.CellType type)
    {
        switch (type)
        {
            case SnakeData.CellType.E: return Color.gray;
            case SnakeData.CellType.H: return Color.red;
            case SnakeData.CellType.B: return Color.white;
            case SnakeData.CellType.T: return Color.pink;
            default: return Color.gray;
        }
    }
}
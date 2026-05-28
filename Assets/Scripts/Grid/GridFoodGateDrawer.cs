using UnityEngine;


public class GridFoodGateDrawer : MonoBehaviour
{
    [SerializeField] private GridTiles gridTiles;
    [SerializeField] private Vector3Int offset;

    [SerializeField] private FoodGateData foodGateData;
    [SerializeField] private GameObject gatePrefab;
    [SerializeField] private GameObject foodPrefab;

    private void Start()
    {
        DrawFoodGate();
    }

    public void DrawFoodGate()
    {
        for (int i = 0; i < foodGateData.board.Length; i++)
        {
            for (int j = 0; j < foodGateData.board[i].column.Length; j++)
            {
                var cell = foodGateData.board[i].column[j];
                var position = gridTiles.GetTileWorldPosition(new Vector3Int(i, 0, j) + offset);
                Vector3 pos = Vector3.zero;
                if (position != null)
                {
                    pos = position.Value.Item1;
                }
                else
                {
                    Debug.Log("Null");
                }
                if (cell.type == FoodGateData.CellType.F)
                {
                    var food = Instantiate(foodPrefab, pos, Quaternion.identity);
                    food.GetComponent<CellObject>().color = GetColor(cell.colorType);
                    food.GetComponent<MeshRenderer>().material.color = GetColor(cell.colorType);
                }
                else if (cell.type == FoodGateData.CellType.G)
                {
                    GameManager.Instance.AddGateCount();
                    var gate = Instantiate(gatePrefab, pos, Quaternion.identity);
                    gate.GetComponent<CellObject>().color = GetColor(cell.colorType);
                    gate.GetComponent<MeshRenderer>().material.color = GetColor(cell.colorType);
                }
            }
        }
    }

    private Color GetColor(FoodGateData.ColorType type)
    {
        switch (type)
        {
            case FoodGateData.ColorType.None: return Color.gray;
            case FoodGateData.ColorType.Red: return Color.red;
            case FoodGateData.ColorType.Blue: return Color.blue;
            case FoodGateData.ColorType.Green: return Color.green;
            case FoodGateData.ColorType.Yellow: return Color.yellow;
            case FoodGateData.ColorType.Orange: return Color.Lerp(Color.red, Color.yellow, 0.5f);
            default: return Color.gray;
        }
    }
}

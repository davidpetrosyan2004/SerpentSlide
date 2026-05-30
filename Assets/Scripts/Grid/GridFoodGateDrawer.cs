using UnityEngine;
using System.Collections.Generic;

public class GridFoodGateDrawer : MonoBehaviour
{
    [SerializeField] private GridTiles gridTiles;
    [SerializeField] private Vector3Int offset;

    [SerializeField] private FoodGateData foodGateData;
    [SerializeField] private GameObject gatePrefab;
    [SerializeField] private GameObject foodPrefab;

    [SerializeField] private List<Texture> gateTextures;
    [SerializeField] private List<Texture> foodTextures;

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
                    food.GetComponent<CellObject>().texture = GetTextureFood(cell.colorType);
                    food.GetComponent<CellObject>().color = GetColor(cell.colorType);
                    food.GetComponentInChildren<MeshRenderer>().material.SetTexture("_BaseMap", GetTextureFood(cell.colorType));
                }
                else if (cell.type == FoodGateData.CellType.G)
                {
                    GameManager.Instance.AddGateCount();
                    var gate = Instantiate(gatePrefab, pos, Quaternion.identity);
                    gate.GetComponent<CellObject>().texture = GetTextureGate(cell.colorType);
                    gate.GetComponent<CellObject>().color = GetColor(cell.colorType);
                    gate.GetComponent<CellObject>().mesh.material.SetTexture("_BaseMap", GetTextureGate(cell.colorType));
                    gate.GetComponent<CellObject>().meshRing.material.color = GetColor(cell.colorType);
                }
            }
        }
    }

    private Texture GetTextureFood(FoodGateData.ColorType type)
    {
        switch (type)
        {
            case FoodGateData.ColorType.None: return null;
            case FoodGateData.ColorType.Purple: return foodTextures[0];
            case FoodGateData.ColorType.Blue: return foodTextures[1];
            case FoodGateData.ColorType.Green: return foodTextures[2];
            case FoodGateData.ColorType.Yellow: return foodTextures[3];
            case FoodGateData.ColorType.Orange: return foodTextures[4];
            case FoodGateData.ColorType.Pink: return foodTextures[5];
            default: return null;
        }
    }
    private Texture GetTextureGate(FoodGateData.ColorType type)
    {
        switch (type)
        {
            case FoodGateData.ColorType.None: return null;
            case FoodGateData.ColorType.Purple: return gateTextures[0];
            case FoodGateData.ColorType.Blue: return gateTextures[1];
            case FoodGateData.ColorType.Green: return gateTextures[2];
            case FoodGateData.ColorType.Yellow: return gateTextures[3];
            case FoodGateData.ColorType.Orange: return gateTextures[4];
            case FoodGateData.ColorType.Pink: return gateTextures[5];
            default: return null;
        }
    }
    private Color GetColor(FoodGateData.ColorType type)
    {
        switch (type)
        {
            case FoodGateData.ColorType.None: return Color.gray;
            case FoodGateData.ColorType.Purple: return Color.purple;
            case FoodGateData.ColorType.Blue: return new Color32(96, 180, 210, 255);
            case FoodGateData.ColorType.Green: return Color.green;
            case FoodGateData.ColorType.Yellow: return Color.yellow;
            case FoodGateData.ColorType.Orange: return Color.orange;
            case FoodGateData.ColorType.Pink: return Color.pink;
            default: return Color.gray;
        }
    }
}

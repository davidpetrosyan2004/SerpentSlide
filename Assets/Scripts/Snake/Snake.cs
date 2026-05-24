using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
public class Snake : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform bodyPrefab;
    [SerializeField] private Transform headPrefab;
    [SerializeField] private Transform tailPrefab;

    [Header("References")]
    [SerializeField] private GridTiles gridTiles;

    [Header("Data")]
    [SerializeField] private SnakeData snakeData;
    [SerializeField] private Vector3Int spawnPos;

    public List<Transform> BodyParts { get; set; } = new();
    public Transform HeadPrefab { get; set; }
    public Transform TailPrefab { get; set; }

    [SerializeField] private int height;
    [SerializeField] private int width;

    private void Awake()
    {
        int lastIndex = int.MinValue;
        for (int i = 0; i < snakeData.rows; i++)
        {
            for (int j = 0; j < snakeData.columns; j++)
            {
                if (snakeData.board[i].column[j].type == SnakeData.CellType.H)
                {
                    HeadPrefab = Instantiate(headPrefab, gridTiles.GetTileWorldPosition(new Vector3Int(i- width/2-1, 0, j- height/2-1)).Value.Item1, Quaternion.identity, transform);
                }
                else if (snakeData.board[i].column[j].type == SnakeData.CellType.B)
                { 
                    var bodyPart = Instantiate(bodyPrefab, gridTiles.GetTileWorldPosition(new Vector3Int(i - width / 2 - 1, 0, j - height / 2 - 1)).Value.Item1, Quaternion.identity, transform);
                    bodyPart.GetComponent<BodyPart>().index = snakeData.board[i].column[j].indexBody;
                    BodyParts.Add(bodyPart);
                }
                else if (snakeData.board[i].column[j].type == SnakeData.CellType.T)
                {
                    TailPrefab = Instantiate(tailPrefab, gridTiles.GetTileWorldPosition(new Vector3Int(i - width / 2 - 1, 0, j - height / 2 - 1)).Value.Item1, Quaternion.identity, transform);
                }
            }
        }
        BodyParts.Sort((a, b) => a.GetComponent<BodyPart>().index.CompareTo(b.GetComponent<BodyPart>().index));
        BodyParts.Add(TailPrefab);
    }
}

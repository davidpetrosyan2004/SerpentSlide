using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<Texture> snakeTextures;
    [SerializeField] public GameObject linkedSnake = null;
    private Color color;

    public bool isReversed { get; set; }
    public int coloredBodyPartsCount { get; set; }

    public List<Transform> BodyParts { get; set; } = new();
    public Transform HeadPrefab { get; set; }
    public Transform TailPrefab { get; set; }

    [SerializeField] private Vector3Int offset;

    public Snake keySnake = null;
    private bool isLocked = false;
    public GameObject lockImage;

    private void Awake()
    {
        if (keySnake != null) 
        {
            isLocked = true;
            var lockImagePrefab = Instantiate(lockImage, transform.position, Quaternion.identity, transform);
            lockImagePrefab.transform.localScale = Vector3.one;
            lockImagePrefab.transform.DOPunchScale(
                new Vector3(0.2f, 0.2f, 0.2f), // сила увеличения
                0.3f,                           // длительность
                8,                              // vibrato
                0.8f                            // elasticity
            );
        }
        if (linkedSnake != null)
        {
            var snake = linkedSnake.GetComponent<Snake>();

            var snakeControler = linkedSnake.GetComponent<SnakeControler>();
 
            MeshRenderer[] renderers = snakeControler.GetComponentsInChildren<MeshRenderer>();

            foreach (var r in renderers)
            {
                r.enabled = false;
            }
        }
        for (int i = 0; i < snakeData.rows; i++)
        {
            for (int j = 0; j < snakeData.columns; j++)
            {
                var spawnPos = gridTiles.GetTileWorldPosition(new Vector3Int(i, 0, j) + offset).Value.Item1;
                if (snakeData.board[i].column[j].type == SnakeData.CellType.H)
                {
                    HeadPrefab = Instantiate(headPrefab, spawnPos, Quaternion.identity, transform);
                    color = GetColor(snakeData.board[i].column[j].color);
                    HeadPrefab.GetComponent<SnakePart>().color = color;
                    HeadPrefab.GetComponent<MeshRenderer>().material.color = color;
                }
                else if (snakeData.board[i].column[j].type == SnakeData.CellType.B)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity, transform);
                    bodyPart.GetComponent<BodyPart>().index = snakeData.board[i].column[j].indexBody;
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (snakeData.board[i].column[j].type == SnakeData.CellType.T)
                {
                    TailPrefab = Instantiate(tailPrefab, spawnPos, Quaternion.identity, transform);
                    TailPrefab.GetComponent<SnakePart>().color = color;
                    coloredBodyPartsCount++;
                }
            }
        }
        BodyParts.Sort((a, b) => a.GetComponent<BodyPart>().index.CompareTo(b.GetComponent<BodyPart>().index));
        BodyParts.Add(TailPrefab);
    }


    private Texture GetTexture(SnakeData.ColorType colorType)
    {
        switch (colorType)
        {
            case SnakeData.ColorType.None: return null;
            case SnakeData.ColorType.Red: return snakeTextures[0];
            case SnakeData.ColorType.Blue: return  snakeTextures[1];
            case SnakeData.ColorType.Green: return snakeTextures[2];
            case SnakeData.ColorType.Yellow: return snakeTextures[3];
            case SnakeData.ColorType.Orange: return snakeTextures[4];
            default: return null;
        }
    }
    private Color GetColor(SnakeData.ColorType colorType)
    {
        switch (colorType)
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

    public void BodyPartFillColor()
    {
        if (!isReversed)
        {
            for (int i = 0; i < BodyParts.Count; i++)
            {
                if (BodyParts[i].GetComponent<BodyPart>().color != color)
                {
                    BodyParts[i].GetComponent<BodyPart>().SetColor(color);
                    coloredBodyPartsCount--;
                    return;
                }
            }
        }
        else
        {
            for (int i = BodyParts.Count - 2; i >= 0; i--)
            {
                if (BodyParts[i].GetComponent<BodyPart>().color != color)
                {
                    BodyParts[i].GetComponent<BodyPart>().SetColor(color);
                    coloredBodyPartsCount--;
                    return;
                }
            }
        }
        if (isReversed && coloredBodyPartsCount == 1)
        {
            TailPrefab.GetComponent<BodyPart>().SetColor(color);
            coloredBodyPartsCount--;
        }
    }
}           

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

    public Snake lockSnake = null;
    public bool isLocked { get; set; }
    public bool isKey { get; set; }
    public GameObject lockImagePrefab = null;
    public GameObject keyImagePrefab = null;

    private void Awake()
    {
        if (linkedSnake != null)
        {
            linkedSnake.GetComponent<SnakeControler>().isLinked = true;
            var snake = linkedSnake.GetComponent<Snake>();

            var snakeControler = linkedSnake.GetComponent<SnakeControler>();

            Collider[] cols = linkedSnake.GetComponentsInChildren<Collider>();

            foreach (var col in cols)
            {
                col.enabled = false;
            }

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
                var cell = snakeData.board[i].column[j];
                if (snakeData.board[i].column[j].type == SnakeData.CellType.H)
                {
                    HeadPrefab = Instantiate(headPrefab, spawnPos, Quaternion.Euler(0, cell.rotation, 0), transform);
                    color = GetColor(cell.color);
                    var HeadScript = HeadPrefab.GetChild(0).GetComponent<SnakePart>();
                    HeadScript.color = color;
                    HeadScript.partMesh.material.color = color;
                }
                else if (cell.type == SnakeData.CellType.B)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity, transform);
                    bodyPart.GetComponent<BodyPart>().index = snakeData.board[i].column[j].indexBody;
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (cell.type == SnakeData.CellType.T)
                {
                    TailPrefab = Instantiate(tailPrefab, spawnPos, Quaternion.identity, transform);
                    TailPrefab.GetComponent<SnakePart>().color = color;
                    coloredBodyPartsCount++;
                }
            }
        }
        BodyParts.Sort((a, b) => a.GetComponent<BodyPart>().index.CompareTo(b.GetComponent<BodyPart>().index));
        BodyParts.Add(TailPrefab);

        if (lockImagePrefab != null)
        {
            isLocked = true;
            InitSpriteObject(lockImagePrefab);
        }
        if (keyImagePrefab != null)
        {
            isKey = true;
            InitSpriteObject(keyImagePrefab);
        }
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

    public void InitSpriteObject(GameObject sprite)
    {
        sprite.transform.rotation = Quaternion.Euler(90, 0, 0);
        sprite.transform.position = HeadPrefab.position + new Vector3(0, 0.4f, 0);
        sprite.transform.SetParent(HeadPrefab.transform);
        sprite.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        sprite.transform.DOPunchScale(
            new Vector3(0.1f, 0.1f, 0.1f),
            0.3f,
            8,
            0.8f
        );
    }
}           

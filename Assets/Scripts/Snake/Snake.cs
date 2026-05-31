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
    private Texture texture;
    public Color color;

    public bool isReversed { get; set; }
    public int coloredBodyPartsCount { get; set; }

    public List<Transform> BodyParts { get; set; } = new();
    public Transform HeadPrefab { get; set; }
    public Transform TailPrefab { get; set; }

    [SerializeField] private Vector3Int offset;

    public Snake lockSnake = null;
    [SerializeField] public GameObject linkedSnake = null;
    public bool isLocked { get; set; }
    public bool isKey { get; set; }
    public GameObject lockImagePrefab = null;
    public GameObject keyImagePrefab = null;

    public bool isTutorial;
    public Transform twoSkinnedSphere = null;
    public Transform skinnedShpere;
    private void Awake()
    {
        for (int i = 0; i < snakeData.rows; i++)
        {
            for (int j = 0; j < snakeData.columns; j++)
            {
                var spawnPos = gridTiles.GetTileWorldPosition(new Vector3Int(i, 0, j) + offset).Value.Item1;
                var cell = snakeData.board[i].column[j];
                if (snakeData.board[i].column[j].type == SnakeData.CellType.H)
                {
                    HeadPrefab = Instantiate(headPrefab, spawnPos, Quaternion.Euler(0, cell.rotation, 0), transform);
                    texture = GetTexture(cell.color);
                    var HeadScript = HeadPrefab.GetComponent<SnakePart>();
                    HeadScript.texture = texture;
                    HeadScript.partMesh.material.SetTexture("_BaseMap", texture);
                    color = GetColor(cell.color);
                    HeadScript.color = color;
                }
                else if (cell.type == SnakeData.CellType.B)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.Euler(0, cell.rotation, 0), transform);
                    bodyPart.GetComponent<BodyPart>().index = snakeData.board[i].column[j].indexBody;
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (cell.type == SnakeData.CellType.UR)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity, transform);
                    var bodyScript = bodyPart.GetComponent<BodyPart>();
                    bodyScript.index = snakeData.board[i].column[j].indexBody;
                    bodyScript.SetGraphic("UR corner");
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (cell.type == SnakeData.CellType.UL)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity, transform);
                    var bodyScript = bodyPart.GetComponent<BodyPart>();
                    bodyScript.index = snakeData.board[i].column[j].indexBody;
                    bodyScript.SetGraphic("UL corner");
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (cell.type == SnakeData.CellType.DL)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity, transform);
                    var bodyScript = bodyPart.GetComponent<BodyPart>();
                    bodyScript.index = snakeData.board[i].column[j].indexBody;
                    bodyScript.SetGraphic("DL corner");
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (cell.type == SnakeData.CellType.DR)
                { 
                    var bodyPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity, transform);
                    var bodyScript = bodyPart.GetComponent<BodyPart>();
                    bodyScript.index = snakeData.board[i].column[j].indexBody;
                    bodyScript.SetGraphic("DR corner");
                    BodyParts.Add(bodyPart);
                    coloredBodyPartsCount++;
                }
                else if (cell.type == SnakeData.CellType.T)
                {
                    TailPrefab = Instantiate(tailPrefab, spawnPos, Quaternion.Euler(0, cell.rotation, 0), transform);
                    //TailPrefab.GetComponent<SnakePart>().texture = GetTexture(cell.color);
                    TailPrefab.GetComponent<SnakePart>().color = GetColor(cell.color);
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

    private void Start()
    {
        if (linkedSnake != null)
        {
            linkedSnake.GetComponent<SnakeControler>().isLinked = true;
            var snakeControler = linkedSnake.GetComponent<SnakeControler>();

            //Collider[] cols = linkedSnake.transform.GetChild(0).GetComponentsInChildren<Collider>();

            //foreach (var col in cols)
            //{
            //    col.enabled = false;
            //}
            //Collider[] cols2 = linkedSnake.transform.GetComponentsInChildren<Collider>();

            //foreach (var col in cols2)
            //{
            //    col.enabled = false;
            //}

            //SkinnedMeshRenderer[] renderers = linkedSnake.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            //foreach (var r in renderers)
            //{
            //    r.enabled = false;
            //}
            //MeshRenderer[] renderers2 = linkedSnake.transform.GetComponentsInChildren<MeshRenderer>();
            //foreach (var r in renderers2)
            //{
            //    r.enabled = false;
            //}
            foreach (Transform child in linkedSnake.transform)
            {
                child.gameObject.SetActive(false);
            }

            if (twoSkinnedSphere != null)
            {   
                skinnedShpere = Instantiate(twoSkinnedSphere, HeadPrefab.position + new Vector3(0, -0.17f, 0) + HeadPrefab.right * 0.17f, Quaternion.identity, HeadPrefab);
                skinnedShpere.GetComponent<MeshRenderer>().material.color = linkedSnake.GetComponent<Snake>().color;
            }
        }
    }
    private Texture GetTexture(SnakeData.ColorType colorType)
    {
        switch (colorType)
        {
            case SnakeData.ColorType.None: return snakeTextures[0];
            case SnakeData.ColorType.Blue: return  snakeTextures[1];
            case SnakeData.ColorType.Green: return snakeTextures[2];
            case SnakeData.ColorType.Yellow: return snakeTextures[3];
            case SnakeData.ColorType.Orange: return snakeTextures[4];
            case SnakeData.ColorType.Pink: return snakeTextures[5];
            case SnakeData.ColorType.Purple: return snakeTextures[6];
            default: return null;
        }
    }
    private Color GetColor(SnakeData.ColorType type)
    {
        switch (type)
        {
            case SnakeData.ColorType.None: return Color.gray;
            case SnakeData.ColorType.Blue: return new Color32(96, 180, 210, 255);
            case SnakeData.ColorType.Green: return Color.green;
            case SnakeData.ColorType.Yellow: return Color.yellow;
            case SnakeData.ColorType.Orange: return Color.orange;
            case SnakeData.ColorType.Pink: return Color.pink;
            case SnakeData.ColorType.Purple: return Color.purple;
            default: return Color.gray;
        }
    }
    public void BodyPartFillColor()
    {
        if (!isReversed)
        {
            for (int i = 0; i < BodyParts.Count; i++)
            {
                if (i == BodyParts.Count - 1)
                {
                    Debug.Log(texture);
                    BodyParts[i].GetComponent<SnakePart>().SetColor(texture);
                    coloredBodyPartsCount--;
                    return;
                }
                else if (BodyParts[i].GetComponent<BodyPart>().texture != texture)
                {
                    Debug.Log(texture);
                    BodyParts[i].GetComponent<BodyPart>().SetColor(texture);
                    coloredBodyPartsCount--;
                    return;
                }
            }

        }
        else
        {
            for (int i = BodyParts.Count - 2; i >= 0; i--)
            {
                if (BodyParts[i].GetComponent<BodyPart>().texture != texture)
                {
                    Debug.Log(texture);
                    BodyParts[i].GetComponent<BodyPart>().SetColor(texture);
                    coloredBodyPartsCount--;
                    return;
                }
            }
        }
        if (isReversed && coloredBodyPartsCount == 1)
        {
            Debug.Log(texture);
            TailPrefab.GetComponent<SnakePart>().SetColor(texture);
            coloredBodyPartsCount--;
        }
    }

    public void InitSpriteObject(GameObject sprite)
    {
        sprite.transform.position = HeadPrefab.position + new Vector3(0, 0.4f, 0);
        sprite.transform.SetParent(HeadPrefab.transform);
        sprite.transform.rotation = Quaternion.Euler(90, 180, 0);
        if (isLocked)
        {
            sprite.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }
        else
        {
            sprite.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        sprite.transform.DOPunchScale(
            new Vector3(0.1f, 0.1f, 0.1f),
            0.3f,
            8,
            0.8f
        );
    }

}           

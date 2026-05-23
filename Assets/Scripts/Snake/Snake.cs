using UnityEngine;
using System.Collections.Generic;
public class Snake : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform bodyPrefab;
    [SerializeField] private Transform headPrefab;
    [SerializeField] private Transform tailPrefab;


    [Header("Data")]
    [SerializeField] private SnakeData snakeData;

    public List<Transform> BodyParts { get; set; } = new();
    public Transform HeadPrefab { get; set; }
    public Transform TailPrefab { get; set; }

    private void Awake()
    {
        HeadPrefab = Instantiate(headPrefab, transform.position, Quaternion.identity, transform);
        for (int i = 0; i < snakeData.snakeLength; i++) 
        {
            BodyParts.Add(Instantiate(bodyPrefab, HeadPrefab.position - new Vector3(1, 0, 0) * (i+1), HeadPrefab.rotation));
        }
        TailPrefab = Instantiate(tailPrefab, BodyParts[BodyParts.Count - 1].position - new Vector3(1, 0, 0), HeadPrefab.rotation);
    }
}

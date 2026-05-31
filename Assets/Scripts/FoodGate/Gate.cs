using UnityEngine;
using System.Linq;
public class Gate : CellObject
{
    [SerializeField] private ParticleSystem gatePuffEffect;
    private Vector3[] divePositions;
    private void Start()
    {
        divePositions = new Vector3[6];
        for(int i = 0; i < 6; i++)
        {
            divePositions[i] = transform.position + Vector3.down * i * 2f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Tail"))
        {
            Debug.Log("Snake entered the gate");
            var snakeControler = other.GetComponentInParent<SnakeControler>();
            if (snakeControler.IsSnakeFilled() && snakeControler.snake.color == color ) {
                if (snakeControler.snake.linkedSnake != null)
                {

                    Debug.Log("Linked Snake Found");
                    foreach (Transform child in snakeControler.snake.linkedSnake.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    snakeControler.snake.linkedSnake.GetComponent<SnakeControler>().isLinked = false;
                    snakeControler.snake.linkedSnake = null;
                }
                snakeControler.snake.BodyParts[^1].GetComponent<Collider>().enabled = false;
                snakeControler.DivePositions = divePositions;
                snakeControler.gate = this;
                snakeControler.isDiving = true;
                GameManager.Instance.OnSnakeDive?.Invoke();
            }
        }
    }

    public void OnBodyPartDiveEffect()
    {
        var puff = Instantiate(gatePuffEffect, transform.position, Quaternion.identity, transform);
        var renderer = puff.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetTexture("_MainTex", texture);

        puff.Play();
    }
}

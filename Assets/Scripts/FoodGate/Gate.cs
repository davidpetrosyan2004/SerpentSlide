using UnityEngine;

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
            if (snakeControler.IsSnakeFilled()) {
                if (snakeControler.snake.linkedSnake != null)
                {
                    Debug.Log("Linked Snake Found");

                    snakeControler.GetComponent<Snake>().enabled = true;

                    Collider[] cols = snakeControler.snake.linkedSnake.transform.GetChild(0).GetComponentsInChildren<Collider>();

                    foreach (var col in cols)
                    {
                        col.enabled = true;
                    }

                    Collider[] cols2 = snakeControler.snake.linkedSnake.transform.GetComponentsInChildren<Collider>();

                    foreach (var col in cols2)
                    {
                        col.enabled = true;
                    }

                    MeshRenderer[] renderers = snakeControler.snake.linkedSnake.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();

                    foreach (var r in renderers)
                    {
                        r.enabled = true;
                    }
                    snakeControler.snake.linkedSnake.GetComponent<SnakeControler>().isLinked = false;
                    snakeControler.snake.linkedSnake = null;
                }
                snakeControler.DivePositions = divePositions;
                snakeControler.gate = this;
                snakeControler.isDiving = true;
            }
        }
    }

    public void OnBodyPartDiveEffect()
    {
        var puff = Instantiate(gatePuffEffect, transform.position, Quaternion.identity);
        var renderer = puff.GetComponent<ParticleSystemRenderer>();
        renderer.material.color = color;

        puff.Play();
    }
}

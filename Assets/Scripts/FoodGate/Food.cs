using UnityEngine;

public class Food : CellObject
{
    public void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (other.CompareTag("Head") || other.CompareTag("Tail"))
        {
            Snake snake = other.GetComponentInParent<Snake>();
            if (snake != null && snake.coloredBodyPartsCount > 0 && color == snake.color)
            {
                AudioManager.Instance.PlaySound("ColorChange", true);
                snake.BodyPartFillColor();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Snake has no colored body parts to change texture.");
            }

        }
    }
}

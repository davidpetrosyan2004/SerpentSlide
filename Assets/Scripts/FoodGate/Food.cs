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
            Debug.Log(other.tag);
            if (snake != null && snake.coloredBodyPartsCount > 0)
            {
                AudioManager.Instance.PlaySound("ColorChange", true);
                snake.BodyPartFillColor();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Snake has no colored body parts to change color.");
            }

        }
    }
}

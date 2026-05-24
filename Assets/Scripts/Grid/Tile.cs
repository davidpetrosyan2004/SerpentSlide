using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;

    private void OnCollisionEnter(Collision collision)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }
}

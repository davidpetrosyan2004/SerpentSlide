using UnityEngine;

public enum SnakeColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple
}

[CreateAssetMenu(fileName = "New Snake Data", menuName = "Snake Data", order = 1)]
public class SnakeData : ScriptableObject
{
    public int snakeLength;
    public SnakeColor snakeColor;
}

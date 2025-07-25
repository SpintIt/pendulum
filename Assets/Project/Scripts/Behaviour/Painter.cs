using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void SetColor(BallColorType color)
    {
        switch (color)
        {
            case BallColorType.Red:
                _spriteRenderer.color = Color.red;
                break;
            case BallColorType.Green:
                _spriteRenderer.color = Color.green;
                break;
            case BallColorType.Blue:
                _spriteRenderer.color = Color.blue;
                break;
            case BallColorType.None:
                _spriteRenderer.color = Color.white;
                break;
        }
    }
}

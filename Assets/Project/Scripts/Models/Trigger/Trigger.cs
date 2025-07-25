using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] public GamePlay _gamePlay;
    [field: SerializeField] public TriggerType Type { get; private set; }
    [field: SerializeField] public int X_matrix { get; private set; }
    [field: SerializeField] public int Y_matrix { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            _gamePlay.MatchThree.SetCell(ball, this);
        }
    }
}

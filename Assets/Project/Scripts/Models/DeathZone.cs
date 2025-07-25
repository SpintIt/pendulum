using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GamePlay _gamePlay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            _gamePlay.RemoveBalls(new List<Ball>() { ball });
        }
    }
}

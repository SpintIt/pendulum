using TMPro;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreCount;

    public UIGameOver ShowScore(int count)
    {
        _scoreCount.text = count.ToString();
        return this;
    }
}

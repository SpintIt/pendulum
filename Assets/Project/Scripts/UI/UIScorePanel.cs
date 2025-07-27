using TMPro;
using UnityEngine;

public class UIScorePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _count;

    public void ShowScore(int count)
        => _count.text = count.ToString();
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreCounter
{
    private readonly UIScorePanel _uiScorePanel;
    private readonly MatchThree _matchThree;
    private readonly List<ScoreForColorProperty> _scoresForColor;

    public int ScoreCount { get; private set; }

    public ScoreCounter(UIScorePanel uiScorePanel, MatchThree matchThree, List<ScoreForColorProperty> scoresForColor)
    {
        _uiScorePanel = uiScorePanel;
        _matchThree = matchThree;
        _scoresForColor = scoresForColor;

        Reset();
        _matchThree.OnCompleteColor += CompleteColor;
    }

    public void Reset()
    {
        ScoreCount = 0;
        _uiScorePanel.ShowScore(ScoreCount);
    }

    private void CompleteColor(BallColorType colorType)
    {
        int count = _scoresForColor.FirstOrDefault(color => color.BallColorType == colorType).Score;
        ScoreCount += count;
        _uiScorePanel.ShowScore(ScoreCount);
    }
}

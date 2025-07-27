using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchThree
{
    private readonly bool _isDedug;

    private readonly Ball[,] _matrix;

    public int Rows => _matrix.GetLength(0);
    public int Cols => _matrix.GetLength(1);

    public event UnityAction<List<Ball>> OnRemoveBall;
    public event UnityAction OnGameOver;
    public event UnityAction<BallColorType> OnCompleteColor;

    public MatchThree()
    {
        _isDedug = false;
        _matrix = new Ball[3, 3];
    }

    public List<Ball> GetBalls()
    {
        List<Ball> filledBalls = new();
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (_matrix[r, c] != null)
                {
                    filledBalls.Add(_matrix[r, c]);
                }
            }
        }

        return filledBalls;
    }

    public void Clear() 
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                _matrix[r, c] = null;
            }
        }
        if (_isDedug) LogMatrix();
    }

    public void SetCell(Ball ball, Trigger trigger)
    {
        int addedX = trigger.X_matrix;
        int addedY = trigger.Y_matrix;

        _matrix[addedX, addedY] = ball;

        if (addedX - 1 >= 0)
        {
            Ball ballToRemove1 = _matrix[addedX - 1, addedY];
            if (ballToRemove1 != null)
            {
                _matrix[addedX - 1, addedY] = null;
                if (_isDedug) Debug.Log($"Удаление элемента ({addedX - 1}, {addedY}).");
            }
        }

        if (addedX - 2 >= 0)
        {
            Ball ballToRemove2 = _matrix[addedX - 2, addedY];
            if (ballToRemove2 != null)
            {
                _matrix[addedX - 2, addedY] = null;
                if (_isDedug) Debug.Log($"Удаление элемента ({addedX - 2}, {addedY}).");
            }
        }

        LogMatrix();

        List<Ball> match = CheckMatches();

        if (match.Count > 0)
        {
            OnCompleteColor?.Invoke(match[0].ColorType);
            RemoveBalls(match);
        }
    }

    public void LogMatrix()
    {
        if (_isDedug == false)
            return;

        Debug.Log("--- Текущее состояние матрицы ---");
        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                Ball ball = _matrix[i, j];
                row += (ball != null ? "⚪" : "null") + "\t";
            }
            Debug.Log(row);
        }
        Debug.Log("---------------------------------");
    }

    private bool IsSupported(int r, int c)
    {
        if (r == Rows - 1)
            return true;

        return _matrix[r + 1, c] != null;
    }

    private List<Ball> CheckMatches()
    {
        List<Ball> matchedBalls = new List<Ball>();

        // Проверка горизонтальных линий
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c <= Cols - 3; c++)
            {
                Ball firstBall = _matrix[r, c];
                if (firstBall != null &&
                    firstBall.ColorType == _matrix[r, c + 1]?.ColorType &&
                    firstBall.ColorType == _matrix[r, c + 2]?.ColorType &&
                    IsSupported(r, c) &&
                    IsSupported(r, c + 1) &&
                    IsSupported(r, c + 2))
                {
                    if (_isDedug)
                        Debug.Log($"Найдена горизонтальная линия из трёх шаров цвета {firstBall.ColorType} в строке {r}, начиная с колонки {c}.");

                    matchedBalls.Add(_matrix[r, c]);
                    matchedBalls.Add(_matrix[r, c + 1]);
                    matchedBalls.Add(_matrix[r, c + 2]);
                    
                    return matchedBalls;
                }
            }
        }

        // Проверка вертикальных линий
        for (int c = 0; c < Cols; c++)
        {
            for (int r = 0; r <= Rows - 3; r++)
            {
                Ball firstBall = _matrix[r, c];
                if (firstBall != null &&
                    firstBall.ColorType == _matrix[r + 1, c]?.ColorType &&
                    firstBall.ColorType == _matrix[r + 2, c]?.ColorType &&
                    IsSupported(r + 2, c))
                {
                    if (_isDedug)
                        Debug.Log($"Найдена вертикальная линия из трёх шаров цвета {firstBall.ColorType} в колонке {c}, начиная со строки {r}.");

                    matchedBalls.Add(_matrix[r, c]);
                    matchedBalls.Add(_matrix[r + 1, c]);
                    matchedBalls.Add(_matrix[r + 2, c]);
                    return matchedBalls;
                }
            }
        }

        // Проверка диагоналей (сверху влево донизу вправо)
        for (int r = 0; r <= Rows - 3; r++)
        {
            for (int c = 0; c <= Cols - 3; c++)
            {
                Ball firstBall = _matrix[r, c];
                if (firstBall != null &&
                    firstBall.ColorType == _matrix[r + 1, c + 1]?.ColorType &&
                    firstBall.ColorType == _matrix[r + 2, c + 2]?.ColorType &&
                    IsSupported(r, c) &&
                    IsSupported(r + 1, c + 1) &&
                    IsSupported(r + 2, c + 2))
                {
                    if (_isDedug)
                        Debug.Log($"Найдена диагональная линия (\\) из трёх шаров цвета {firstBall.ColorType} начиная с [{r},{c}].");

                    matchedBalls.Add(_matrix[r, c]);
                    matchedBalls.Add(_matrix[r + 1, c + 1]);
                    matchedBalls.Add(_matrix[r + 2, c + 2]);
                    return matchedBalls;
                }
            }
        }

        // Проверка диагоналей (сверху вправо донизу влево)
        for (int r = 0; r <= Rows - 3; r++)
        {
            for (int c = 2; c < Cols; c++)
            {
                Ball firstBall = _matrix[r, c];
                if (firstBall != null &&
                    firstBall.ColorType == _matrix[r + 1, c - 1]?.ColorType &&
                    firstBall.ColorType == _matrix[r + 2, c - 2]?.ColorType &&
                    IsSupported(r, c) &&
                    IsSupported(r + 1, c - 1) &&
                    IsSupported(r + 2, c - 2))
                {
                    if (_isDedug)
                        Debug.Log($"Найдена диагональная линия (/) из трёх шаров цвета {firstBall.ColorType} начиная с [{r},{c}].");

                    matchedBalls.Add(_matrix[r, c]);
                    matchedBalls.Add(_matrix[r + 1, c - 1]);
                    matchedBalls.Add(_matrix[r + 2, c - 2]);
                    return matchedBalls;
                }
            }
        }

        if (matchedBalls.Count == 0)
        {
            bool isMatrixFull = true;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (_matrix[r, c] == null)
                    {
                        isMatrixFull = false;
                        break;
                    }
                }

                if (!isMatrixFull) break;
            }

            if (isMatrixFull)
            {
                if (_isDedug) Debug.Log("Матрица заполнена, и совпадений нет. Игра окончена!");
                OnGameOver?.Invoke();
            }
        }

        return matchedBalls;
    }

    private void RemoveBalls(List<Ball> ballsToRemove)
    {
        OnRemoveBall?.Invoke(ballsToRemove);

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (_matrix[r, c] != null && ballsToRemove.Contains(_matrix[r, c]))
                {
                    _matrix[r, c] = null;
                }
            }
        }

        LogMatrix();
    }
}

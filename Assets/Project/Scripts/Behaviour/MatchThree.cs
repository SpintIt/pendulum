using System.Collections.Generic;
using UnityEngine;

public class MatchThree
{
    private readonly Ball[,] _matrix;
    private int _rows => _matrix.GetLength(0);
    private int _cols => _matrix.GetLength(1);

    public MatchThree()
    {
        _matrix = new Ball[3, 3];
    }

    public void SetCell(Ball ball, Trigger trigger)
    {
        _matrix[trigger.X_matrix, trigger.Y_matrix] = ball;

        // LogMatrix();

        List<Ball> firstMatch = CheckForMatchesAndReturnFirst();

        if (firstMatch.Count > 0)
        {
            Debug.Log($"Найдена первая линия из {firstMatch.Count} шаров.");
            // Удаляем найденные шары
            // game.RemoveBalls(firstMatch);
            // game.LogMatrix(); // Показываем матрицу ПОСЛЕ удаления
        }
    }

    public void LogMatrix()
    {
        Debug.Log("--- Текущее состояние матрицы ---");
        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                Ball ball = _matrix[i, j];
                row += (ball != null ? ball.ToString() : "null") + "\t";
            }
            Debug.Log(row);
        }
        Debug.Log("---------------------------------");
    }

    public List<Ball> CheckForMatchesAndReturnFirst()
    {
        List<Ball> matchedBalls = new List<Ball>();

        // Проверяем горизонтальные линии
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c <= _cols - 3; c++)
            {
                Ball firstBall = _matrix[r, c];
                if (firstBall != null &&
                    firstBall.ColorType == _matrix[r, c + 1]?.ColorType &&
                    firstBall.ColorType == _matrix[r, c + 2]?.ColorType)
                {
                    Debug.Log($"Найдена горизонтальная линия из трёх шаров цвета {firstBall.ColorType} в строке {r}, начиная с колонки {c}.");
                    matchedBalls.Add(_matrix[r, c]);
                    matchedBalls.Add(_matrix[r, c + 1]);
                    matchedBalls.Add(_matrix[r, c + 2]);
                    return matchedBalls; // Возвращаем первую найденную линию
                }
            }
        }

        // Проверяем вертикальные линии
        for (int c = 0; c < _cols; c++)
        {
            for (int r = 0; r <= _rows - 3; r++)
            {
                Ball firstBall = _matrix[r, c];
                if (firstBall != null &&
                    firstBall.ColorType == _matrix[r + 1, c]?.ColorType &&
                    firstBall.ColorType == _matrix[r + 2, c]?.ColorType)
                {
                    Debug.Log($"Найдена вертикальная линия из трёх шаров цвета {firstBall.ColorType} в колонке {c}, начиная со строки {r}.");
                    matchedBalls.Add(_matrix[r, c]);
                    matchedBalls.Add(_matrix[r + 1, c]);
                    matchedBalls.Add(_matrix[r + 2, c]);
                    return matchedBalls; // Возвращаем первую найденную линию
                }
            }
        }

        return matchedBalls; // Возвращаем пустой список, если ничего не найдено
    }
    
    public void RemoveBalls(List<Ball> ballsToRemove)
    {
        if (ballsToRemove == null || ballsToRemove.Count == 0)
        {
            Debug.Log("Нет шаров для удаления.");
            return;
        }

        Debug.Log("Удаление найденных шаров из матрицы...");
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                // Проверяем, содержится ли текущий шар матрицы в списке на удаление
                if (_matrix[r, c] != null && ballsToRemove.Contains(_matrix[r, c]))
                {
                    Debug.Log($"Удален шар цвета {_matrix[r, c].ColorType} из позиции ({r}, {c}).");
                    _matrix[r, c] = null; // Устанавливаем ячейку в null
                }
            }
        }
        Debug.Log("Шары удалены.");
    }
}

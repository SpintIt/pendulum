using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [Header("Pool Balls")]
    [SerializeField, Range(1, 50)] private int _startBallCount = 10;
    [SerializeField] private Transform _parentBallSignals;
    [SerializeField] private Ball _prefabBall;
    private BallPoolHandler _ballPool;
    private BallSpawner _ballSpawner;
    private Ball _currentBall;

    [Header("Pendulum")]
    [SerializeField] private Pendulum _pendulum;
    [SerializeField] private LayerMask _layerMaskBall;

    [Header("Match Three")]
    [SerializeField] private List<ScoreForColorProperty> _scoresForColor;

    [Header("Application")]
    [SerializeField] private EventSystem _eventSystem;

    public MatchThree MatchThree { get; private set; }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        MatchThree = new();

        _eventSystem.Init();

        _ballPool = new BallPoolHandler(_prefabBall, _parentBallSignals, _startBallCount);
        _ballSpawner = new(_ballPool);

        TryCreateBall();
    }

    private void OnEnable()
    {
        _eventSystem.OnClick += OnClick;
        _eventSystem.PressAnyKey += PressAnyKey;
        MatchThree.OnRemoveBall += RemoveBalls;
        MatchThree.OnGameOver += GameOver;
        MatchThree.OnCompleteColor += CompleteColor;
    }

    private void OnDisable()
    {
        _eventSystem.OnClick -= OnClick;
        _eventSystem.PressAnyKey -= PressAnyKey;
        MatchThree.OnRemoveBall -= RemoveBalls;
        MatchThree.OnGameOver -= GameOver;
        MatchThree.OnCompleteColor -= CompleteColor;
    }

    private void TryCreateBall()
    {
        if (_currentBall && _currentBall.IsConnected == false)
            return;

        if (_currentBall != null)
            _currentBall.Disconnect();

        _currentBall = _ballSpawner.Spawn()
            .Show();

        _currentBall.ConnectTo(_pendulum);
    }

    public void RemoveBalls(List<Ball> balls)
    {
        balls.ForEach(ball => {
            ball.Burst();
            _ballPool.Return(ball);
        });
    }

    private void GameOver()
    {
        RemoveBalls(MatchThree.GetBalls());
        MatchThree.Clear();
    }

    private void CompleteColor(BallColorType colorType)
    {
        Debug.Log(colorType);
    }

    private void OnClick(Vector2 mousePosition)
        => TryCreateBall();

    private void PressAnyKey(string key)
    {
        if (key == Constants.KEY_SPACE)
        {
            TryCreateBall();
        }
    }
}

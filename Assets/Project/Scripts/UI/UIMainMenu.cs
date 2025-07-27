using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    private BallPoolHandler _ballPool;
    private BallSpawner _ballSpawner;
    private Tweener _loaderTweener;
    private List<Ball> _activeBalls = new List<Ball>();

    [SerializeField] private Transform _coub;
    [SerializeField, Range(3f, 10f)] private float _speed = 2f;
    [SerializeField] private float _spawnRadius = 4f;
    [SerializeField] private Transform _targetSpawn;
    [SerializeField] private Ball _prefabBall;
    [SerializeField, Range(1, 50)] private int _startBallCount = 10;
    [SerializeField] private Transform _parentBallPool;


    public void Awake()
    {
        _ballPool = new BallPoolHandler(_prefabBall, _parentBallPool, _startBallCount);
        _ballSpawner = new(_ballPool);
    }

    private void OnEnable()
    {
        for (int i = 0; i < _startBallCount; i++)
        {
            SpawnBall();
        }

        _loaderTweener = _coub.DORotate(new Vector3(0, 0, -360), _speed, RotateMode.FastBeyond360).SetRelative(true)
           .SetEase(Ease.Linear).SetLoops(-1);
    }

    private void OnDisable()
    {
        _loaderTweener?.Kill();
        RemoveBalls(_activeBalls);
        _activeBalls.Clear();
    }

    public void RemoveBalls(List<Ball> balls)
    {
        balls.ForEach(ball => {
            if (ball != null)
            {
                _ballPool.Return(ball);
            }
        });
    }

    private void SpawnBall()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
        Vector3 spawnPosition = (Vector2)_targetSpawn.position + randomOffset;

        Ball ball = _ballSpawner.Spawn()
            .With(item => item.transform.position = spawnPosition)
            .Show();
        _activeBalls.Add(ball);
    }

    private void OnDrawGizmosSelected()
    {
        if (_targetSpawn != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_targetSpawn.position, _spawnRadius);
        }
    }
}
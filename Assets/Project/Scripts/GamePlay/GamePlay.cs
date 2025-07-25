using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [Header("Pool Balls")]
    [SerializeField, Range(1, 50)] private int _startBallCount = 10;
    [SerializeField] private Transform _parentBallSignals;
    [SerializeField] private Ball _prefabBall;
    private BallSpawner _ballSpawner;

    [Header("Pendulum")]
    [SerializeField] private Pendulum _pendulum;
    [SerializeField] private LayerMask _layerMaskBall;

    [Header("Application")]
    [SerializeField] private EventSystem _eventSystem;

    public MatchThree MatchThree { get; private set; }


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        MatchThree = new();

        _eventSystem.Init();

        _ballSpawner = new(new BallPoolHandler(_prefabBall, _parentBallSignals, _startBallCount));
        CreateBall();
    }

    private void OnEnable()
    {
        _eventSystem.OnClick += OnClick;
    }

    private void OnDisable()
    {
        _eventSystem.OnClick -= OnClick;
    }

    private void CreateBall()
    {
        Ball ball = _ballSpawner.Spawn()
            .Show();

        ball.ConnectTo(_pendulum);
    }

    private void OnClick(Vector2 mousePosition)
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero, 100f, _layerMaskBall);

        if (hit.collider != null && hit.collider.TryGetComponent(out Ball ball))
        {
            if (ball.IsConnected == false)
                return;

            ball.Disconnect();
            CreateBall();
        }
    }
}

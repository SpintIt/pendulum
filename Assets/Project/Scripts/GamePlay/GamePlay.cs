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

    [Header("Application")]
    [SerializeField] private EventSystem _eventSystem;


    private void Awake()
    {
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

    // TODO Настроить слои физического взаимодействия, отключить верчение по оси Z

    private void CreateBall()
    {
        Ball ball = _ballSpawner.Spawn()
            .Show();

        ball.ConnectTo(_pendulum);
    }

    private void OnClick(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider != null && hit.collider.TryGetComponent(out Ball ball))
        {
            ball.Disconnect();
            CreateBall();
        }
    }
}

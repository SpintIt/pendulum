using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField, Range(-100f, 100f)] private float _speed;
    [SerializeField] private float _leftAngle;
    [SerializeField] private float _rightAngle;

    [field: SerializeField] public Rigidbody2D ConnectedRigidbody { get; private set; }

    [field: SerializeField] public Transform SpawnPosition { get; private set; }

    private bool _isBackMove;

    private void Update()
    {
        Move();
    }

    private void TryChangeDirection()
    { 
        if (ConnectedRigidbody.transform.rotation.z > _rightAngle)
            _isBackMove = true;

        if (ConnectedRigidbody.transform.rotation.z < _leftAngle)
            _isBackMove = false;
    }

    private void Move()
    {
        TryChangeDirection();

        if (_isBackMove)
            ConnectedRigidbody.angularVelocity = -1 * _speed;
        else
            ConnectedRigidbody.angularVelocity = _speed;
    }
}

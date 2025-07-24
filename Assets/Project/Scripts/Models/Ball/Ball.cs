using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _joint;
    [SerializeField] private Collider2D _collider;
    [SerializeField, Range(.1f, 1f)] private float _speedShow;

    public void ConnectTo(Pendulum pendulum) // inteface
    {
        transform.parent = pendulum.transform;
        transform.position = pendulum.SpawnPosition.transform.position;
        _joint.connectedBody = pendulum.ConnectedRigidbody;
    }

    public Ball Show()
    {
        transform
            .With(item => item.Show())
            .With(item => item.localScale = Vector3.zero)
            .With(item => item.DOScale(Vector3.one, _speedShow));

        return this;
    }

    public void Disconnect()
    {
        _joint.enabled = false;
    }
}

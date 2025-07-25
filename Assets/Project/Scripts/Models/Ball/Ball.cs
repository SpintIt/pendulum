using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallEffects _effets;
    [SerializeField] private Painter _painter;
    [SerializeField] private HingeJoint2D _joint;
    [SerializeField] private Collider2D _collider;
    [SerializeField, Range(.1f, 1f)] private float _speedShow;

    public bool IsConnected { get; private set; }
    public BallColorType ColorType { get; private set; }

    public void ConnectTo(Pendulum pendulum) // inteface
    {
        transform.parent = pendulum.transform;
        transform.position = pendulum.SpawnPosition.transform.position;
        _joint.enabled = true;
        _joint.connectedBody = pendulum.ConnectedRigidbody;
        IsConnected = true;
    }

    public Ball SetColor(BallColorType ballColorType)
    {
        ColorType = ballColorType;
        _painter.SetColor(ColorType);

        return this;        
    }

    public Ball Show()
    {
        _collider.enabled = false;
        transform
            .With(item => item.Show())
            .With(item => item.localScale = Vector3.zero)
            .With(item => item.DOScale(Vector3.one, _speedShow).OnComplete(() =>
            {
                _collider.enabled = true;
            }));

        return this;
    }

    public void Disconnect()
    {
        _joint.enabled = false;
        IsConnected = false;
    }

    public void Burst()
        => _effets.Burst(this);
}

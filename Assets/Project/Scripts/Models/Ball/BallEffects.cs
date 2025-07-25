using UnityEngine;

public class BallEffects : MonoBehaviour
{
    [Header("Burst")]
    [SerializeField] private ParticleSystem _burstParticle;

    public void Burst(Ball ball)
    {
        transform
            .With(item => item.parent = ball.transform)
            .With(item => item.position = ball.transform.position)
            .With(item => item.parent = null);
        
            _burstParticle.Play();
    }
}
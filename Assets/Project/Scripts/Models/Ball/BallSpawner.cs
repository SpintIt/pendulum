public class BallSpawner
{
    private BallPoolHandler _signalBallHandler;

    public BallSpawner(BallPoolHandler signalBallHandler)  // inteface
    {
        _signalBallHandler = signalBallHandler;
    }

    public Ball Spawn()
        => _signalBallHandler.Get();
}
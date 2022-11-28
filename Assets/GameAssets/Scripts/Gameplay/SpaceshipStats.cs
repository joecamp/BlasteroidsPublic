public struct SpaceshipStats
{
    public float TimeBetweenShots { get; }
    public float RotationSpeed { get; }
    public AttackBehavior AttackBehavior { get; }
    public int ScoreReward { get; }

    public SpaceshipStats(float timeBetweenShots, float rotationSpeed, AttackBehavior attackBehavior,
        int scoreReward)
    {
        TimeBetweenShots = timeBetweenShots;
        RotationSpeed = rotationSpeed;
        AttackBehavior = attackBehavior;
        ScoreReward = scoreReward;
    }
}

public enum AttackBehavior
{
    TargetNearest,
    TargetFurthest,
    TargetRandom
}
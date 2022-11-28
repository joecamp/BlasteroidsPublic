public static class SpaceshipFactory
{
    public static SpaceshipStats GenerateShooterStats()
    {
        float timeBetweenShots = 3f;
        float rotationSpeed = 1f;
        AttackBehavior attackBehavior = Utilities.GetRandomEnum<AttackBehavior>();
        int scoreReward = 10;

        return new SpaceshipStats(timeBetweenShots, rotationSpeed, attackBehavior, scoreReward);
    }

    public static SpaceshipStats GenerateLaserStats()
    {
        float timeBetweenShots = 6f;
        float rotationSpeed = 1f;
        AttackBehavior attackBehavior = Utilities.GetRandomEnum<AttackBehavior>();
        int scoreReward = 10;

        return new SpaceshipStats(timeBetweenShots, rotationSpeed, attackBehavior, scoreReward);
    }

    public static SpaceshipStats GenerateBombStats()
    {
        float timeBetweenShots = 0f;
        float rotationSpeed = 0f;
        AttackBehavior attackBehavior = Utilities.GetRandomEnum<AttackBehavior>();
        int scoreReward = 15;

        return new SpaceshipStats(timeBetweenShots, rotationSpeed, attackBehavior, scoreReward);
    }
}
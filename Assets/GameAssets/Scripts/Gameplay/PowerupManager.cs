using UnityEngine;

public class PowerupManager : Singleton<PowerupManager>
{
    public float FlingPower { get; private set; } = defaultFlingPower;
    private static float defaultFlingPower = 150f;
    private static float powerupFlingPower = 225f;

    public int NumberOfSmallAsteroidsToSpawn { get; private set; } = defaultNumberOfSmallAsteroidsToSpawn;
    private static int defaultNumberOfSmallAsteroidsToSpawn = 2;
    private static int powerupNumberOfSmallAsteroidsToSpawn = 3;

    public bool SlowdownOnAsteroidDrag { get; private set; } = false;
    public bool ShieldAsteroidsOnFling { get; private set; } = false;
    public float ShieldUptime { get; private set; } = 2.5f;
    public bool ChainReaction { get; private set; } = false;
    public float ChainReactionRadius { get; private set; } = 6.5f;
    public bool VortexOnRightMouse { get; private set; } = false;
    public float VortexForce { get; private set; } = 5f;

    public bool AreCheatsOn { get; private set; } = false;

    private void Awake()
    {
        PowerupOptions.OnPowerupSelected += ActivatePowerup;
    }

    public void Update()
    {
        ListenForCheats();
    }

    private void ActivatePowerup(Powerup powerup)
    {
        Debug.Log("ActivatedPowerup: " + powerup.ToString());

        switch (powerup)
        {
            case Powerup.Slingshot:
                FlingPower = powerupFlingPower;
                break;
            case Powerup.MoreAsteroids:
                NumberOfSmallAsteroidsToSpawn = powerupNumberOfSmallAsteroidsToSpawn;
                break;
            case Powerup.Timewarp:
                SlowdownOnAsteroidDrag = true;
                break;
            case Powerup.Armor:
                ShieldAsteroidsOnFling = true;
                break;
            case Powerup.ChainReaction:
                ChainReaction = true;
                break;
            case Powerup.Vortex:
                VortexOnRightMouse = true;
                break;
        }
    }

    public void ResetPowerups()
    {
        Debug.Log("Resetting powerups");

        FlingPower = defaultFlingPower;
        NumberOfSmallAsteroidsToSpawn = defaultNumberOfSmallAsteroidsToSpawn;
        SlowdownOnAsteroidDrag = false;
        ShieldAsteroidsOnFling = false;
        ChainReaction = false;
        VortexOnRightMouse = false;
    }

    string keyboardInput;
    private void ListenForCheats()
    {
        if (!AreCheatsOn)
        {
            string input = Input.inputString;
            if (input != "")
            {
                keyboardInput += input;

                if (keyboardInput.Contains("cheater"))
                {
                    AreCheatsOn = true;
                    UiManager.Instance.ActivateCheatsUI();
                }
            }
        }
    }
}

public enum Powerup
{
    Slingshot, MoreAsteroids, Timewarp, Armor, ChainReaction, Vortex
}
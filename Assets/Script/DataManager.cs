using UnityEngine;

public static class DataManager
{
    public static int Score { get; private set; }
    public static float Health { get; private set; }
    public static float Fuel { get; private set; }
    public static int Weapon { get; private set; }
    public static int Stage { get; private set; }
    public static bool Protect;
    private static TextManager textManager;

    public static void Reset()
    {
        textManager = GameObject.Find("TextManager").GetComponent<TextManager>();
        Score = 0;
        Fuel = 100;
        Health = 100;
        Weapon = 1;
        Protect = false;
        Stage = 1;
        textManager.UpdateHealthText();
        textManager.UpdateScoreText(0);
        textManager.UpdateStageText();
    }

    public static void SetFuel(int fuel) => Fuel = Mathf.Min(fuel, 100);

    public static void RemoveFuel(float fuel) => Fuel -= fuel;

    public static void AddScore(int score)
    {
        Score += score;
        textManager.UpdateScoreText(score);
    }

    public static void SetHealth(float health)
    {
        Health = health;
        textManager.UpdateHealthText();
    }

    public static void Heal(float health)
    {
        Health += health;
        textManager.UpdateHealthText();
    }

    public static void Damage(float health)
    {
        if (Protect) return;
        Health -= health;
        textManager.UpdateHealthText();
    }

    public static void MaxWeapon() => Weapon = 4;

    public static void UpgradeWeapon()
    {
        if (Weapon >= 4)
        {
            AddScore(Random.Range(150, 251));
            return;
        }
        Weapon++;
    }

    public static void FirstStage()
    {
        Stage = 1;
        textManager.UpdateStageText();
    }

    public static void NextStage()
    {
        Stage++;
        textManager.UpdateStageText();
    }

    public static bool IsDeath() => Health <= 0;
}
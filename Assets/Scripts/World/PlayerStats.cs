using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public static int money;
    public static int health;
    public static int maxHealth;
    public static float regenerationSpeed = 0;
    public int startHealth, startMoney;
    float countdown = 0;
    public Text healthText, moneyText;

    void Awake()
    {
        maxHealth = health = startHealth;
        money = startMoney;
    }

    void Update()
    {
        if (health == maxHealth) return;

        if (regenerationSpeed > 0 && WaveLauncher.IsWaveInProcess)
        {
            if (countdown <= 0)
            {
                health++;
                countdown = 1f / regenerationSpeed;
            }
            else
            {
                countdown -= Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        healthText.text = health + "/" + maxHealth;
        moneyText.text = "$" + money;

        if(health <= 0)
        {

        }
    }
}

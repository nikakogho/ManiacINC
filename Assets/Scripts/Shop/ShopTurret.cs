using UnityEngine;

public class ShopTurret : ShopItem
{
    public float priceMultiplier = 1.1f;
    public Turret turret;

    public override void Buy()
    {
        price = (int)(priceMultiplier * price);
        UpdatePriceText();

        if (turret.gameObject.activeSelf)
        {
            if (Random.Range(0, 5) > 0)
                turret.fireRate = turret.fireRate * 1.15f + 0.1f;
            else turret.damage *= 1.25f;
        }
        else
        {
            turret.gameObject.SetActive(true);
        }
    }

    public override string TextToDisplay()
    {
        return turret.name;
    }
}

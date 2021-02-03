public class ShopMaxHealthBuyer : ShopItem
{
    public override void Buy()
    {
        PlayerStats.maxHealth++;
        PlayerStats.health++;
    }

    public override string TextToDisplay()
    {
        return "Increase Max Health By One";
    }
}
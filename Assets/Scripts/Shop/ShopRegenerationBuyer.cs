public class ShopRegenerationBuyer : ShopItem
{
    public float priceMultiplier = 1.15f;
    public float upgradeValue = 0.5f;

    public override void Buy()
    {
        price = (int)(priceMultiplier * price);
        UpdatePriceText();
        PlayerStats.regenerationSpeed += upgradeValue;
    }

    public override string TextToDisplay()
    {
        return "Increase Regeneration Speed";
    }
}

public class ShopHealthBuyer : ShopItem
{
    protected override void UpdateButton()
    {
        base.UpdateButton();
        if (button.interactable) button.interactable = PlayerStats.health < PlayerStats.maxHealth;
    }

    public override void Buy()
    {
        PlayerStats.health++;
    }

    public override string TextToDisplay()
    {
        return "Increase Health By One";
    }
}

public class ShopWeaponUnlock : ShopItem {
    public Weapon weapon;

    public override void Buy()
    {
        WeaponManager.instance.AddWeapon(weapon);
        Destroy(gameObject);
    }

    public override string TextToDisplay()
    {
        return weapon.name;
    }
}

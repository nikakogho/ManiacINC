using UnityEngine;
using UnityEngine.UI;

public abstract class ShopItem : MonoBehaviour {
    public int price;
    public Button button;
    public Text text;
    public Text priceText;
    delegate void Updater();
    static Updater updater;
    
    [ContextMenu("Apply")]
    void Apply()
    {
        text.text = TextToDisplay();
        UpdatePriceText();
    }

    void Awake()
    {
        updater += Updating;
        UpdatePriceText();
        Updating();
    }

    void OnEnable()
    {
        UpdateButton();
    }

    protected void UpdatePriceText()
    {
        priceText.text = "$" + price;
    }
    
    void Updating()
    {
        UpdateButton();
        text.text = TextToDisplay();
    }

    protected virtual void UpdateButton()
    {
        if (button == null) return;

        button.interactable = price <= PlayerStats.money;
    }

    public void PayTheCost()
    {
        PlayerStats.money -= price;
    }

    public void UpdateIt()
    {
        updater.Invoke();
    }

    public abstract void Buy();
    public abstract string TextToDisplay();
}

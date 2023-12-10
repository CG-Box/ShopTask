using UnityEngine;

public enum ShopItemType
{   
    Chest,
    Skin,
    Location
}
public enum ShopItemState
{   
    Owned,
    Unlocked,
    Locked
}


[System.Serializable]
public class ShopItemData
{
    public string id;
    public string title;
    public int requiredLevel;

    public int bonusTickets;
    public int ticketsPrice;
    public Sprite image;

    public ShopItemType type;
    public ShopItemState state;

    public ShopItemData()
    {
        id = "SHOP_ITEM_1";
        title = "Shop Item";
        requiredLevel = 0;
        bonusTickets = 0;
        ticketsPrice = 0;
        image = null;
        type = ShopItemType.Chest;
        state = ShopItemState.Locked;
    }
    public ShopItemData(ShopItemData shopItemData)
    {
        id = shopItemData.id;
        title = shopItemData.title;
        requiredLevel = shopItemData.requiredLevel;
        bonusTickets = shopItemData.bonusTickets;
        ticketsPrice = shopItemData.ticketsPrice;
        image = shopItemData.image;
        type = shopItemData.type;
        state = shopItemData.state;
    }
}

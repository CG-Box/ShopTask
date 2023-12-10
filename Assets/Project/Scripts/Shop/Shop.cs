using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System;


public class Shop : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_StoreController;

    #region ShopPoducts
    struct chestProduct_500
    {
        public const string name = "com.mycompany.mygame.chest500";
        public const int amount = 500; 
    }
    struct chestProduct_1200
    {
        public const string name = "com.mycompany.mygame.chest1200";
        public const int amount = 1200; 
    }
    struct skinProduct_1
    {   
        public const string name = "com.mycompany.mygame.skin1";
        public const int id = 1; 
    }
    struct skinProduct_2
    {   
        public const string name = "com.mycompany.mygame.skin2";
        public const int id = 2; 
    }
    struct locationProduct_1
    {   
        public const string name = "com.mycompany.mygame.location1";
        public const int id = 1; 
    }
    struct locationProduct_2
    {   
        public const string name = "com.mycompany.mygame.location2";
        public const int id = 2; 
    }
    struct locationProduct_3
    {   
        public const string name = "com.mycompany.mygame.location3";
        public const int id = 3; 
    }
    #endregion


    public event EventHandler OnShopStart;
    public event EventHandler OnShopOpens;
    public event EventHandler OnShopCloses;

    [SerializeField]
    private GameData_SO gameDataSO;

    [SerializeField]
    private ShopItem[] shopItemList;

    void OnEnable()
    {
        OnShopOpens?.Invoke(this, EventArgs.Empty);
        UnlockItemsByLevel(gameDataSO.HighestCompletedLevel);
        AddShopItemsListeners();
    }
    void OnDisable()
    {
        OnShopCloses?.Invoke(this, EventArgs.Empty);
        RemoveShopItemsListeners();
    }

    void Start()
    {
        OnShopStart?.Invoke(this, EventArgs.Empty);
        InitializePurchasing();
        //UpdateUI();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(chestProduct_500.name, ProductType.Consumable);
        builder.AddProduct(chestProduct_1200.name, ProductType.Consumable);
        builder.AddProduct(skinProduct_1.name, ProductType.NonConsumable);
        builder.AddProduct(skinProduct_2.name, ProductType.NonConsumable);
        builder.AddProduct(locationProduct_1.name, ProductType.NonConsumable);
        builder.AddProduct(locationProduct_2.name, ProductType.NonConsumable);
        builder.AddProduct(locationProduct_3.name, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    void AddShopItemsListeners()
    {
        foreach(ShopItem shopItem in shopItemList)
        {
            shopItem.OnBuyingFromTickets += Shop_OnBuyingFromTickets;
        }
    }
    void RemoveShopItemsListeners()
    {
        foreach(ShopItem shopItem in shopItemList)
        {
            shopItem.OnBuyingFromTickets -= Shop_OnBuyingFromTickets;
        }
    }

    public void Shop_OnBuyingFromTickets(object sender, EventArgs eventArgs)
    {
        ShopItem shopItem = (ShopItem)sender;
        if(shopItem.data.ticketsPrice <= gameDataSO.Tickets)
        {
            gameDataSO.RemoveTickets(shopItem.data.ticketsPrice);
            shopItem.OwnItem();
            StaticEvents.ShopSystem.PurchaseFromTicketsComplete?.Invoke();
            gameDataSO.OwnShopItem(shopItem.data.id);
        }
        else
        {
            StaticEvents.ShopSystem.PurchaseFromTicketsDeclined?.Invoke();
        }
    }

    #region ProductsBuys
    public void BuyChest500()
    {
        m_StoreController.InitiatePurchase(chestProduct_500.name);
    }

    public void BuyChest1200()
    {
        m_StoreController.InitiatePurchase(chestProduct_1200.name);
    }
    public void BuySkin1()
    {
        m_StoreController.InitiatePurchase(skinProduct_1.name);
    }
    public void BuySkin2()
    {
        m_StoreController.InitiatePurchase(skinProduct_2.name);
    }
    public void BuyLocation1()
    {
        m_StoreController.InitiatePurchase(locationProduct_1.name);
    }
    public void BuyLocation2()
    {
        m_StoreController.InitiatePurchase(locationProduct_2.name);
    }
    public void BuyLocation3()
    {
        m_StoreController.InitiatePurchase(locationProduct_3.name);
    }
    #endregion

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);

        StaticEvents.ShopSystem.PurchaseFailed?.Invoke();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        StaticEvents.ShopSystem.PurchaseCompleted?.Invoke();

        //Retrieve the purchased product
        var product = args.purchasedProduct;

        string definitionId = product.definition.id;
        switch (definitionId)
        {
            case chestProduct_500.name:
                AddTickets(chestProduct_500.amount);
                break;
            case chestProduct_1200.name:
                AddTickets(chestProduct_1200.amount);
                break;
            case skinProduct_1.name:
                UnlockSkin(skinProduct_1.id);
                break;
            case skinProduct_2.name:
                UnlockSkin(skinProduct_2.id);
                break;
            case locationProduct_1.name:
                UnlockLocation(locationProduct_1.id);
                break;
            case locationProduct_2.name:
                UnlockLocation(locationProduct_2.id);
                break;
            case locationProduct_3.name:
                UnlockLocation(locationProduct_3.id);
                break;
        }

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");

        StaticEvents.ShopSystem.PurchaseFailed?.Invoke();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }

    void AddTickets(int amount)
    {
        StaticEvents.Tickets.Added?.Invoke(amount);
    }

    void UnlockSkin(int id)
    {
        //UpdateUI();
    }
    void UnlockLocation(int id)
    {
        //UpdateUI();
    }

    void UnlockItemsByLevel(int levelNumber)
    {
        ShopItem[] lockedShopItems = GetLockedItems();
        ShopItem[] requiredLevelShopItems = Array.FindAll(lockedShopItems, item => item.data.requiredLevel <= levelNumber);
        foreach(ShopItem shopItem in requiredLevelShopItems)
        {
            shopItem.UnlockItem();
        }

        if(requiredLevelShopItems.Length != 0)
        {
            StaticEvents.ShopSystem.NewItemsUnlocked?.Invoke(requiredLevelShopItems.Length);
        }
    }
    public void OwnItemsByIds(string[] ownedIds)
    {
        ShopItem shopItem;
        foreach(string itemId in ownedIds)
        {
            shopItem = Array.Find(shopItemList, item => item.data.id == itemId);
            shopItem.OwnItem();
        }
    }

    ShopItem[] GetLockedItems()
    {
        ShopItem[] shopItems = Array.FindAll(shopItemList, item => item.data.state == ShopItemState.Locked);
        return shopItems;
    }

    void UpdateUI()
    {}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text itemTitle;

    [SerializeField]
    private Button moneyButton;
    
    [SerializeField]
    private Button ticketButton;
    
    [SerializeField]
    private GameObject ownButton;

    [SerializeField]
    private GameObject bonusInfo;

    [SerializeField]
    private TMP_Text bonusText;

    [SerializeField]
    private TMP_Text requiredText;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private GameObject imageBlocker;

    const string currencySign = "$";
    const string bonuxPrefix = "x";
    const string requiredPrefix = "LV. ";

    public ShopItemData data;

    public event EventHandler OnBuyingFromTickets;

    #region EventsBinding
    void OnEnable()
    {
        ticketButton.onClick.AddListener(TicketButtonClick);
        moneyButton.onClick.AddListener(MoneyButtonClick);
    }
    void OnDisable()
    {
        ticketButton.onClick.RemoveListener(TicketButtonClick);
        moneyButton.onClick.RemoveListener(MoneyButtonClick);
    }
    #endregion

    void Awake()
    {
       //SetData(new ShopItemData());
       UpdateVisual();
    }

    public void TicketButtonClick()
    {
        OnBuyingFromTickets?.Invoke(this, EventArgs.Empty);
        //StaticEvents.LevelUI.LockedButtonPressed?.Invoke();
    }

    public void MoneyButtonClick()
    {
        //Nothing();
    }

    public void DisableButtons()
    {
        ticketButton.interactable = false;
        moneyButton.interactable = false;
        //ticketButton.enabled = false;
        //moneyButton.enabled = false;
    }
    public void EnableButtons()
    {
        ticketButton.interactable = true;
        moneyButton.interactable = true;
        //ticketButton.enabled = true;
        //moneyButton.enabled = true;
    }
    public void UnlockItem()
    {
        data.state = ShopItemState.Unlocked;
        UpdateVisual();
    }
    public void LockItem()
    {
        data.state = ShopItemState.Locked;
        UpdateVisual();
    }
    public void OwnItem()
    {
        data.state = ShopItemState.Owned;
        UpdateVisual();
    }

    public void SetRequiredLevel(int level)
    {
        data.requiredLevel = level;
        UpdateVisual();
    }

    public void SetData(ShopItemData newData)
    {
        data = new ShopItemData(newData);
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        itemTitle.text = data.title;
        requiredText.text = $"{requiredPrefix}{data.requiredLevel}";
        bonusText.text = $"{bonuxPrefix}{data.bonusTickets}";

        itemImage.sprite = data.image;

        switch (data.type)
        {
            case ShopItemType.Chest:
                //chest
                ShowBonusInfo();
                HideTicketButton();
                ShowMoneyButton();
                break;
            case ShopItemType.Skin:
                //skin
                HideBonusInfo();
                ShowTicketButton();
                HideMoneyButton();
                break;
            case ShopItemType.Location:
                //location
                HideBonusInfo();
                ShowTicketButton();
                HideMoneyButton();
                break;
        }
        switch (data.state)
        {
            case ShopItemState.Owned:
                HideBlocker();
                HideRequiredText();
                DisableButtons();
                ShowOwnButton();
                break;
            case ShopItemState.Unlocked:
                HideBlocker();
                HideRequiredText();
                EnableButtons();
                HideOwnButton();
                break;
            case ShopItemState.Locked:
                ShowBlocker();
                ShowRequiredText();
                DisableButtons();
                HideOwnButton();
                break;
        }
    }

    #region HideAndShowHelpers
    void HideBlocker()
    {
        imageBlocker.SetActive(false);
        requiredText.gameObject.SetActive(false);
    }
    void ShowBlocker()
    {
        imageBlocker.SetActive(true);
        requiredText.gameObject.SetActive(true);
    }
    void ShowOwnButton()
    {
        ownButton.SetActive(true);
    }
    void HideOwnButton()
    {
        ownButton.SetActive(false);
    }
    void ShowTicketButton()
    {
        ticketButton.gameObject.SetActive(true);
    }
    void HideTicketButton()
    {
        ticketButton.gameObject.SetActive(false);
    }
    void ShowMoneyButton()
    {
        moneyButton.gameObject.SetActive(true);
    }
    void HideMoneyButton()
    {
        moneyButton.gameObject.SetActive(false);
    }
    void ShowRequiredText()
    {
        requiredText.gameObject.SetActive(true);
    }
    void HideRequiredText()
    {
        requiredText.gameObject.SetActive(false);
    }
    void ShowBonusInfo()
    {
        bonusInfo.SetActive(true);
    }
    void HideBonusInfo()
    {
        bonusInfo.SetActive(false);
    }
    #endregion
}
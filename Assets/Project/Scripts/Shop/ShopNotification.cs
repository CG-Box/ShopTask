using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNotification : MonoBehaviour
{
    [SerializeField]
    private GameObject notification;

    [SerializeField]
    private TMP_Text unlockedAmount;
    Button shopButton;
    int amountOfClicksAfterUpdate = 0;
    IEnumerator hideCoroutine;

    void Awake()
    {
        shopButton = gameObject.GetComponent<Button>();
    }

    void OnEnable()
    {
        StaticEvents.ShopSystem.NewItemsUnlocked += UpdateNotificationUI;
        shopButton.onClick.AddListener(TryHideCoroutine);
    }
    void OnDisable()
    {
        StaticEvents.ShopSystem.NewItemsUnlocked -= UpdateNotificationUI;
        shopButton.onClick.RemoveListener(TryHideCoroutine);
    }

    public void UpdateNotificationUI(int amount)
    {
        amountOfClicksAfterUpdate = 0;
        //StopCoroutine(hideCoroutine);
        unlockedAmount.text = amount.ToString(); 
        ShowNotification();
    }

    public void TryHideCoroutine()
    {  
        amountOfClicksAfterUpdate++;
        hideCoroutine = DelayedHide(0.5f);
        StartCoroutine(hideCoroutine);
    }
    public void HideNotification()
    {
        notification.SetActive(false);
    }
    public void ShowNotification()
    {
        notification.SetActive(true);
    }

    public IEnumerator DelayedHide(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(amountOfClicksAfterUpdate>1)
            HideNotification();
    }
}

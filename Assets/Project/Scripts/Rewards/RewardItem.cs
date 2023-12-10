using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RewardItem : MonoBehaviour, IReward
{
    [SerializeField]
    private TMP_Text dayText;
    
    [SerializeField]
    private TMP_Text bonusText;

    [SerializeField]
    private Image rewardImage;

    [SerializeField]
    private GameObject blockerObject;

    [SerializeField]
    private GameObject collectedObject;

    public RewardData data;
    public RewardData Data
    {
        get { return data; }
    }

    Button rewardButton;

    public event EventHandler OnRewardClicked;

    const string bonuxPrefix = "x";

    void Awake()
    {
        rewardButton = gameObject.GetComponent<Button>();
        UpdateVisual();
    }

    void OnEnable()
    {
        rewardButton.onClick.AddListener(RewardOnClick);
    }
    void OnDisable()
    {
        rewardButton.onClick.RemoveListener(RewardOnClick);
    }

    void RewardOnClick()
    {
        OnRewardClicked?.Invoke(this, EventArgs.Empty);
    }

    public void UnlockItem()
    {
        data.state = RewardState.Unlocked;
        UpdateVisual();
    }
    public void LockItem()
    {
        data.state = RewardState.Locked;
        UpdateVisual();
    }
    public void CollectItem()
    {
        data.state = RewardState.Сollected;
        UpdateVisual();
    }

    public void SetData(RewardData newData)
    {
        data = new RewardData(newData);
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        dayText.text = data.title;
        bonusText.text = $"{bonuxPrefix}{data.bonusTickets}";

        switch (data.state)
        {
            case RewardState.Сollected:
                HideBlocker();
                //DisableButtons();
                ShowCollectedBlocker();
                break;
            case RewardState.Unlocked:
                HideBlocker();
                //EnableButtons();
                HideCollectedBlocker();
                break;
            case RewardState.Locked:
                ShowBlocker();
                //DisableButtons();
                HideCollectedBlocker();
                break;
        }
    }

    public void DisableButtons()
    {
        //rewardButton.interactable = false;
        rewardButton.enabled = false;
    }
    public void EnableButtons()
    {
        //rewardButton.interactable = true;
        rewardButton.enabled = true;
    }

    void HideBlocker()
    {
        blockerObject.SetActive(false);
    }
    void ShowBlocker()
    {
        blockerObject.SetActive(true);
    }

    void HideCollectedBlocker()
    {
        collectedObject.SetActive(false);
    }
    void ShowCollectedBlocker()
    {
        collectedObject.SetActive(true);
    }
}

public enum RewardState
{   
    Сollected,
    Unlocked,
    Locked
}


[System.Serializable]
public class RewardData
{
    public int index;
    public string title;
    public int bonusTickets;

    public RewardState state;

    public RewardData()
    {
        index = 1;
        title = "Day0";
        bonusTickets = 0;
        state = RewardState.Locked;
    }
    public RewardData(RewardData rewardData)
    {
        index = rewardData.index;
        title = rewardData.title;
        state = rewardData.state;
        bonusTickets = rewardData.bonusTickets;
    }
}


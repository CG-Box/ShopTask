using System;
using UnityEngine;
using UnityEngine.UI;

public class FinalRewardItem : MonoBehaviour, IReward
{
    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private Animator animator;

    const string pulsingOn = "Unlocked";
    const string pulsingOff = "Locked";

    public RewardData data;
    public RewardData Data
    {
        get { return data; }
    }

    public event EventHandler OnRewardClicked;


    void Awake()
    {
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

    public void CollectItem()
    {
        data.state = RewardState.Сollected;
        UpdateVisual();
    }

    public void LockItem()
    {
        data.state = RewardState.Locked;
        UpdateVisual();
    }

    public void SetData(RewardData newData)
    {
        data = new RewardData(newData);
        UpdateVisual();
    }

    public void UnlockItem()
    {
        data.state = RewardState.Unlocked;
        UpdateVisual();
    }

    public void StartPulsing()
    {
        if (animator != null && animator.isActiveAndEnabled)
            animator.SetTrigger(pulsingOn);
    }
    public void StopPulsing()
    {
        if (animator != null && animator.isActiveAndEnabled)
            animator.SetTrigger(pulsingOff);
    }

    public void UpdateVisual()
    {
        switch (data.state)
        {
            case RewardState.Сollected:
                DisableButtons();
                StopPulsing();
                break;
            case RewardState.Unlocked:
                EnableButtons();
                StartPulsing();
                break;
            case RewardState.Locked:
                DisableButtons();
                StopPulsing();
                break;
        }
    }
}

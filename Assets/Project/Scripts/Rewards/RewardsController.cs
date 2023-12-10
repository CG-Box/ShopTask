using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsController : MonoBehaviour
{
    [SerializeField]
    private Slider rewardsSlider;

    [SerializeField]
    private TMP_Text sliderText;

    [SerializeField]
    private float rewardAvailablePeriod = 86400f; //24h seconds
    int rewardInRow = 0;

    [SerializeField]
    private GameObject[] rewardItemGOList;

    IReward[] rewardItemList;

    float timeRemaining;

    void Awake()
    {
        ConvertRewardsGOtoInterface();
    }
    void OnEnable()
    {
        AddRewardsListeners();
    }
    void OnDisable()
    {
        RemoveRewardsListeners();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        timeRemaining = rewardAvailablePeriod;
        LockAllRewards();
        UnlockRewardByIndex(1);
    }

    void ConvertRewardsGOtoInterface()
    {
        rewardItemList = new IReward[rewardItemGOList.Length];
        for(int i = 0; i < rewardItemList.Length; i++ )
        {
            rewardItemList[i] = rewardItemGOList[i].GetComponent<IReward>();
        }
    }

    void AddRewardsListeners()
    {
        foreach(IReward rewardItem in rewardItemList)
        {
            rewardItem.OnRewardClicked += RewardsController_OnRewardClicked;
        }
    }
    void RemoveRewardsListeners()
    {
        foreach(IReward rewardItem in rewardItemList)
        {
            rewardItem.OnRewardClicked -= RewardsController_OnRewardClicked;
        }
    }

    void RewardsController_OnRewardClicked(object sender, EventArgs eventArgs)
    {
        IReward rewardItem = (IReward)sender;
        RewardState currentRewardState = rewardItem.Data.state;
        switch(currentRewardState)
        {
            case RewardState.Сollected:
                StaticEvents.Rewards.CollectedClick?.Invoke();
                break;
            case RewardState.Unlocked:
                StaticEvents.Tickets.Added?.Invoke(rewardItem.Data.bonusTickets);
                rewardItem.CollectItem();
                SetSliderValue(rewardItem.Data.index);
                //StaticEvents.Rewards.Collected?.Invoke(rewardItem.Data.index, GetCurrentTime());
                StaticEvents.Rewards.UnlockedClick?.Invoke();
                break;
            case RewardState.Locked:
                StaticEvents.Rewards.LockedClick?.Invoke();
                break;
        }
    }

    void SetSliderValue(float newValue)
    {
        rewardsSlider.value = newValue;
        sliderText.text = $"{newValue}/{rewardsSlider.maxValue}"; 
    }

    void DayPassed()
    {
        if(IsAllUnlockedRewardsCollected())
        {
            WeekRewardsCollected weekState = TryUnlockNextRewardAfterCollected();
            switch(weekState)
            {
                case WeekRewardsCollected.Zero:
                    rewardInRow = 0;
                    //Debug.Log("No rewads collected");
                    LockAllRewards();
                    UnlockRewardByIndex(1);
                    break;
                case WeekRewardsCollected.Seven:
                    //Debug.Log("Open 7 chest");
                    LockAllRewards();
                    UnlockRewardByIndex(1);
                    break;
                 case WeekRewardsCollected.BetweenOneAndSix:
                    rewardInRow++;
                    //Debug.Log("Nothing to do it's all ok");
                    break;
            }
        }
        else
        {
            rewardInRow = 0;
            LockAllRewards();
            UnlockRewardByIndex(1);
            SetSliderValue(0f);
        }
    }

    void LockAllRewards()
    {
        foreach(IReward rewardItem in rewardItemList)
        {
            rewardItem.LockItem();
        }
    }

    void UnlockRewardByIndex(int rewardIndex)
    {
        IReward unlockedReward = Array.Find(rewardItemList, reward => reward.Data.index == rewardIndex);
        if(unlockedReward != null)
            unlockedReward.UnlockItem();
    }
    
    WeekRewardsCollected TryUnlockNextRewardAfterCollected()
    {
        IReward lastCollectedItem = GetLastRewardItemByState(RewardState.Сollected);
        if(lastCollectedItem == null)
        {
            return WeekRewardsCollected.Zero;
        }
        else if(lastCollectedItem.Data.index == 7)
        {
            return WeekRewardsCollected.Seven;
        }
        else
        {
            UnlockRewardByIndex(lastCollectedItem.Data.index+1);
            return WeekRewardsCollected.BetweenOneAndSix;
        }
    }

    bool IsAllUnlockedRewardsCollected()
    {
        IReward lastUnlockedItem = GetLastRewardItemByState(RewardState.Unlocked);

        return lastUnlockedItem == null;
    }

    long GetCurrentTime()
    {
        return DateTime.Now.ToBinary();
    }

    IReward GetLastRewardItemByState(RewardState lastItemState)
    {
        IReward lastCollectedItem = null;
        foreach(IReward rewardItem in rewardItemList)
        {
            if(rewardItem.Data.state == lastItemState)
            {
                lastCollectedItem = rewardItem;
            }
        }

        if(lastCollectedItem == null)
        {
            //Debug.Log($"No rewards {lastItemState.ToString()}");
        }

        return lastCollectedItem;
    }


    void Update()
    {
        UpdateTime();
    }
    void UpdateTime()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = rewardAvailablePeriod;
            DayPassed();
        }
    }
}

public enum WeekRewardsCollected
{   
    Zero,
    BetweenOneAndSix,
    Seven
}

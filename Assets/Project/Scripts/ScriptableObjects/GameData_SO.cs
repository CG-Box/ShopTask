using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData_SO", menuName = "SriptableObjects/GameData_SO", order = 2)]
public class GameData_SO: ScriptableObject
{
    [SerializeField]
    private int tickets;
    public int Tickets
    {
        get { return tickets; }
    }

    [SerializeField]
    private int highestCompletedLevel;
    public int HighestCompletedLevel
    {
        get { return highestCompletedLevel; }
    }
    private long lastRewardGrab;
    public long LastRewardGrab
    {
        get { return lastRewardGrab; }
    }

    int ownedItemsAmount;
    public List<string> ownedItemsId;

    //public List<TreasureChest> treasureChestList;
    //public List<CharacterSkin> characterSkinList;
    //public List<Locations> locationsList;

    void OnEnable()
    {
        StaticEvents.Tickets.Added += AddTickets;
        StaticEvents.LevelUI.LevelCompleted += TrySetHighestCompletedLevel;
        StaticEvents.Rewards.Collected += SetLastRewardTime;
        Init();

    }
    void OnDisable()
    {
        StaticEvents.Tickets.Added -= AddTickets;
        StaticEvents.LevelUI.LevelCompleted -= TrySetHighestCompletedLevel;
        StaticEvents.Rewards.Collected -= SetLastRewardTime;
    }

    void Init()
    {
        if(IsGameDataInPrefs())
        {
            GetGameDataFromPrefs();
        }
        else
        {
            InitDefaultGameData();
        }
        UpdateTicketsUI();
    }

    public void InitDefaultGameData() 
    {
        tickets = 100;

        highestCompletedLevel = 0;

        ownedItemsAmount = 0;
        ownedItemsId = new List<string>(ownedItemsAmount);
    }

    public bool IsGameDataInPrefs()
    {
        return PlayerPrefs.HasKey(nameof(tickets));
    }
    public void GetGameDataFromPrefs()
    {
        tickets = PlayerPrefs.GetInt(nameof(tickets));
        highestCompletedLevel = PlayerPrefs.GetInt(nameof(highestCompletedLevel));

        GetShopItemsFromPrefs();
    }
    public void SaveGameDataToPrefs()
    {
        PlayerPrefs.SetInt(nameof(tickets), tickets);
        PlayerPrefs.SetInt(nameof(highestCompletedLevel), highestCompletedLevel);

        SaveShopItemsToPrefs();

        PlayerPrefs.Save();
    }

    public void SaveShopItemsToPrefs()
    {
        for (int i = 0; i < ownedItemsId.Count; i++) {
            PlayerPrefs.SetString($"{nameof(ownedItemsAmount)}_{i}", ownedItemsId[i]);
        }
        PlayerPrefs.SetInt(nameof(ownedItemsAmount), ownedItemsId.Count);
    }
    public void GetShopItemsFromPrefs()
    {
        ownedItemsAmount = PlayerPrefs.GetInt(nameof(ownedItemsAmount));
        ownedItemsId = new List<string>();
        for (int i = 0; i < ownedItemsAmount; i++) {
            ownedItemsId.Add(PlayerPrefs.GetString($"{nameof(ownedItemsAmount)}_{i}"));
        }
    }

    public void AddTickets(int amount)
    {
        tickets += amount;
        SaveGameDataToPrefs();
        UpdateTicketsUI();
    }
    public void RemoveTickets(int amount)
    {
        tickets -= amount;
        SaveGameDataToPrefs();
        UpdateTicketsUI();
    }
    public void UpdateTicketsUI()
    {
        StaticEvents.Tickets.UpdateUI?.Invoke(tickets);
    }

    public void TrySetHighestCompletedLevel(int levelNumer)
    {
        if(levelNumer > highestCompletedLevel)
            SetHighestCompletedLevel(levelNumer);
    }
    public void SetHighestCompletedLevel(int levelNumer)
    {
        highestCompletedLevel = levelNumer;
        SaveGameDataToPrefs();
    }

    public void SetLastRewardTime(int index, long rewardTime)
    {
        //Debug.Log($"rewardTime : {rewardTime}");
    }

    public void OwnShopItem(string id)
    {
        ownedItemsId.Add(id);
        ownedItemsAmount = ownedItemsId.Count;
        SaveGameDataToPrefs();
    }
    public void ResetToDefault()
    {
        DeleteAllPlayerPrefs();
        InitDefaultGameData();
    }
    
    [ContextMenu("DeleteAllPlayerPrefs")]
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    //void OnValidate(){} //Every change in SO
}

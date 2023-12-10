using UnityEngine;
using System;


public class LevelsMap : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSlotTemplate;

    private int slotsAmount = 20;

    [SerializeField]
    private LevelSlot[] levelSelectionList;

    public event EventHandler OnCompleteAllLevels;
    public event EventHandler OnLevelMapStart;
    public event EventHandler OnLevelMapOpens;
    public event EventHandler OnLevelMapCloses;

    int levelCompleteTickets = 100;

    void Start()
    {
        //SpawnLevelSlots();
        OnLevelMapStart?.Invoke(this, EventArgs.Empty);
        InitializeLevelSlots();
    }

    void OnEnable()
    {
        OnLevelMapOpens?.Invoke(this, EventArgs.Empty);
        //InitializeLevelSlots();
    }
    void OnDisable()
    {
        OnLevelMapCloses?.Invoke(this, EventArgs.Empty);
        //ClearLevelSlots();
    }

    void SpawnLevelSlots()
    {
        levelSelectionList = new LevelSlot[slotsAmount];

        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        GameObject levelSlotGameObject;
        LevelSlot levelSlot;
        for(int i = 0; i < slotsAmount; i++)
        {
            levelSlotGameObject = Instantiate(levelSlotTemplate, this.gameObject.transform);
            levelSlot = levelSlotGameObject.GetComponent<LevelSlot>();
            levelSlot.SetLevelSlotNumber(i+1);
            levelSelectionList[i] = levelSlot;

            if(i == 0)
                levelSlot.UnlockLevel();
        }
    }

    void InitializeLevelSlots()
    {
        //CompleteAllLevelsTillNumber(UnlockAAA);
        foreach(LevelSlot levelSlot in levelSelectionList)
        {
            AddLevelSlotListeners(levelSlot);
        }
    }
    void ClearLevelSlots()
    {
        foreach(LevelSlot levelSlot in levelSelectionList)
        {
            RemoveLevelSlotListeners(levelSlot);
        }
    }

    private void AddLevelSlotListeners(LevelSlot levelSlot)
    {

        levelSlot.OnUnlockLevelSlot += LevelsMap_OnUnlockLevelSlot;
        levelSlot.OnLockLevelSlot += LevelsMap_OnLockLevelSlot;
        levelSlot.OnCompleteLevelSlot += LevelsMap_OnCompleteLevelSlot;
    }
    private void RemoveLevelSlotListeners(LevelSlot levelSlot)
    {

        levelSlot.OnUnlockLevelSlot -= LevelsMap_OnUnlockLevelSlot;
        levelSlot.OnLockLevelSlot -= LevelsMap_OnLockLevelSlot;
        levelSlot.OnCompleteLevelSlot -= LevelsMap_OnCompleteLevelSlot;
    }

    void OpenNextLevelSlot(LevelSlot currentLevelSlot)
    {
        LevelSlot nextLevelSlot = GetNextLevelSlot(currentLevelSlot);
        //LevelSlot prevLevelSlot = GetPrevLevelSlot(levelSlot);

        bool nextIsLocked = false;
        if(nextLevelSlot != null)
            nextIsLocked = nextLevelSlot.State == LevelState.Locked;
        if(nextIsLocked)
            UnlockLevelSlot(nextLevelSlot);
    }
    private void LevelsMap_OnCompleteLevelSlot(object sender, LevelSlotEventArgs levelEventArgs)
    {
        //StaticEvents.Tickets.Added?.Invoke(levelCompleteTickets);

        if(IsAllLevelsCompleted())
        {
            OnCompleteAllLevels?.Invoke(this, EventArgs.Empty);
            StaticEvents.LevelUI.AllLevelsCompleted?.Invoke();
            return;
        }

        LevelSlot completedLevelSlot = (LevelSlot)sender;

        OpenNextLevelSlot(completedLevelSlot);

        StaticEvents.LevelUI.LevelCompleted?.Invoke(completedLevelSlot.Number);
    }
    private void LevelsMap_OnUnlockLevelSlot(object sender, LevelSlotEventArgs eventArgs)
    {

        //Debug.Log(eventArgs.number);     
    }
    private void LevelsMap_OnLockLevelSlot(object sender, LevelSlotEventArgs eventArgs)
    {
        //Debug.Log(eventArgs.number);
    }

    public void UnlockLevelSlot(LevelSlot levelSlot)
    {
        if(levelSlot == null)
        {
            Debug.Log("LevelSlot not found");
            return;
        }
        levelSlot.UnlockLevel(false);
    }
    public void LockLevelSlot(LevelSlot levelSlot)
    {
        if(levelSlot == null)
        {
            Debug.Log("LevelSlot not found");
            return;
        }
        levelSlot.LockLevel(false);
    }
    public void UnlockLevelSlot(int number)
    {
        LevelSlot levelSlot = GetLevelSlotByNumber(number);
        UnlockLevelSlot(levelSlot);
    }
    public void LockLevelSlot(int number)
    {
        LevelSlot levelSlot = GetLevelSlotByNumber(number);
        LockLevelSlot(levelSlot);
    }

    public LevelSlot GetLevelSlotByNumber(int number)
    {
        LevelSlot levelSlot = Array.Find(levelSelectionList, level => level.Number == number);
        return levelSlot;
    }
    public LevelSlot GetNextLevelSlot(LevelSlot levelSlot)
    {  
        bool isInRange = LevelSlotIsInRange(levelSlot);
        if(!isInRange)
        {
            Debug.Log("LevelSlot is not in LevelsMap range");
            return null;
        }
       
        if(levelSlot.Number == levelSelectionList.Length || levelSelectionList.Length < 2)
        {
            //Debug.Log("LevelSlot is the last level in LevelsMap range");
            return null;
        }
        return levelSelectionList[levelSlot.Number];
    }
    public LevelSlot GetPrevLevelSlot(LevelSlot levelSlot)
    {
        bool isInRange = LevelSlotIsInRange(levelSlot);
        if(!isInRange)
        {
            Debug.Log("LevelSlot is not in LevelsMap range");
            return null;
        }
       
        if(levelSlot.Number == 1)
        {
            Debug.Log("LevelSlot is the first level in LevelsMap range");
            return null;
        }
        return levelSelectionList[levelSlot.Number-2];
    }
    bool LevelSlotIsInRange(LevelSlot levelSlot)
    {
        LevelSlot currentLevelSlot = Array.Find(levelSelectionList, level => level.Number == levelSlot.Number);
        return currentLevelSlot != null;
    }

    bool IsAllLevelsCompleted()
    {
        int completedAmount = 0;
        foreach(LevelSlot levelSlot in levelSelectionList)
        {
            if(levelSlot.State == LevelState.Completed)
                completedAmount++;
        }
        return levelSelectionList.Length == completedAmount;
    }

    public void CompleteAllLevelsTillNumber(int levelNumber)
    {
        if(levelNumber == 0)
            return;

        if(levelNumber > levelSelectionList.Length)
        {
            Debug.LogWarning("level number is out of range");
            levelNumber = levelSelectionList.Length;
        }
        for (int i = 0; i < levelNumber; i++)
        {
            levelSelectionList[i].UnlockLevel(false);
            levelSelectionList[i].CompleteLevel(false);
        }
        OpenNextLevelSlot(levelSelectionList[levelNumber-1]);
    }
}

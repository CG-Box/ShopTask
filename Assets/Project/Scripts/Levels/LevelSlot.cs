using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LevelSlot : MonoBehaviour
{
    [SerializeField]
    private Button levelOpenButton;

    [SerializeField]
    private Button levelLockedButton;
    
    [SerializeField]
    private Button levelCompleteButton;

    [SerializeField]
    private TMP_Text openTextNumber;
    [SerializeField]
    private TMP_Text completeTextNumber;

    [SerializeField]
    private int number;
    public int Number { get { return number; } }
    public LevelState State
    { 
        get
        { 
            LevelState state;
            if(levelLockedButton.gameObject.activeSelf)
            {
                state = LevelState.Locked;
            }
            else if(levelOpenButton.gameObject.activeSelf)
            {
                state = LevelState.Unlocked;
            }
            else
            {
                state = LevelState.Completed;
            }
            return state;   
        } 
    }

    public event EventHandler<LevelSlotEventArgs> OnUnlockLevelSlot;
    public event EventHandler<LevelSlotEventArgs> OnLockLevelSlot;
    public event EventHandler<LevelSlotEventArgs> OnCompleteLevelSlot;

    #region EventsBinding
    void OnEnable()
    {
        levelOpenButton.onClick.AddListener(LevelOpenClick);
        levelLockedButton.onClick.AddListener(LockedButtonClick);
        levelCompleteButton.onClick.AddListener(LevelCompleteClick);
    }
    void OnDisable()
    {
        levelOpenButton.onClick.RemoveListener(LevelOpenClick);
        levelLockedButton.onClick.RemoveListener(LockedButtonClick);
        levelCompleteButton.onClick.RemoveListener(LevelCompleteClick);
    }
    #endregion

    void Awake()
    {
        UpdateNumber();
    }

    public void UpdateNumber()
    {
        openTextNumber.text = number.ToString(); 
        completeTextNumber.text = number.ToString(); 
    }
    public void SetLevelSlotNumber(int newNumber)
    {
        number = newNumber;
        UpdateNumber();
    }

    public void UnlockLevel(bool invokeEvent = true)
    {
        levelLockedButton.gameObject.SetActive(false);
        levelOpenButton.gameObject.SetActive(true);
        if(invokeEvent)
            OnUnlockLevelSlot?.Invoke(this, new LevelSlotEventArgs(number));
    }
    public void LockLevel(bool invokeEvent = true)
    {
        levelLockedButton.gameObject.SetActive(true);
        levelOpenButton.gameObject.SetActive(true);
        if(invokeEvent)
            OnLockLevelSlot?.Invoke(this, new LevelSlotEventArgs(number));
    }
    public void CompleteLevel(bool invokeEvent = true)
    {
        levelLockedButton.gameObject.SetActive(false);
        levelOpenButton.gameObject.SetActive(false);
        if(invokeEvent)
            OnCompleteLevelSlot?.Invoke(this, new LevelSlotEventArgs(number));
    }


    public void LockedButtonClick()
    {
        //UnlockLevel();
        //StaticEvents.LevelUI.LockedButtonPressed?.Invoke();
    }

    public void LevelOpenClick()
    {
        CompleteLevel();
        //StaticEvents.LevelUI.UnlockedButtonPressed?.Invoke();
    }

    public void LevelCompleteClick()
    {
        //LockLevel();
        //StaticEvents.LevelUI.CompleteButtonPressed?.Invoke();
    }
}

public enum LevelState
{   
    Locked,
    Unlocked,
    Completed
}
public class LevelSlotEventArgs : EventArgs
{
    public int number;
    public LevelSlotEventArgs(int number)
    {
        this.number = number;
    }
}

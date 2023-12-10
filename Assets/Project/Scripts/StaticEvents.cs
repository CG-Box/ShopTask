using System;
using UnityEngine;

public class StaticEvents 
{
    public class Tickets
    {
        public static Action<int> Added;
        public static Action<int> Removed;
        public static Action<int> UpdateUI;
    }

    public class ShopSystem
    {
        public static Action PurchaseFromTicketsComplete;
        public static Action PurchaseFromTicketsDeclined;
        public static Action PurchaseCompleted;
        public static Action PurchaseFailed;
        public static Action<int> NewItemsUnlocked;
    }

    public class LevelUI 
    {
        public static Action LockedButtonPressed;
        public static Action UnlockedButtonPressed;
        public static Action CompleteButtonPressed;
        public static Action<int> LevelCompleted;

        public static Action AllLevelsCompleted;
    }

}
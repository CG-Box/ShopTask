using System;
interface IReward
{
    RewardData Data { get; }

    event EventHandler OnRewardClicked;
    void UnlockItem();
    void LockItem();
    void CollectItem();
    void EnableButtons();
    void DisableButtons();
    void UpdateVisual();
    void SetData(RewardData newData);
}
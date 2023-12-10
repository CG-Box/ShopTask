using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameData_SO gameDataSO;

    [SerializeField]
    private LevelsMap levelsMap;

    [SerializeField]
    private Shop shop;

    #region Singleton
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<GameManager>();
            if (instance != null)
                return instance;
                
            Create ();
            return instance;
        }
    }
    public static GameManager Create ()
    {
        GameObject dataManagerGameObject = new GameObject("GameManager");
        DontDestroyOnLoad(dataManagerGameObject);
        instance = dataManagerGameObject.AddComponent<GameManager>();
        return instance;
    }
    #endregion

    #region EventsBinding
    void OnEnable()
    {
        shop.OnShopStart += GameManager_OnShopStart;
        shop.OnShopOpens += GameManager_OnShopOpens;
        shop.OnShopCloses += GameManager_OnShopCloses;

        levelsMap.OnLevelMapStart += GameManager_OnLevelMapStart;
        levelsMap.OnLevelMapOpens += GameManager_OnLevelMapOpens;
        levelsMap.OnLevelMapOpens += GameManager_OnLevelMapCloses;
    }
    void OnDisable()
    {
        shop.OnShopStart -= GameManager_OnShopStart;
        shop.OnShopOpens -= GameManager_OnShopOpens;
        shop.OnShopCloses -= GameManager_OnShopCloses;

        levelsMap.OnLevelMapOpens -= GameManager_OnLevelMapOpens;
        levelsMap.OnLevelMapOpens -= GameManager_OnLevelMapCloses;
        levelsMap.OnLevelMapStart -= GameManager_OnLevelMapStart;
    }
    #endregion

    void Start()
    {
        UpdateTicketsUI();
    }

    void UpdateTicketsUI()
    {
        gameDataSO.UpdateTicketsUI();
    }

    void GameManager_OnLevelMapStart(object sender, EventArgs eventArgs)
    {
        levelsMap.CompleteAllLevelsTillNumber(gameDataSO.HighestCompletedLevel);
    }
    void GameManager_OnLevelMapOpens(object sender, EventArgs eventArgs)
    {}
    void GameManager_OnLevelMapCloses(object sender, EventArgs eventArgs)
    {}

    void GameManager_OnShopStart(object sender, EventArgs eventArgs)
    {
        shop.OwnItemsByIds(gameDataSO.ownedItemsId.ToArray());
    }
    void GameManager_OnShopOpens(object sender, EventArgs eventArgs)
    {
        Invoke(nameof(UpdateTicketsUI),0.1f);
    }
    void GameManager_OnShopCloses(object sender, EventArgs eventArgs)
    {}
}

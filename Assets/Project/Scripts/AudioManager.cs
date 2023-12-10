using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic("Fluffing");
    }

    #region EventsBinding
    void OnEnable()
    {
        StaticEvents.LevelUI.LockedButtonPressed += PlayLevelButtonLockedSfx;
        StaticEvents.LevelUI.UnlockedButtonPressed += PlayLevelCompleteButtonSfx;
        StaticEvents.LevelUI.CompleteButtonPressed += PlayLevelButtonSfx;

        StaticEvents.LevelUI.AllLevelsCompleted += PlayLevelsWinSfx;

        StaticEvents.ShopSystem.PurchaseCompleted += PlayPurchaseCompleteSfx;
        StaticEvents.ShopSystem.PurchaseFailed += PlayPurchaseFailedSfx;
        StaticEvents.ShopSystem.PurchaseFromTicketsComplete += PlayPurchaseCompleteSfx;
        StaticEvents.ShopSystem.PurchaseFromTicketsDeclined += PlayPurchaseFromTicketsDeclinedSfx;

        StaticEvents.Rewards.LockedClick += PlayRewardBlockedSfx;
        StaticEvents.Rewards.UnlockedClick += PlayRewardUnlockedSfx;
        StaticEvents.Rewards.CollectedClick += PlayRewardCollectedSfx;
        
    }
    void OnDisable()
    {
        StaticEvents.LevelUI.LockedButtonPressed -= PlayLevelButtonLockedSfx;
        StaticEvents.LevelUI.UnlockedButtonPressed -= PlayLevelCompleteButtonSfx;
        StaticEvents.LevelUI.CompleteButtonPressed -= PlayLevelButtonSfx;

        StaticEvents.LevelUI.AllLevelsCompleted -= PlayLevelsWinSfx;

        StaticEvents.ShopSystem.PurchaseCompleted -= PlayPurchaseCompleteSfx;
        StaticEvents.ShopSystem.PurchaseFailed -= PlayPurchaseFailedSfx;
        StaticEvents.ShopSystem.PurchaseFromTicketsComplete -= PlayPurchaseCompleteSfx;
        StaticEvents.ShopSystem.PurchaseFromTicketsDeclined -= PlayPurchaseFromTicketsDeclinedSfx;

        StaticEvents.Rewards.LockedClick -= PlayRewardBlockedSfx;
        StaticEvents.Rewards.UnlockedClick -= PlayRewardUnlockedSfx;
        StaticEvents.Rewards.CollectedClick -= PlayRewardCollectedSfx;
    }
    #endregion

    #region SpecialAudioForEvents
    void PlayLevelButtonLockedSfx()
    {
        PlaySfx("error");
        
    }
    void PlayLevelButtonSfx()
    {
        PlaySfx("button2");
    }
    void PlayLevelCompleteButtonSfx()
    {
        PlaySfx("level-complete");
    }
    public void PlayLevelsWinSfx()
    {
        // mute music for duration if its on
        if(musicSource.mute != true)
        {
            musicSource.mute = true;
            Invoke(nameof(ToggleMusic), 8f);
        }
        PlaySfx("game-end");
    }
    void PlayPurchaseCompleteSfx()
    {
        PlaySfx("gold");
    }
    void PlayPurchaseFailedSfx()
    {
        PlaySfx("fail");
    }
    void PlayPurchaseFromTicketsDeclinedSfx()
    {
        PlaySfx("error");
    }

    void PlayRewardBlockedSfx()
    {
        PlaySfx("error");
    }
    void PlayRewardUnlockedSfx()
    {
        PlaySfx("magic");
    }
    void PlayRewardCollectedSfx()
    {
        //PlaySfx("error");
    }
    #endregion

    public Sound FindAudio(string name, Sound[] audioList)
    {
        Sound sound = Array.Find(audioList, s => s.name == name);
        if(sound == null)
        {
            Debug.Log("Audio not found");
        }
        return sound;
    }

    public void PlayMusic(string name)
    {
        Sound sound = FindAudio(name, musicSounds);
        if(sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }
    public void PlaySfx(string name)
    {
        Sound sound = FindAudio(name, sfxSounds);
        if(sound != null)
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume; 
    }
    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume; 
    }

}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Toggle musicToggle;

    [SerializeField]
    private Toggle soundsToggle;
    bool musicIsOn;
    bool soundsIsOn;
    
    void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(MusicMuteChanges);
        soundsToggle.onValueChanged.AddListener(SoundMuteChanges);
    }
    void OnDisable()
    {
        musicToggle.onValueChanged.RemoveListener(MusicMuteChanges);
        soundsToggle.onValueChanged.RemoveListener(SoundMuteChanges);
    }

    void Start()
    {
        if(IsSettingsInPrefs())
        {
            GetSettingsFromPrefs();
        }
        else
        {
            InitDefaultSettings();
        }
        InitAudioToggles();
    }


    bool IsSettingsInPrefs()
    {
        return PlayerPrefs.HasKey(nameof(musicIsOn));
    }
    void InitDefaultSettings()
    {
        musicIsOn = true;
        soundsIsOn = true;
    }
    void GetSettingsFromPrefs()
    {
        musicIsOn = PlayerPrefs.GetInt(nameof(musicIsOn))==1;
        soundsIsOn = PlayerPrefs.GetInt(nameof(soundsIsOn))==1;
    }

    void InitAudioToggles()
    {
        musicToggle.isOn = musicIsOn;
        soundsToggle.isOn = soundsIsOn;
    }

    void SaveSettingsToPrefs()
    {
        PlayerPrefs.SetInt(nameof(musicIsOn), musicIsOn ? 1 : 0);
        PlayerPrefs.SetInt(nameof(soundsIsOn), soundsIsOn ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void MusicMuteChanges(bool muted)
    {
        musicIsOn = muted;
        SaveSettingsToPrefs();
    }
    public void SoundMuteChanges(bool muted)
    {
        soundsIsOn = muted;
        SaveSettingsToPrefs();
    }  
}

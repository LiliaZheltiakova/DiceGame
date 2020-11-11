using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    private static HUD m_instance;
    public static HUD Instance
    {
        get { return m_instance; }
    }

    [SerializeField] private TextMeshProUGUI m_scoreValue;
    [SerializeField] private TextMeshProUGUI m_turnsValue;

    [SerializeField] private Slider m_soundSlider;
    [SerializeField] private Slider m_musicSlider;

    void Awake()
    {
        m_instance = this;
    }

    void Update()
    {
        
    }

    public void UpdateTurnsValue(int value)
    {
        m_turnsValue.text = value.ToString();
    }

    public void UpdateScoreValue(int value)
    {
        m_scoreValue.text = value.ToString();
    }

    public void ShowWindow(CanvasGroup window)
    {
        window.alpha = 1f;
        window.blocksRaycasts = true;
        window.interactable = true;
    }

    public void HideWindow(CanvasGroup window)
    {
        window.alpha = 0f;
        window.blocksRaycasts = false;
        window.interactable = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        Controller.Instance.Reset();
    }

    public void SetMusicVolume(float volume)
    {
        Controller.Instance.Audio.MusicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        Controller.Instance.Audio.SfxVolume = volume;
    }

    public void UpdateOptions()
    {
        m_musicSlider.value = Controller.Instance.Audio.MusicVolume;

        m_soundSlider.value = Controller.Instance.Audio.SfxVolume;
    }

}

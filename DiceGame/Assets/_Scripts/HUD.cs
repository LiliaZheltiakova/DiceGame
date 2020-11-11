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

    [SerializeField] private TextMeshProUGUI[] m_scoreValue;
    [SerializeField] private TextMeshProUGUI m_turnsValue;

    [SerializeField] private Slider m_soundSlider;
    [SerializeField] private Slider m_musicSlider;

    [SerializeField] private CanvasGroup m_levelCompleteWindow;

    private GraphicRaycaster m_raycaster;

    void Awake()
    {
        m_instance = this;
        m_raycaster = gameObject.GetComponent<GraphicRaycaster>();
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
        for(int i = 0; i < m_scoreValue.Length; i++)
        {
            m_scoreValue[i].text = value.ToString();
        }
        
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

    public void Next()
    {
        Controller.Instance.InitializeLevel();
    }

    private IEnumerator Count(int to, float delay)
    {
        m_raycaster.enabled = false;

        for(int i = 0; i <= to; i++)
        {
            yield return new WaitForSeconds(delay);

            Controller.Instance.Score.AddTurnBonus();
        }
        DataStore.SaveGame();
        m_raycaster.enabled = true;
        yield break;
    }

    public void CountScore(int to)
    {
        ShowWindow(m_levelCompleteWindow);
        StartCoroutine(Count(to, .3f));
    }

    public void PlayPreviewSound()
    {
        Controller.Instance.Audio.PlaySound("Drop");
    }
}
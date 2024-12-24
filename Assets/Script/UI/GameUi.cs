using System;
using Script.UI;
using UnityEngine;
using Yd.Audio;
using Yd.Gameplay.Object;

public class GameUi : Actor
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Hud;
    [SerializeField] private GameObject DeadPanel;
    [SerializeField] private GameObject MessagePanel;
    [SerializeField] private GameObject FinishPanel;

    private void Start()
    {
        var buttonSounds = GetComponentsInChildren<ButtonSound>(includeInactive: true);
        foreach (var buttonSound in buttonSounds)
        {
            buttonSound.PointerClick += PlayButtonClickSound;
            buttonSound.PointerEnter += PlayButtonHoverSound;
        }
    }

    public void OnStartGameClicked()
    {
        StartGameClicked?.Invoke();
    }

    public void SetActiveMainMenu(bool active)
    {
        MainMenu.SetActive(active);
    }

    public void SetActiveHud(bool active)
    {
        Hud.SetActive(active);
    }

    public void SetActiveMessagePanel(bool active)
    {
        MessagePanel.SetActive(active);
    }

    public void SetActiveDeadPanel(bool active)
    {
        DeadPanel.SetActive(active);
    }

    public void SetActiveFinishPanel(bool active)
    {
        FinishPanel.SetActive(active);
    }

    public void OnBackClicked()
    {
        BackClicked?.Invoke();
    }

    private void PlayButtonClickSound()
    {
        AudioManager.PlayOneShot(AudioId.ButtonClick, AudioChannel.SFX);
    }

    private void PlayButtonHoverSound()
    {
        AudioManager.PlayOneShot(AudioId.ButtonHover, AudioChannel.SFX);
    }

    public void OnMessageCloseButtonClicked()
    {
        SetActiveMessagePanel(false);
        MessageClosed?.Invoke();
    }

    public event Action BackClicked;
    public event Action StartGameClicked;
    public event Action MessageClosed;
}
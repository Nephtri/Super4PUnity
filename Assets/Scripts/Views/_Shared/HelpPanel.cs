using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

//Loads static content (music, characters, voice) & Populates UI elements
public class HelpPanel : MonoBehaviour
{
    public RectTransform InnerHelpPanelRect;
    public float HelpPanelTransitionSpeed;
    private Vector2 HelpPanelTransitionVector;
    private GameObject HelpPanelTextGroup;
    private Vector2 InitialHelpPanelSizeDelta;
    public string SoundFXName;
    public bool IsHelpOpen;
    public bool IsHelpTransitionComplete;

    // Start is called before the first frame update
    public void Initialize(string soundFXName)
    {
        HelpPanelTextGroup = InnerHelpPanelRect.transform.GetChild(0).gameObject;
        InitialHelpPanelSizeDelta = new Vector2(InnerHelpPanelRect.sizeDelta.x, InnerHelpPanelRect.sizeDelta.y);
        HelpPanelTransitionVector = new Vector2(InnerHelpPanelRect.sizeDelta.x * HelpPanelTransitionSpeed,
            InnerHelpPanelRect.sizeDelta.y * HelpPanelTransitionSpeed);

        SoundFXName = soundFXName;

        IsHelpOpen = false;
        IsHelpTransitionComplete = true;
    }
    public void OnToggleHelp()
    {
        if(!IsHelpTransitionComplete) { return; }

        if(!IsHelpOpen) {
            if(!string.IsNullOrWhiteSpace(SoundFXName)) {
                MainController.MC.SFXAudioPlayer.clip = AudioLibrary.AL.GetAudioClipByName(SoundFXName);
                MainController.MC.SFXAudioPlayer.Stop();
                MainController.MC.SFXAudioPlayer.Play();
            }

            InnerHelpPanelRect.sizeDelta = Vector2.zero;
            HelpPanelTextGroup.gameObject.SetActive(false);
            gameObject.SetActive(true);

            IsHelpOpen = true;
            IsHelpTransitionComplete = false;
            MainController.MC.UpdateDelegate += TransitionHelpPanel;
        }
        else {
            IsHelpOpen = false;
            IsHelpTransitionComplete = false;
            MainController.MC.UpdateDelegate += TransitionHelpPanel;

            HelpPanelTextGroup.gameObject.SetActive(false);
        }
    }

    private void TransitionHelpPanel()
    {
        if(IsHelpOpen) 
        {
            InnerHelpPanelRect.sizeDelta += HelpPanelTransitionVector;

            if(InnerHelpPanelRect.sizeDelta.y >= InitialHelpPanelSizeDelta.y)
            {
                InnerHelpPanelRect.sizeDelta.Set(InitialHelpPanelSizeDelta.x, InitialHelpPanelSizeDelta.y);
                HelpPanelTextGroup.gameObject.SetActive(true);
                IsHelpTransitionComplete = true;
                MainController.MC.UpdateDelegate -= TransitionHelpPanel;
            }
        } 
        else 
        {
            InnerHelpPanelRect.sizeDelta -= HelpPanelTransitionVector;

            if(InnerHelpPanelRect.sizeDelta.y <= 0)
            {
                InnerHelpPanelRect.sizeDelta.Set(0, 0);
                gameObject.SetActive(false);
                IsHelpTransitionComplete = true;
                MainController.MC.UpdateDelegate -= TransitionHelpPanel;
            }
        }
    }
}

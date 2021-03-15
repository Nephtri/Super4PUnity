// GENERATED AUTOMATICALLY FROM 'Assets/Utilities/InputActionHub.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActionHub : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActionHub()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActionHub"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""17de1da7-4a35-46af-97bc-5da8178a13fd"",
            ""actions"": [
                {
                    ""name"": ""HideUI"",
                    ""type"": ""Button"",
                    ""id"": ""24ecdd7e-d37f-4dee-bd71-407da9e25edc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""e3feacb8-9b9b-48c0-ad00-a0f85f50e7af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fullscreen"",
                    ""type"": ""Button"",
                    ""id"": ""6b48b7ea-6941-4761-8598-ea0720f11145"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchMusicPrev"",
                    ""type"": ""Button"",
                    ""id"": ""281682f2-f93a-4b28-990f-621e65423a1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchMusicNext"",
                    ""type"": ""Button"",
                    ""id"": ""81789e57-5cf3-4f15-bc08-061550ac7417"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleHelp"",
                    ""type"": ""Button"",
                    ""id"": ""141124c9-1e61-407c-9178-5381f68d61ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleMusic"",
                    ""type"": ""Button"",
                    ""id"": ""b6465336-f9b3-42af-9da4-80f9723b8552"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleVideoSFX"",
                    ""type"": ""Button"",
                    ""id"": ""e9f31bb8-32ed-4202-965d-c446b1e93bf3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleVoiceSFX"",
                    ""type"": ""Button"",
                    ""id"": ""ed55f495-b4f2-495e-b821-0230c4a28d94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AnyKey"",
                    ""type"": ""Button"",
                    ""id"": ""a07c296f-dc76-43cf-bbcc-a501029d08ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""311499b7-99a3-4df8-876c-4004a1fe382d"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HideUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55de8daa-1648-442e-bebc-77f94641d883"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""390f86ce-37d3-4146-918e-8197049ede18"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4a7a7ab-385f-4c19-8eee-5f62a6af57a4"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleHelp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""704944d7-6228-4c30-b4d1-c69ee8453d09"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleMusic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bb01a34-da77-4754-a397-e0a1e3b37773"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleVideoSFX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""758f1560-f405-42db-9184-2f8232bc3cca"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleVoiceSFX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b832b4a-10a5-4ebd-a6e6-0371cbdc73b1"",
                    ""path"": ""<Keyboard>/comma"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchMusicPrev"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bd85a5c-0503-495d-960d-6bc37f1c172e"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AnyKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5c511ee-faf2-496b-823d-1c3adbea98c8"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchMusicNext"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_HideUI = m_Player.FindAction("HideUI", throwIfNotFound: true);
        m_Player_Escape = m_Player.FindAction("Escape", throwIfNotFound: true);
        m_Player_Fullscreen = m_Player.FindAction("Fullscreen", throwIfNotFound: true);
        m_Player_SwitchMusicPrev = m_Player.FindAction("SwitchMusicPrev", throwIfNotFound: true);
        m_Player_SwitchMusicNext = m_Player.FindAction("SwitchMusicNext", throwIfNotFound: true);
        m_Player_ToggleHelp = m_Player.FindAction("ToggleHelp", throwIfNotFound: true);
        m_Player_ToggleMusic = m_Player.FindAction("ToggleMusic", throwIfNotFound: true);
        m_Player_ToggleVideoSFX = m_Player.FindAction("ToggleVideoSFX", throwIfNotFound: true);
        m_Player_ToggleVoiceSFX = m_Player.FindAction("ToggleVoiceSFX", throwIfNotFound: true);
        m_Player_AnyKey = m_Player.FindAction("AnyKey", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_HideUI;
    private readonly InputAction m_Player_Escape;
    private readonly InputAction m_Player_Fullscreen;
    private readonly InputAction m_Player_SwitchMusicPrev;
    private readonly InputAction m_Player_SwitchMusicNext;
    private readonly InputAction m_Player_ToggleHelp;
    private readonly InputAction m_Player_ToggleMusic;
    private readonly InputAction m_Player_ToggleVideoSFX;
    private readonly InputAction m_Player_ToggleVoiceSFX;
    private readonly InputAction m_Player_AnyKey;
    public struct PlayerActions
    {
        private @InputActionHub m_Wrapper;
        public PlayerActions(@InputActionHub wrapper) { m_Wrapper = wrapper; }
        public InputAction @HideUI => m_Wrapper.m_Player_HideUI;
        public InputAction @Escape => m_Wrapper.m_Player_Escape;
        public InputAction @Fullscreen => m_Wrapper.m_Player_Fullscreen;
        public InputAction @SwitchMusicPrev => m_Wrapper.m_Player_SwitchMusicPrev;
        public InputAction @SwitchMusicNext => m_Wrapper.m_Player_SwitchMusicNext;
        public InputAction @ToggleHelp => m_Wrapper.m_Player_ToggleHelp;
        public InputAction @ToggleMusic => m_Wrapper.m_Player_ToggleMusic;
        public InputAction @ToggleVideoSFX => m_Wrapper.m_Player_ToggleVideoSFX;
        public InputAction @ToggleVoiceSFX => m_Wrapper.m_Player_ToggleVoiceSFX;
        public InputAction @AnyKey => m_Wrapper.m_Player_AnyKey;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @HideUI.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHideUI;
                @HideUI.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHideUI;
                @HideUI.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHideUI;
                @Escape.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEscape;
                @Fullscreen.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFullscreen;
                @Fullscreen.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFullscreen;
                @Fullscreen.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFullscreen;
                @SwitchMusicPrev.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchMusicPrev;
                @SwitchMusicPrev.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchMusicPrev;
                @SwitchMusicPrev.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchMusicPrev;
                @SwitchMusicNext.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchMusicNext;
                @SwitchMusicNext.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchMusicNext;
                @SwitchMusicNext.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchMusicNext;
                @ToggleHelp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleHelp;
                @ToggleHelp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleHelp;
                @ToggleHelp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleHelp;
                @ToggleMusic.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleMusic;
                @ToggleMusic.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleMusic;
                @ToggleMusic.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleMusic;
                @ToggleVideoSFX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleVideoSFX;
                @ToggleVideoSFX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleVideoSFX;
                @ToggleVideoSFX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleVideoSFX;
                @ToggleVoiceSFX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleVoiceSFX;
                @ToggleVoiceSFX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleVoiceSFX;
                @ToggleVoiceSFX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleVoiceSFX;
                @AnyKey.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnyKey;
                @AnyKey.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnyKey;
                @AnyKey.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnyKey;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HideUI.started += instance.OnHideUI;
                @HideUI.performed += instance.OnHideUI;
                @HideUI.canceled += instance.OnHideUI;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
                @Fullscreen.started += instance.OnFullscreen;
                @Fullscreen.performed += instance.OnFullscreen;
                @Fullscreen.canceled += instance.OnFullscreen;
                @SwitchMusicPrev.started += instance.OnSwitchMusicPrev;
                @SwitchMusicPrev.performed += instance.OnSwitchMusicPrev;
                @SwitchMusicPrev.canceled += instance.OnSwitchMusicPrev;
                @SwitchMusicNext.started += instance.OnSwitchMusicNext;
                @SwitchMusicNext.performed += instance.OnSwitchMusicNext;
                @SwitchMusicNext.canceled += instance.OnSwitchMusicNext;
                @ToggleHelp.started += instance.OnToggleHelp;
                @ToggleHelp.performed += instance.OnToggleHelp;
                @ToggleHelp.canceled += instance.OnToggleHelp;
                @ToggleMusic.started += instance.OnToggleMusic;
                @ToggleMusic.performed += instance.OnToggleMusic;
                @ToggleMusic.canceled += instance.OnToggleMusic;
                @ToggleVideoSFX.started += instance.OnToggleVideoSFX;
                @ToggleVideoSFX.performed += instance.OnToggleVideoSFX;
                @ToggleVideoSFX.canceled += instance.OnToggleVideoSFX;
                @ToggleVoiceSFX.started += instance.OnToggleVoiceSFX;
                @ToggleVoiceSFX.performed += instance.OnToggleVoiceSFX;
                @ToggleVoiceSFX.canceled += instance.OnToggleVoiceSFX;
                @AnyKey.started += instance.OnAnyKey;
                @AnyKey.performed += instance.OnAnyKey;
                @AnyKey.canceled += instance.OnAnyKey;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnHideUI(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnFullscreen(InputAction.CallbackContext context);
        void OnSwitchMusicPrev(InputAction.CallbackContext context);
        void OnSwitchMusicNext(InputAction.CallbackContext context);
        void OnToggleHelp(InputAction.CallbackContext context);
        void OnToggleMusic(InputAction.CallbackContext context);
        void OnToggleVideoSFX(InputAction.CallbackContext context);
        void OnToggleVoiceSFX(InputAction.CallbackContext context);
        void OnAnyKey(InputAction.CallbackContext context);
    }
}

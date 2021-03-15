using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

//Parent controller for title screen (mod loading/config). Passes on inputs to relevant game objects
public class MenuController : InputController
{
    public SourcePanel SourcePanel;
    public ModPanel ModPanel;
    public MusicManagerPanel MusicPanel;
    public InputField PresetInputField;

    public HelpPanel HelpPanel;

    // public CanvasGroup HelpPanel;
    // public RectTransform InnerHelpPanelRect;
    // public float HelpPanelTransitionSpeed;
    // private Vector2 HelpPanelTransitionVector;
    // private GameObject HelpPanelTextGroup;
    // private Vector2 InitialHelpPanelSizeDelta;
    // private bool IsHelpOpen;
    // private bool IsHelpTransitionComplete;

    public Dropdown ResDropdown;
    public Toggle FullscreenToggle;
    public Dropdown DefaultCharStyleDD;
    public Dropdown DefaultPoseStyleDD;

    private readonly string SPREADSHEET_URL = "https://docs.google.com/spreadsheets/d/1kAkBLDDOEF-v0_LPNFYi4IwR0NpWBF575WicjG6tEIw/";
    //private readonly string SPREADSHEET_URL = "https://docs.google.com/document/d/1FP3rpCCxqR_ar4e6-vXTJwo41zf72_0a5tq3VNmpg9Q/edit?usp=sharing";
    public bool IsUpdateDelegated = false;

    public static MenuController MenuCtrl;
    
    protected virtual void Awake()
    {
        //Singleton pattern
        if (MenuCtrl == null) {
            MenuCtrl = this;
        }
        else if (MenuCtrl != this) {
            Destroy(MenuCtrl);
            MenuCtrl = this;
        }     
    }

    override protected void Start() 
    {
        base.Start();

        HelpPanel.Initialize("SMW-MessageBlock");

        if(GameDataManager.IsPlayerDataLoaded)
        {
            Initialize();
        }
        else
        {
            MainController.MC.UpdateDelegate += Initialize;
            IsUpdateDelegated = true;
        }
    }
    
    public void Initialize() 
    {
        if(IsUpdateDelegated)
        {
            MainController.MC.UpdateDelegate -= Initialize;
            IsUpdateDelegated = false;
        }

        MusicPanel.Initialize();
        ModPanel.Initialize();
        SourcePanel.Initialize();

        ResDropdown.SetValueWithoutNotify((int)GameDataManager.GM.PlyrConfigData.AspectResolution);
        FullscreenToggle.SetIsOnWithoutNotify(GameDataManager.GM.PlyrConfigData.IsFullscreen);
        DefaultCharStyleDD.SetValueWithoutNotify((int)GameDataManager.GM.PlyrConfigData.CharPlaybackStyle);
        DefaultPoseStyleDD.SetValueWithoutNotify((int)GameDataManager.GM.PlyrConfigData.PosePlaybackStyle);

        MainController.MC.GS_Current = GameState.Active;
    }

    public void OnLibraryLinkClick()
    {
        Application.OpenURL(SPREADSHEET_URL);
    }

    public void OnSetCharStyle(int value)
    {
        GameDataManager.GM.PlyrConfigData.CharPlaybackStyle = (PlaybackStyle)value;
    }

    public void OnSetPoseStyle(int value)
    {
        GameDataManager.GM.PlyrConfigData.PosePlaybackStyle = (PlaybackStyle)value;
    }

    public void OnPlay()
    {
        GameDataManager.GM.SetSelectedMods(SourcePanel.GetSelectedMods());
        GameDataManager.GM.SetSelectedMusic(SourcePanel.GetSelectedMusic());
        ModPanel.RemoveUnusedSprites();
        MainController.MC.GoToScene("Game");
    }

    public void OnHelpClick()
    {
        HelpPanel.OnToggleHelp();
    }

    public void SwitchResolution(int res)
    {
        MainController.MC.SwitchResolution(res);
    }

    //--Player Input Events--
    override public void OnAnyKey(CallbackContext context)
    {
        if(!context.performed || !HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        HelpPanel.OnToggleHelp();
    } 

    override public void OnToggleHelp(CallbackContext context)
    {
        if(!context.performed || PresetInputField.isFocused || HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        OnHelpClick();
    } 

    override public void OnFullscreen(CallbackContext context)
    {
        if(!context.performed || PresetInputField.isFocused || HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        FullscreenToggle.isOn = !FullscreenToggle.isOn;
    } 

    override public void OnEscape(CallbackContext context)
    {
        if(!context.performed || PresetInputField.isFocused) { return; }
        Application.Quit();
    }
}
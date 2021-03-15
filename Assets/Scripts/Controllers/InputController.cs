using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

//Controller that handles input actions
//Override if need additional conditions met before broadcasting input
public class InputController : MonoBehaviour, InputActionHub.IPlayerActions
{
    public static Vector2 UnitResolution;
    public static int PPU = 100;
    protected InputActionHub InputHub;
    protected Vector2 PrevMoveDir;
    protected float PrevMoveSqrMag;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitializeBinding();
    }

    void InitializeBinding()
    {
        InputHub = new InputActionHub();
        InputHub.Player.SetCallbacks(this);
        InputHub.Player.Enable();
    }

    protected virtual void OnDestroy()
    {
        if(InputHub != null) { InputHub.Dispose(); }
    }
    public virtual void OnHideUI(CallbackContext context)
    {
        if(!context.performed) { return; }
        //BroadcastMessage("InputHideUI", SendMessageOptions.DontRequireReceiver);
    }
    public virtual void OnAnyKey(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnEscape(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnFullscreen(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnSwitchMusicPrev(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnSwitchMusicNext(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnToggleHelp(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnToggleMusic(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnToggleVideoSFX(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
    public virtual void OnToggleVoiceSFX(CallbackContext context)
    {
        if(!context.performed) { return; }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

//Loads static content (music, characters, voice) & Populates UI elements
public class CharacterPanel : MonoBehaviour
{
    public GameObject CharPanel;
    public Button CharPanelBtn;
    private bool IsVisible;

    public GameObject ListPanel;
    public GameObject ItemActiveHighlighter;
    public GameObject ItemNextHighlighter;
    public GameObject ItemSelectedHighlighter;
    private Vector3 InitialHighlighterPos;

    public ObjectPooler CharItemPooler;

    public Image PlaybackStyleBtnImg;
    public Text PlaybackStyleBtnTxt;
    public PlaybackStyle PlaybackStyle;
    public Color DefaultTxtColor;

    private List<CharacterListItem> ContentListItems;

    //Active: Is currently playing
    //Next: Will play next based on playback style
    //Selected: Have picked it manually, poses should be open
    private CharacterListItem ActiveItem;
    private CharacterListItem NextItem;
    private CharacterListItem SelectedItem;

    private float ItemHeight;
    private float InitialListHeight;

    private readonly int PANEL_WIDTH = 260;

    // Start is called before the first frame update
    public int Initialize()
    {
        IsVisible = true;
        InitialHighlighterPos = ItemSelectedHighlighter.transform.localPosition;
        SetPlaybackStyle(GameDataManager.GM.PlyrConfigData.CharPlaybackStyle);

        ContentListItems = new List<CharacterListItem>();

        var index = 0;
        foreach (string mod in GameDataManager.GM.SelectedMods)
        {
            var charIndex = index;
            var charName = mod.Replace('\\','/').Substring(mod.LastIndexOf('_') + 1);

            var charItem = CharItemPooler.GetPooledObject(charName, ListPanel.transform).GetComponent<CharacterListItem>();
            ItemHeight = charItem.RectTrans.rect.height;
            
            charItem.RectTrans.anchoredPosition = new Vector3(0, index * -ItemHeight, 0);
            charItem.CharImg.sprite = SpriteLibrary.SL.GetSpriteByName(charName);
            charItem.CharBtn.onClick.AddListener(() => OnSelectItem(charIndex));
            charItem.CharacterName = charName;
            charItem.VideoPaths = GameDataManager.GM.GetVideoPaths(mod);
            charItem.VideoPaths.Sort();

            ContentListItems.Add(charItem);

            index++;
        }
        ResizeScrollView();

        var firstIndex = PlaybackStyle != PlaybackStyle.Random ? 0 : UnityEngine.Random.Range(0, ContentListItems.Count);
        OnSelectItem(firstIndex, false, false);

        return ContentListItems.Count;
        
    }

    //TODO: All 3 Items are set similarly, refactor?
    //Either cycle completed with a new next char OR First iteration
    public void SetActivecharacter(int index)
    {
        var item = ContentListItems[index];
        if(ActiveItem == item) { return; }

        ActiveItem = item;

        if(ActiveItem == NextItem && ItemNextHighlighter.activeSelf) { ItemNextHighlighter.SetActive(false); }

        if(!ItemActiveHighlighter.activeSelf) { ItemActiveHighlighter.SetActive(true); }
        RepositionHighlighter(ItemActiveHighlighter.gameObject, index);

        VideoController.VC.ActiveCharIndex = index;
        VideoController.VC.ActivePoseCount = ActiveItem.VideoPaths.Count;
    }

    //Sets NextItem var based on passed index OR playback style and ActiveItem
    public void SetNextCharacter(int index = -1, bool forcePoseUpdate = true)
    {
        if(index == -1)
        {
            index = ContentListItems.IndexOf(ActiveItem);
            switch(PlaybackStyle)
            {
                case(PlaybackStyle.Order):
                    if(index >= ContentListItems.Count-1) { index = 0; }
                    else { index++; }
                    break;

                case(PlaybackStyle.Random):
                    index = UnityEngine.Random.Range(0, ContentListItems.Count);
                    break;
            }
        }

        var item = ContentListItems[index];
        if(NextItem == item) { return; }

        NextItem = item;

        if(NextItem == ActiveItem)
        {
            if(ItemNextHighlighter.activeSelf) { 
                ItemNextHighlighter.SetActive(false); 
            }
        }
        else
        {
            if(NextItem == SelectedItem && ItemSelectedHighlighter.activeSelf) { ItemSelectedHighlighter.SetActive(false); }
            if(NextItem != ActiveItem) {
                if(!ItemNextHighlighter.activeSelf) { ItemNextHighlighter.SetActive(true); }
                RepositionHighlighter(ItemNextHighlighter.gameObject, index);
            }
        }

        VideoController.VC.NextCharIndex = index;
        VideoController.VC.NextPoseCount = item.VideoPaths.Count;
        
        if(forcePoseUpdate) { VideoController.VC.OnSetNextCharIndex(); }
    }

    public void OnSelectItem(int index, bool showHighlighter = true, bool isManual = true)
    {
        var item = ContentListItems[index];
        if(item == SelectedItem || (SelectedItem == null && item == ActiveItem)) { return; }

        SelectedItem = item;

        if(showHighlighter)
        {
            if(!ItemSelectedHighlighter.activeSelf) { ItemSelectedHighlighter.SetActive(true); }
            RepositionHighlighter(ItemSelectedHighlighter.gameObject, index);
        }

        VideoController.VC.SelectedCharIndex = index;
        VideoController.VC.OnCharacterSelected(SelectedItem.VideoPaths.Count, isManual);
    }

    public void ClearSelectedItem()
    {
        SelectedItem = null;
        if(ItemSelectedHighlighter.activeSelf) { ItemSelectedHighlighter.SetActive(false); }
    }

    private void RepositionHighlighter(GameObject highlighter, int index)
    {
        // var initialPosY = highlighter == ItemSelectedHighlighter ? InitialHighlighterPos.y : 0;
        // var prevX = highlighter.transform.localPosition.x;
        // highlighter.transform.localPosition = new Vector3(prevX, initialPosY - (ItemHeight * index), 0);   

        var prevX = highlighter.transform.localPosition.x;
        highlighter.transform.localPosition = new Vector3(prevX, InitialHighlighterPos.y - (ItemHeight * index), 0);   
    }

    //Called with NextCharacterIndex
    public string GetVideoPath(int index)
    {
        return NextItem.VideoPaths[index];
    }

    public void TogglePlaybackStyle()
    {
        switch(PlaybackStyle)
        {
            case(PlaybackStyle.Loop):
                SetPlaybackStyle(PlaybackStyle.Order);
                break;
            case(PlaybackStyle.Order):
                SetPlaybackStyle(PlaybackStyle.Random);
                break;
            case(PlaybackStyle.Random):
                SetPlaybackStyle(PlaybackStyle.Loop);
                break;
        }
        VideoController.VC.OnToggleCharStyle();
    }

    public void SetPlaybackStyle(PlaybackStyle style)
    {
        switch(style)
        {
            case(PlaybackStyle.Loop):
                PlaybackStyleBtnTxt.color = DefaultTxtColor;
                PlaybackStyleBtnTxt.rectTransform.offsetMax = new Vector2(PlaybackStyleBtnTxt.rectTransform.offsetMax.x, -60);
                break;

            default:
                if(PlaybackStyle == PlaybackStyle.Loop)
                {
                    PlaybackStyleBtnTxt.color = Color.white;
                    PlaybackStyleBtnTxt.rectTransform.offsetMax = new Vector2(PlaybackStyleBtnTxt.rectTransform.offsetMax.x, 0);
                }
                break;    
        }
        PlaybackStyle = style;
        var styleName = Enum.GetName(typeof(PlaybackStyle), PlaybackStyle);
        PlaybackStyleBtnTxt.text = styleName;
        PlaybackStyleBtnImg.sprite = SpriteLibrary.SL.GetSpriteByName(styleName + "Button");
    }

    private void ResizeScrollView()
    {
        var charPanelRect = ListPanel.GetComponent<RectTransform>();
        InitialListHeight = charPanelRect.rect.height;
        var deltaListHeight = (ItemHeight * ContentListItems.Count) - InitialListHeight;
        
        if(deltaListHeight > 0) {
            charPanelRect.offsetMin = new Vector2(charPanelRect.offsetMin.x, charPanelRect.offsetMin.y - deltaListHeight);
        }
    }

    public void ToggleCharPanel()
    {
        var rect = CharPanel.GetComponent<RectTransform>();
        if (IsVisible)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x - PANEL_WIDTH, rect.offsetMin.y);
            rect.offsetMax = new Vector2(rect.offsetMax.x - PANEL_WIDTH, rect.offsetMax.y);
        } else
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x + PANEL_WIDTH, rect.offsetMin.y);
            rect.offsetMax = new Vector2(rect.offsetMax.x + PANEL_WIDTH, rect.offsetMax.y);
        }
        IsVisible = !IsVisible;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PosePanel : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject ListPanel;
    public Scrollbar VerticalScrollbar;
    public ObjectPooler PoseItemPooler;
    public Button HelpBtn;

    public Image PlaybackStyleBtnImg;
    public Text PlaybackStyleBtnTxt;
    public PlaybackStyle PlaybackStyle;
    public Color DefaultTxtColor;
    public Color DefaultBtnColor;
    public Color SelectedBtnColor;

    private List<PoseListItem> ContentListItems;
    private PoseListItem ActiveItem; 
    private PoseListItem NextItem;
    private PoseListItem SelectedItem;

    private string DefaultBlockSpriteName;
    private string NextBlockSpriteName;

    private float ItemHeight;
    private float InitialListHeight;
    private float InitialOffsetMinY;
    private bool IsVisible;
    private readonly int PANEL_WIDTH = 260;

    public void Initialize()
    {
        IsVisible = true;
        InitialListHeight = InitialOffsetMinY = -1;
        ContentListItems = new List<PoseListItem>();
        DefaultBlockSpriteName = "WhiteButton";
        NextBlockSpriteName = "YellowSelectedButton";
        SetPlaybackStyle(GameDataManager.GM.PlyrConfigData.PosePlaybackStyle);
    }
    
    public void CreatePoses(int poseCount)
    {
        //Remove extra pose btns if any
        if(ContentListItems.Count > poseCount) 
        {
            for(var i = poseCount; i < ContentListItems.Count; i++)
            {
                var item = ContentListItems[i];
                item.BlockBtn.onClick.RemoveAllListeners();
                PoseItemPooler.RepoolObject(item.gameObject);
            }
            ContentListItems.RemoveRange(poseCount, ContentListItems.Count - poseCount);
            
        }
        for(var i = ContentListItems.Count; i < poseCount; i++)
        {
            var poseItem = PoseItemPooler.GetPooledObject("PoseBtn_" + i.ToString(), ListPanel.transform).GetComponent<PoseListItem>();
            var poseIndex = i;

            if(ItemHeight == 0) { ItemHeight = poseItem.RectTrans.rect.height; }

            poseItem.RectTrans.anchoredPosition = new Vector3(0, i * -ItemHeight, 0);
            poseItem.BlockImg.sprite = SpriteLibrary.SL.GetSpriteByName(DefaultBlockSpriteName);
            poseItem.NumText.text = (i+1).ToString();
            poseItem.BlockBtn.onClick.AddListener(() => OnSelectItem(poseIndex, true));

            ContentListItems.Add(poseItem);
        }
        ResizeScrollView(ContentListItems.Count);
    }

    public void OnSelectItem(int index, bool isManual)
    {
        var item = ContentListItems[index];
        if(item == SelectedItem || item == ActiveItem) { return; }

        //Set previously selected to normal style
        if(SelectedItem != null && SelectedItem != ActiveItem) { 
            SelectedItem.BlockBtn.image.sprite = SpriteLibrary.SL.GetSpriteByName(DefaultBlockSpriteName);
            SelectedItem.BlockBtn.image.color = DefaultBtnColor;
        }

        //Set new SelectedItem & style
        SelectedItem = item;

        //Make btn lighter if clicked
        if(isManual) {
            SelectedItem.BlockBtn.image.color = SelectedBtnColor;
        } else {
            SelectedItem.NumText.color = Color.white;
        }
        VideoController.VC.OnPoseSelected(index);

        Debug.Log("Selected set: " + index);
    }

    public void StyleBtns(bool hasActiveItem, bool hasNextItem)
    {
        if(hasActiveItem)
        {
            SetActivePose(VideoController.VC.ActivePoseIndex);
        }
        else if(ActiveItem != null)
        {
            ActiveItem.BlockBtn.interactable = true;
            ActiveItem.BlockImg.color = DefaultBtnColor;
            ActiveItem.NumText.color = DefaultTxtColor;
            ActiveItem = null;
        }

        if(hasNextItem)
        {
            SetNextPose(VideoController.VC.NextPoseIndex);
        }
        else if(NextItem != null)
        {
            NextItem.BlockImg.sprite = SpriteLibrary.SL.GetSpriteByName(DefaultBlockSpriteName);
            NextItem.BlockImg.color = DefaultBtnColor;

            if(NextItem != ActiveItem) { 
                NextItem.NumText.color = DefaultTxtColor; 
            }
           
            NextItem = null;
        }

        //Debug.Log("Styling Complete");
    }

    //Strictly styling of button
    public void SetActivePose(int index)
    {
        if(index >= ContentListItems.Count) { return; }
        
        var item = ContentListItems[index];
        if(item == ActiveItem) { return; }

        if(SelectedItem != null) {
            SelectedItem = null;
        }
        if(ActiveItem != null) {
            ActiveItem.BlockBtn.interactable = true;
            ActiveItem.BlockImg.color = DefaultBtnColor;
            ActiveItem.NumText.color = DefaultTxtColor;
        }

        ActiveItem = item;
        ActiveItem.BlockBtn.interactable = false;
        ActiveItem.BlockImg.sprite = SpriteLibrary.SL.GetSpriteByName(DefaultBlockSpriteName);
        ActiveItem.BlockImg.color = Color.white;
        ActiveItem.NumText.color = Color.white;

        //Debug.Log("Active set: " + index);
    }
    public void SetNextPose(int index)
    {
        if(index >= ContentListItems.Count) { return; }

        var item = ContentListItems[index];
        if(item == NextItem) { return; }

        if(NextItem != null && NextItem != ActiveItem) {
            NextItem.BlockImg.sprite = SpriteLibrary.SL.GetSpriteByName(DefaultBlockSpriteName);
            NextItem.BlockImg.color = DefaultBtnColor;
            NextItem.NumText.color = DefaultTxtColor;
        }

        NextItem = item;
        NextItem.NumText.color = Color.white;

        if(NextItem != ActiveItem) {
            NextItem.BlockImg.sprite = SpriteLibrary.SL.GetSpriteByName(NextBlockSpriteName);
            NextItem.BlockImg.color = Color.white;
            
        }

        //Debug.Log("Next set: " + index);
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
        VideoController.VC.OnTogglePoseStyle();
        
        if(SelectedItem != null) { 
            SelectedItem.BlockBtn.image.color = DefaultBtnColor;
        }
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

    private void ResizeScrollView(int listCount)
    {
        var posePanelRect = ListPanel.GetComponent<RectTransform>();

        if(InitialListHeight == -1) { 
            InitialListHeight = posePanelRect.rect.height;
            InitialOffsetMinY = posePanelRect.offsetMin.y; 
        }

        VerticalScrollbar.value = 1;
        var deltaListHeight = (ItemHeight * listCount) - InitialListHeight;

        if(deltaListHeight > 0) {
            posePanelRect.offsetMin = new Vector2(posePanelRect.offsetMin.x, InitialOffsetMinY - deltaListHeight);
        } else {
            posePanelRect.offsetMin = new Vector2(posePanelRect.offsetMin.x, InitialOffsetMinY);
        }
    }

    public void TogglePosePanel()
    {
        var rect = MainPanel.GetComponent<RectTransform>();
        if (IsVisible)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x + PANEL_WIDTH, rect.offsetMin.y);
            rect.offsetMax = new Vector2(rect.offsetMax.x + PANEL_WIDTH, rect.offsetMax.y);
            HelpBtn.gameObject.SetActive(false);
        } 
        else
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x - PANEL_WIDTH, rect.offsetMin.y);
            rect.offsetMax = new Vector2(rect.offsetMax.x - PANEL_WIDTH, rect.offsetMax.y);
            HelpBtn.gameObject.SetActive(true);
        }
        IsVisible = !IsVisible;
    }
}
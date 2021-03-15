using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

//Panel that lists all Mods in Mod Folder
public class MusicManagerPanel : MonoBehaviour
{
    public SourcePanel SourcePanel;
    public GameObject ContentList;
    public ObjectPooler ItemPooler;
    //public Text ModCountTxt;
    public Text ModLabelTxt;
    public Text NoMusicMsg;
    private int ModCount;
    private List<ToggleListItem> ContentListItems;
    private float InitialContentListHeight;
    private float InitialContentListWidth;

    
    void Start() {
        ContentListItems = new List<ToggleListItem>();
    }

    public void Initialize() 
    {
        ModCount = 0;

        var contentListRect = ContentList.GetComponent<RectTransform>();
        InitialContentListHeight = contentListRect.rect.height;
        InitialContentListWidth = contentListRect.rect.width;

        var charDirs = GameDataManager.GM.CharDirs;
        var index = 0;

        var musicFilePaths = GameDataManager.GM.GetMusicFilePaths();

        //TODO: Allow for selecting of music in the Title Screen instead of this loading
        // var musicStr = string.Empty;
        // var index = 0;
        // foreach(var musicFileName in MusicFileNames)
        // {
        //     var isLastItem = (index == MusicFileNames.Count() - 1);
        //     musicStr += musicFileName + (!isLastItem ? "," : string.Empty);
        //     index++;
        // }
        // SetSelectedMusic(musicStr);

        foreach(var musicFilePath in musicFilePaths)
        {
            var normalizedPath = musicFilePath.Replace('\\', '/');
            var fullFileName = normalizedPath.Substring(normalizedPath.LastIndexOf('/') + 1);
            var fileName = fullFileName.Substring(0, fullFileName.Length - 4);

            var listItem = CreateMusicListItem(fileName, fullFileName, index);
            listItem.Toggle.SetIsOnWithoutNotify(false);

            index++;
        }
        if(index == 0) {
            NoMusicMsg.text = NoMusicMsg.text + GameDataManager.GM.GetMusicPath().CleanFolderPath();
            NoMusicMsg.gameObject.SetActive(true);
        }
        var heightDiff = index * 45f - InitialContentListHeight;
        if(heightDiff > 0) {
            contentListRect.offsetMin = new Vector2(contentListRect.offsetMin.x, contentListRect.offsetMin.y - heightDiff);
        }
    }

    public bool HasItemWithValue(string value)
    {
        return ContentListItems.Any(x => x.Value == value);
    }

    public void OnItemSelect(ToggleListItem listItem, bool isFromPreset = false)
    {
        ModCount += listItem.Toggle.isOn ? 1 : -1;
        ModLabelTxt.text = "Music - " + ModCount;

        // ModCount += modListItem.Toggle.isOn ? 1 : -1;
        // ModCountTxt.text = ModCount + " selected";

        if(!isFromPreset) { SourcePanel.OnModToggle(listItem.Toggle.isOn, listItem.Value, false); }
        SourcePanel.CheckPlayBtn();
    }

    public void OnSelectAll()
    {
        foreach(var item in ContentListItems)
        {
            if(!item.Toggle.isOn) { item.Toggle.isOn = true; }
        }
    }

    public void OnClear(bool isFromPreset = false)
    {
        foreach(var item in ContentListItems)
        {
            if(item.Toggle.isOn) { 
                if(isFromPreset) { 
                    item.Toggle.SetIsOnWithoutNotify(false);
                    OnItemSelect(item, isFromPreset);
                }
                else { item.Toggle.isOn = false; }
            }
        }
    }

    //Invoked by SourcePanel when changing source
    public void SetItemsByString(string musicListStr)
    {
        OnClear(true);
        foreach(var musicFilePath in musicListStr.Split(','))
        {
            //TODO: If don't find a matching mod, put up a warning message
            var item = ContentListItems.FirstOrDefault(x => x.Value == musicFilePath.Trim());
            if(item != null)
            {
                item.Toggle.SetIsOnWithoutNotify(true);
                OnItemSelect(item, true);
            }
        }
    }

    private ToggleListItem CreateMusicListItem(string fileName, string fullFileName, int listIndex)
    {
        var listItem = ItemPooler.GetPooledObject("MusicListItem", ContentList.transform).GetComponent<ToggleListItem>();
        listItem.Name = fileName;
        listItem.Label.text = fileName;
        listItem.Value = fullFileName;
        listItem.transform.localPosition = new Vector3(0, -45f * listIndex, 0);

        var prevHeight = listItem.RectTransform.rect.height;
        listItem.RectTransform.sizeDelta = new Vector2(InitialContentListWidth, prevHeight);

        listItem.Toggle.onValueChanged.AddListener(delegate { 
            OnItemSelect(listItem); });
        ContentListItems.Add(listItem);

        return listItem;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

//Panel that lists all Mods in Mod Folder
public class ModPanel : MonoBehaviour
{
    public SourcePanel SourcePanel;
    public GameObject ContentList;
    public GameObject ModPreviewPanel;
    public ObjectPooler ItemPooler;
    public Text ModLabelTxt;
    public Text NoModsMsg;
    //public Text ModCountTxt;
    private int ModCount;
    private List<ModListItem> ContentListItems;
    private float InitialContentListHeight;
    
    void Start() {
        ContentListItems = new List<ModListItem>();
    }

    public void Initialize() 
    {
        ModCount = 0;

        var contentListRect = ContentList.GetComponent<RectTransform>();
        InitialContentListHeight = contentListRect.rect.height;

        var charDirs = GameDataManager.GM.CharDirs;
        var index = 0;

        foreach(var charDir in charDirs)
        {
            //TODO: Should we display long name or short name?
            
            #if UNITY_ANDROID
                var modPath = charDir.Substring(charDir.LastIndexOf('/') + 6);
            #else
                 var modPath = charDir.Substring(charDir.LastIndexOf('/') + 11);
            #endif
           
            var modShortName = modPath.Substring(modPath.LastIndexOf('_') + 1);

            SpriteLibrary.SL.AddExternalSprite(charDir + "/" + modShortName + ".png", modShortName);

            var modListItem = ItemPooler.GetPooledObject("ModListItem", ContentList.transform).GetComponent<ModListItem>();
            modListItem.ModName = modShortName;
            modListItem.ModPath = modPath;
            modListItem.Label.text = modPath;
            modListItem.transform.localPosition = new Vector3(0, -45f * index, 0);
            modListItem.Toggle.isOn = false;
            modListItem.ModPreviewPanel = ModPreviewPanel;

            modListItem.Toggle.onValueChanged.AddListener(delegate { 
                OnItemSelect(modListItem); });
            ContentListItems.Add(modListItem);

            index++;
        }

        if(index == 0) {
            NoModsMsg.text = NoModsMsg.text + GameDataManager.GM.GetModsPath().CleanFolderPath();
            NoModsMsg.gameObject.SetActive(true);
        }

        var heightDiff = index * 45f - InitialContentListHeight;
        if(heightDiff > 0) {
            contentListRect.offsetMin = new Vector2(contentListRect.offsetMin.x, contentListRect.offsetMin.y - heightDiff);
        }
    }

    public void OnItemSelect(ModListItem modListItem, bool isFromPreset = false)
    {
        ModCount += modListItem.Toggle.isOn ? 1 : -1;
        ModLabelTxt.text = "Chars - " + ModCount;
        //ModCountTxt.text = ModCount + " selected";

        if(!isFromPreset) { SourcePanel.OnModToggle(modListItem.Toggle.isOn, modListItem.ModPath, true); }
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
    //True = All mods found. False = Some mods not found
    public bool SetModsByString(string modListStr)
    {
        OnClear(true);
        var allModsFound = true;

        foreach(var modPath in modListStr.Split(','))
        {
            //TODO: If don't find a matching mod, put up a warning message
            var item = ContentListItems.FirstOrDefault(x => x.ModPath == modPath.Trim());
            if(item != null)
            {
                item.Toggle.SetIsOnWithoutNotify(true);
                OnItemSelect(item, true);
            }
            //Remove "broken" reference
            else
            { 
                if(allModsFound) { 
                    allModsFound = false; 
                }
                SourcePanel.OnModToggle(false, modPath, true);
            }
        }

        return allModsFound;
    }

    //Call before leaving scene to clear unused mod sprites
    public void RemoveUnusedSprites()
    {
        SpriteLibrary.SL.RemoveSpritesByName(ContentListItems.Where(x => !x.Toggle.isOn).Select(x => x.ModName));
    }
}
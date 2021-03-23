using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

//Panel that lists Mod sources (default folder or preset)
public class SourcePanel : MonoBehaviour
{
    public ModPanel ModPanel;
    public MusicManagerPanel MusicPanel;
    public GameObject ContentList;
    public SourceListItem DefaultItem;
    public GameObject PresetLabel;
    public GameObject PanelArrow;
    private float ArrowInitY;

    //Create & Edit Preset Form
    public Text PresetOptLabel;
    public InputField PresetNameInput;
    public Button PlayBtn;
    public Button SaveBtn;
    public Button DeleteBtn;

    public ObjectPooler ItemPooler;
    // private List<SourceListItem> ContentListItems;
    // private SourceListItem SelectedItem;
    private List<SourceListItem> ContentListItems;
    private SourceListItem SelectedItem;
    private float PresetYBuffer;
    private float InitialContentListHeight;
    private float InitialContentListYOffset;
    private float SourceListItemWidth;

    public void Initialize() 
    {
        ContentListItems = new List<SourceListItem>();
        GameDataManager.GM.LoadPresetData();

        var contentListRect = ContentList.GetComponent<RectTransform>();
        InitialContentListHeight = contentListRect.rect.height;
        InitialContentListYOffset = contentListRect.offsetMin.y;
        SourceListItemWidth = contentListRect.rect.width;

        ArrowInitY = PanelArrow.transform.localPosition.y;

        //Always add Mods folder at top
        DefaultItem.gameObject.SetActive(true);
        DefaultItem.Toggle.enabled = false;
        DefaultItem.Toggle.onValueChanged.AddListener(delegate { OnItemSelect(DefaultItem); });
        SelectedItem = DefaultItem;
        ContentListItems.Add(DefaultItem);

        PresetLabel.SetActive(true);

        //Y Space taken up by Default and Presets label + 10
        PresetYBuffer = DefaultItem.GetComponent<RectTransform>().rect.height
            + PresetLabel.GetComponent<RectTransform>().rect.height + 10;
        
        var index = 0;

        foreach(var presetName in GameDataManager.GM.ParsedPresetData.Keys.OrderBy(x => x))
        {
            var sourceListItem = CreateSourceListItem(presetName, index);
            sourceListItem.Toggle.isOn = false;

            index++;
        }

        ResizeScrollView();
    }

    public string GetSelectedMods()
    {
        return SelectedItem.Value;
    }
    public string GetSelectedMusic()
    {
        return SelectedItem.MusicValue;
    }

    //Selecting a mod or preset in the list
    public void OnItemSelect(SourceListItem sourceListItem)
    {
        if(!sourceListItem.Toggle.isOn) { return; }

        //Turn off previously selected item
        var prevSelectedItem = ContentListItems.First(x => x != sourceListItem && x.Toggle.isOn);
        prevSelectedItem.Toggle.isOn = false;
        prevSelectedItem.Toggle.enabled = true;

        sourceListItem.Toggle.enabled = false;
        SelectedItem = sourceListItem;
        MoveArrowToSelectedItem();

        //Hand off ModListStr to ModPanel
        var allModsFound = ModPanel.SetModsByString(SelectedItem.Value);
        var allMusicFound = MusicPanel.SetItemsByString(SelectedItem.MusicValue);

        if(sourceListItem == DefaultItem)
        {
            PresetOptLabel.text = "Create Preset:";
            PresetNameInput.text = string.Empty;
            SaveBtn.interactable = false;
            DeleteBtn.gameObject.SetActive(false);
        }
        else
        {
            PresetOptLabel.text = "Edit Preset:";
            PresetNameInput.text = sourceListItem.Name;
            CheckSaveBtn();
            DeleteBtn.gameObject.SetActive(true);

            if(!allModsFound || !allMusicFound)
            {
                OnSavePreset();
            }
        }
        CheckPlayBtn();
    }

    //When a mod is turned on or off
    public void OnModToggle(bool isOn, string modPath, bool isCharacter)
    {
        var existingStr = isCharacter ? SelectedItem.Value : SelectedItem.MusicValue;

        //Adding checked mod
        if(isOn)
        {
            existingStr += !string.IsNullOrWhiteSpace(existingStr) ? "," + modPath : modPath;
        }
        //Removing unchecked mod
        else
        {
            var index = existingStr.IndexOf(modPath);
            var nextCommaIndex = existingStr.IndexOf(',', index);
            if(nextCommaIndex != -1)
            {
                existingStr = existingStr.Remove(index, nextCommaIndex - index + 1);
            }
            else
            {
                var prevCommaIndex = existingStr.LastIndexOf(',', index);

                if(prevCommaIndex == -1) { existingStr = string.Empty; }
                else { 
                    existingStr = existingStr.Remove(prevCommaIndex, modPath.Length + index - prevCommaIndex); 
                }
            }
        }

        //Set the updated string
        if(isCharacter) {
            SelectedItem.Value = existingStr;
        } else {
            SelectedItem.MusicValue = existingStr;
        }

        CheckSaveBtn();
    }

    public void OnSavePreset()
    {
        var inputName = PresetNameInput.text;

        if(ContentListItems.Any(x => x.Name == inputName && x != SelectedItem))
        {
            //TODO: Warning that already have a preset of the same name
            return;
        }
        else if(SelectedItem != DefaultItem)
        {
            GameDataManager.GM.ParsedPresetData.Remove(SelectedItem.Name);

            SelectedItem.Name = inputName;
            SelectedItem.GetComponentInChildren<Text>().text = inputName;
        }

        //Save Data
        GameDataManager.GM.AddPresetData(inputName, SelectedItem.Value, SelectedItem.MusicValue);

        if(SelectedItem == DefaultItem)
        {
            var sourceListItem = CreateSourceListItem(inputName, ContentListItems.Count-1);
            sourceListItem.Toggle.SetIsOnWithoutNotify(false);
            ResizeScrollView();
        }

        ReorderListItems();
        MoveArrowToSelectedItem();
    }

    public void OnDeletePreset()
    {
        //Delete Data
        GameDataManager.GM.DeletePresetData(SelectedItem.Name);

        var deletedItem = SelectedItem;
        DefaultItem.Toggle.isOn = true;

        DeleteSourceListItem(deletedItem);

        ReorderListItems();
        ResizeScrollView();
        MoveArrowToSelectedItem();
    }

    public void CheckSaveBtn()
    {
        if(SaveBtn.interactable == false && !string.IsNullOrWhiteSpace(SelectedItem.Value)
            && !string.IsNullOrWhiteSpace(PresetNameInput.text))
        {
            SaveBtn.interactable = true;
        }
        else if(SaveBtn.interactable == true && (string.IsNullOrWhiteSpace(SelectedItem.Value)
            || string.IsNullOrWhiteSpace(PresetNameInput.text)))
        {
            SaveBtn.interactable = false;
        }
    }

    public void CheckPlayBtn()
    {
        if(PlayBtn.interactable == false && !string.IsNullOrWhiteSpace(SelectedItem.Value)
            && !string.IsNullOrWhiteSpace(SelectedItem.MusicValue))
        {
            PlayBtn.interactable = true;
        }
        else if(PlayBtn.interactable == true && string.IsNullOrWhiteSpace(SelectedItem.Value)
            || string.IsNullOrWhiteSpace(SelectedItem.MusicValue))
        {
            PlayBtn.interactable = false;
        }
    }

    private SourceListItem CreateSourceListItem(string presetName, int listIndex)
    {
        var sourceListItem = ItemPooler.GetPooledObject("SourceListItem", ContentList.transform).GetComponent<SourceListItem>();
        sourceListItem.Name = presetName;
        sourceListItem.Label.text = presetName;

        //Set Char & Music Values
        var values = GameDataManager.GM.ParsedPresetData[presetName].Split('>');
        sourceListItem.Value = values[0];
        if(values.Length > 1) 
        {
            var musicNames = values[1];
            var isFirst = true;

            foreach(var musicName in musicNames.Split(','))
            {
                if(MusicPanel.HasItemWithValue(musicName)) 
                {
                    sourceListItem.MusicValue += isFirst ? musicName : "," + musicName;
                    if(isFirst) { isFirst = false; }
                }
            }
        }

        //Set Transform & Rect
        sourceListItem.transform.localPosition = new Vector3(0, -45f * listIndex - PresetYBuffer, 0);

        var prevHeight = sourceListItem.RectTransform.rect.height;
        sourceListItem.RectTransform.sizeDelta = new Vector2(SourceListItemWidth, prevHeight);

        sourceListItem.Toggle.onValueChanged.AddListener(delegate { 
            OnItemSelect(sourceListItem); });
        ContentListItems.Add(sourceListItem);

        return sourceListItem;
    }

    private void DeleteSourceListItem(SourceListItem item)
    {
        ContentListItems.Remove(item);

        item.Toggle.onValueChanged.RemoveAllListeners();
        item.Toggle.SetIsOnWithoutNotify(false);
        item.MusicValue = null;
        item.Name = null;
        item.Value = null;
        ItemPooler.RepoolObject(item.gameObject);
    }

    private void ReorderListItems()
    {
        var index = 0;

        foreach(var item in ContentListItems.Where(x => x != DefaultItem).OrderBy(x => x.Name))
        {
            var pos = new Vector3(0, -45f * index - PresetYBuffer, 0);
            if(item.transform.localPosition != pos) { item.transform.localPosition = pos; }

            index++;
        }
    }

    private void ResizeScrollView(bool moveUp = false)
    {
        var contentListRect = ContentList.GetComponent<RectTransform>();

        var heightDiff = PresetYBuffer + ((ContentListItems.Count-1) * 45f) 
            - InitialContentListHeight - contentListRect.localPosition.y;

        if(heightDiff > 0) {
            contentListRect.offsetMin = new Vector2(contentListRect.offsetMin.x, InitialContentListYOffset - heightDiff);
        }
    }

    private void MoveArrowToSelectedItem()
    {
        PanelArrow.transform.localPosition = new Vector3(
            PanelArrow.transform.localPosition.x, 
            ArrowInitY + SelectedItem.transform.localPosition.y, 0);
    }
}
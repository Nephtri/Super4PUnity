using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string ModName;
    public string ModPath;
    public Toggle Toggle;
    public Text Label;
    public GameObject ModPreviewPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {  
        var modImg = ModPreviewPanel.transform.GetChild(0).GetComponent<Image>();
        modImg.sprite = SpriteLibrary.SL.GetSpriteByName(ModName);
        ModPreviewPanel.SetActive(true);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {    
        ModPreviewPanel.SetActive(false);
    }
}
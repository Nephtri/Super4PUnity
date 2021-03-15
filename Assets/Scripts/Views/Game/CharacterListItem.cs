using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItem : MonoBehaviour
{
    public RectTransform RectTrans;
    public Button CharBtn;
    public Image CharImg;
    
    public string CharacterName;
    public List<string> VideoPaths;
    //TODO: Implement VoicePaths logic
    public List<string> VoicePaths;
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterListItem : MonoBehaviour
{
    public RectTransform RectTrans;
    public Button CharBtn;
    public Image CharImg;
    
    public string CharacterName;
    public List<string> VideoPaths;
    //TODO: Implement VoicePaths logic
    //public List<string> VoicePaths;

    public List<CharPoseData> PoseDataList;
    public int PoseCount => PoseDataList.Count;
}
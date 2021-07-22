using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [HideInInspector]
    public Text roomDataTxt;
    
    [HideInInspector]
    public string roomName;
    
    [HideInInspector]
    public int playerCount;
    
    [HideInInspector]
    public int maxPlayer;

    private void Awake()
    {
        roomDataTxt = GetComponentInChildren<Text>();
    }

    public void UpdateInfo()
    {
        roomDataTxt.text = string.Format(" {0} [{1} / {2}]", roomName, playerCount.ToString("00"),maxPlayer);
    }
    
}

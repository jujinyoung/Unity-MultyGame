using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//PhotonInit
public class PhotonInit : MonoBehaviourPunCallbacks
{
    public enum ActivePanel{LOGIN,ROOMS}
    private string gameVersion = "1.0";
    public byte maxPlayer = 20;
    public InputField User_ID;
    public InputField Room_ID;
    public GameObject[] panels;
    public GameObject room;
    public Transform gridTr;
    bool check = false;
    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(1280,720,false);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Start()
    {
        User_ID.text = PlayerPrefs.GetString("USER_ID", "USER_" + Random.Range(1,999));
        Room_ID.text = PlayerPrefs.GetString("Room_ID", "ROOM_" + Random.Range(1,999));
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();              //OnJoinedLobby()콜백함수 
        check = true;
    }

    // public override void OnJoinRandomFailed(short returnCode, string message)
    // {
    //     Debug.Log("Failed Join room!!!");
    //     PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = this.maxPlayer});
    // }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room!!!");
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("MainGame");
    }

    public void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = User_ID.text;
        PhotonNetwork.ConnectUsingSettings();

        PlayerPrefs.SetString("User_ID", PhotonNetwork.NickName);
        ChangePanel(ActivePanel.ROOMS);
    }

    void ChangePanel(ActivePanel panel)
    {
        foreach(GameObject _panel in panels)
        {
            _panel.SetActive(false);
        }
        panels[(int)panel].SetActive(true);
    }

    public void OnCreateRoomClick()
    {
        if(check == true)
        {
            PhotonNetwork.CreateRoom(Room_ID.text, new RoomOptions{MaxPlayers = this.maxPlayer});
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
        {
            Destroy(obj);
        }
        foreach(RoomInfo roomInfo in roomList)
        {
            Debug.Log("실행");
            GameObject _room = Instantiate(room, gridTr);
            RoomData roomData = _room.GetComponent<RoomData>();
            roomData.roomName = roomInfo.Name;
            roomData.maxPlayer = roomInfo.MaxPlayers;
            roomData.playerCount = roomInfo.PlayerCount;
            roomData.UpdateInfo();
            roomData.GetComponent<Button>().onClick.AddListener
            (
                delegate
                {
                    OnClickRoom(roomData.roomName);
                }
            );
        }
    }

    void OnClickRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName, null);
    }

    public void Backspace()
    {
        PhotonNetwork.Disconnect();
        ChangePanel(ActivePanel.LOGIN);
    }
}

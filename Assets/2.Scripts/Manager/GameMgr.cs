using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameMgr : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //플레이어생성
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(0,3), 0, Random.Range(0,3)), Quaternion.identity);
        PhotonNetwork.IsMessageQueueRunning = true;
    }
}

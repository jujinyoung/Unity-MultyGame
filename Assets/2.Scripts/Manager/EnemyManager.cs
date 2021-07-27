using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnemyManager : MonoBehaviourPunCallbacks
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn",spawnTime,spawnTime);
    }

    void Spawn()
    {
        //플레이어가 사망시 적 캐릭터 소환x
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

        //SpawnPoint를 기준으로 랜덤 위치에서 생성
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        PhotonNetwork.Instantiate("Zombunny", spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        // Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}

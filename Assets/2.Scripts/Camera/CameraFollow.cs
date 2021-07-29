using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraFollow : MonoBehaviourPunCallbacks
{
    public GameObject target;  
    public float smoothing = 5.0f;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newCamPos = target.transform.position + offset;
        //Vector3.Lerp(시작지전, 도착지점, 시간) => 선형보간법으로 한 템포 늦게 플레이어를 따라감(부드럽게 추적)
        transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing *Time.deltaTime);
    }
}

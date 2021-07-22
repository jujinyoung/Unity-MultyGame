using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float speed = 7.0f;      //플레이어 이동속도
    
    Animator animator;              //플레이어 애니메이터
    Rigidbody rigidBody;            //플레이어 리지드바디
    Vector3 movement;               //플레이어 위치
    float camRayLength = 100.0f;    //카메라 레이캐스트 거리
    int floorMask;                  // 레이어

    public TextMeshProUGUI nickName;    

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");   

        if(photonView.IsMine)
        {
            Camera.main.GetComponent<CameraFollow>().target = gameObject;
        }     

        nickName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h,v);        
        Turning();
        Anim(h,v);
    }

    void Move(float h, float v)
    {
        //Vector.set(x,y,z) : 벡터값을 세팅
        movement.Set(h,0,v);

        //normalized : 방향을 가지고옴, Time.deltaTime: 기기에 따른 보정값
        movement = movement.normalized * speed * Time.deltaTime;

        //MovePosition : rigidvody를 이용해서 물체를 이동시킴
        //transform.position : 이 스크립트 객체의 포지션
        // rigidBody.MovePosition(transform.position + movement);
        rigidBody.position += movement;
    }

    void Turning()
    {
        //마우스 위치로 Ray를 만듬
        //ScreenPointToRay : 2d 화면을 클릭했을 떄 3d 좌표계로 계산
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //레이캐스팅 => 만들어진 레이를 발사해서 충돌되는 객체가 있는지 판단
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, camRayLength, floorMask))
        {
            Vector3 playerToMouse = hitInfo.point - transform.position;
            playerToMouse.y = 0;

            Quaternion rot = Quaternion.LookRotation(playerToMouse);
            
            //회전값 적용
            rigidBody.MoveRotation(rot);
        }

        //레이 그리기
        //시작위치, 방향, 길이, 색깔, 유지시간
        Debug.DrawRay(ray.origin, ray.direction * camRayLength, Color.red, 0.1f);
    }

    void Anim(float h,float v)
    {
        bool isWalking = (h!=0 || v!=0);
        animator.SetBool("IsWalking",isWalking);
    }
}

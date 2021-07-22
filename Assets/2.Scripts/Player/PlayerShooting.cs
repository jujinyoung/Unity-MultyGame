using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerShooting : MonoBehaviourPunCallbacks
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.5f;
    public float range = 100f;
    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    bool check = true;

    
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        timer += Time.deltaTime;

        if(Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            photonView.RPC("Shoot",RpcTarget.Others,null);
            Shoot();
        }

        if(timer >= timeBetweenBullets*effectsDisplayTime && check == true)
        {
            photonView.RPC("DisableEffects",RpcTarget.Others,null);
            DisableEffects();
        }
    }

    [PunRPC]
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
        check =false;
    }

    [PunRPC]
    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0,transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast(shootRay, out shootHit, range))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot,shootHit.point);
            }
            gunLine.SetPosition(1,shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1,shootRay.origin + shootRay.direction * range);
        }
        check = true;
    }
}

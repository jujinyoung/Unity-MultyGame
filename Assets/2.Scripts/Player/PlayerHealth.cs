using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public int startingHealth = 100;
    public int currentHealth;
    Slider healthSlider;
    Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f,0f,0f,0.1f);
    GameOverManager gameOverManager;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;

    bool isDead = false;    //죽음 유무
    bool damaged = false;   //데미지 유무

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        healthSlider = GameObject.Find("Slider").GetComponent<Slider>();
        damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
        gameOverManager = GameObject.Find("Canvas").GetComponent<GameOverManager>();
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        if(damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed*Time.deltaTime);
        }

        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        playerAudio.Play();
        if(currentHealth <= 0 && !isDead)
        {
            Death();
            gameOverManager.GameOver(1.0f,2.0f);
        }
    }

    void Death()
    {
        isDead = true;

        playerShooting.DisableEffects();

        anim.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        //0번쨰 씬 재시작
        SceneManager.LoadScene(0);
    }
}

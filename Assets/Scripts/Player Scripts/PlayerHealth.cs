using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    [SerializeField] float timeSinceLastHit = 2.0f;

    [SerializeField] Slider healthSlider;

    private CharacterMovement characterMovement;
    public float timer = 0f;
    private Animator animator;
    [SerializeField] /*private*/ int currentHealth;

    private AudioSource audio;
    public AudioClip hurtAudio;
    public AudioClip deadPlayerAudio;
    public AudioClip pickItem;

    public LevelManager levelManager;

    public bool isDead;

    public Slider HealthSlider
    {
        get { return healthSlider;  }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value < 0)
            {
                currentHealth = 0;
            } else
            {
                currentHealth = value;
            }
        }
    }


    private void Awake()
    {
        Assert.IsNotNull(healthSlider);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = startingHealth;
        characterMovement = GetComponent<CharacterMovement>();

        audio = GetComponent<AudioSource>();

        levelManager = FindObjectOfType<LevelManager>();

        isDead = false;
    }

    public void PlayerKill()
    {
        if(currentHealth <= 0)
        {
            characterMovement.enabled = false;
            levelManager.RespawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        PlayerKill();
    }

    void OnTriggerEnter(Collider other)
    {
        if(timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "Weapon")
            {
                takeHit();
                timer = 0;
            }
        }
        
    }

    void takeHit()
    {
        if(currentHealth > 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            animator.Play("PlayerHurt");
            
            currentHealth -= 10;

            healthSlider.value = currentHealth;

            audio.PlayOneShot(hurtAudio);
        }
        
        if(currentHealth <= 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            animator.SetTrigger("isDead");
            characterMovement.enabled = false;

            audio.PlayOneShot(deadPlayerAudio);
        }
        
    }

    public void PowerUpHealth()
    {
        if(currentHealth <= 80)
        {
            CurrentHealth += 20;
        }
        else if(currentHealth < startingHealth)
        {
            CurrentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
        audio.PlayOneShot(pickItem);
        //animator.SetTrigger("isHealthItem");
    }

    public void KillBox()
    {
        currentHealth = 0;
        healthSlider.value = currentHealth;
    }
}

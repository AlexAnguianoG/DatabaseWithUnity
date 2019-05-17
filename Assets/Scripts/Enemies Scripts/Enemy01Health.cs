using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Data;
using System.Data.SqlClient;
using System;

public class Enemy01Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 2f;
    [SerializeField] int currentHealth;

    private float timer = 0f;
    private Animator animator;
    private bool isAlive;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
 

    private BoxCollider weaponCollider;

    private AudioSource audio;
    public AudioClip deadEnemyAudio;
    public AudioClip hurtEnemyAudio;
    

    private DropItems dropItems;

    private CharacterMovement characterMovement;

    string wep;
    string tagWeapon;

    public int IdEnm = 1;

    public bool IsAlive
    {
        get { return isAlive; }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        isAlive = true;
        currentHealth = startingHealth;

        weaponCollider = GetComponentInChildren<BoxCollider>();

        audio = GetComponent<AudioSource>();

        dropItems = GetComponent<DropItems>();


       
    }

    // Update is called once per frame
    void Update()
    {
        //wep = characterMovement.Wept;
        timer += Time.deltaTime;

        if (dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
    }

    /*private*/ void OnTriggerEnter(Collider other)
    {
        if(timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if(other.tag == "Hammer" || other.tag == "Sword")
            {
                tagWeapon = other.tag;
                takeHit();
                timer = 0f;
            }
        }
    }
    void takeHit()
    {
        if(currentHealth > 0)
        {
            animator.Play("EnemyHurt");
            currentHealth -= 10;
        }

        if(currentHealth <= 0)
        {
            isAlive = false;
            KillEnemy();
            audio.PlayOneShot(hurtEnemyAudio);
        }
    }

    void KillEnemy()
    {

        string date = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

        string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
        SqlConnection dbConnection = new SqlConnection(conn);
        try
        {

            dbConnection.Open();
            Debug.Log("Connected to database.");
            using (SqlCommand command = new SqlCommand("dbo.killMon", dbConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdEnem", 1);
                command.Parameters.AddWithValue("@Desdate", date);
                command.Parameters.AddWithValue("@UsedWep", tag);
                command.ExecuteNonQuery();
            }

        }
        catch (SqlException _exception)
        {
            Debug.LogWarning(_exception.ToString());
        }

        capsuleCollider.enabled = false;
        animator.SetTrigger("EnemyDie");
        rigidbody.isKinematic = true;

        weaponCollider.enabled = false;

        StartCoroutine(removeEnemy());

        audio.PlayOneShot(deadEnemyAudio);

        dropItems.Drop();
        
    }

 

    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds(2f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

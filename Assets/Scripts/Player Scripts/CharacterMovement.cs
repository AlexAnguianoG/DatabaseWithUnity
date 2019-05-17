using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Data;
using System.Data.SqlClient;


public class CharacterMovement : MonoBehaviour
{   
    //RUN
    public float maxSpeed = 6.0f;
    public bool facingRight = true;
    public float moveDirection;
    new Rigidbody rigidbody;

    //ANIMATOR
    private Animator animator;

    //JUMP
    public float jumpSpeed = 800.0f;

    //GROUNDED?
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    //WEAPON
    public float weaponSpeed = 600.0f;
    public Transform weaponSpawn;
    public Rigidbody Hammer;
    public Rigidbody Sword;

    Rigidbody clone;

    //AUDIO
    private AudioSource audio;
    public AudioClip swordAudio;
    public AudioClip jumpAudio;

    //DATABASE VARIABLES
    public int ammoNormal1;
    public int usedAmmoNormal1;
    public int ammoExplosive1;
    public int usedAmmoExplosive1;

    public int ammoNormal2;
    public int usedAmmoNormal2;
    public int ammoExplosive2;
    public int usedAmmoExplosive2;
    public string typewep;
    public int fa = 0;



    private void Awake()
    {
        groundCheck = GameObject.Find("GroundCheck").transform;
        weaponSpawn = GameObject.Find("WeaponSpawn").transform;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Example());

        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        audio = GetComponent<AudioSource>();

        try
        {
            string connr = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
            using (SqlConnection connection = new SqlConnection(connr))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT dbo.ammoSet", connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    fa = (Int32)command.ExecuteScalar();
                    print(fa);
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }


        ammoNormal1 = 300;
        usedAmmoNormal1 = 0;
        ammoExplosive1 = 300;
        usedAmmoExplosive1 = 0;

        ammoNormal2 = 300;
        usedAmmoNormal2 = 0;
        ammoExplosive2 = 300;
        usedAmmoExplosive2 = 0;
    
        string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
        SqlConnection dbConnection = new SqlConnection(conn);
        try
        {
            dbConnection.Open();
            Debug.Log("Connected to database.");
            using (SqlCommand command = new SqlCommand("dbo.addAmmo", dbConnection))
            { 
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@type1", "Normal1");
                command.Parameters.AddWithValue("@type2", "Explosive1");
                command.Parameters.AddWithValue("@type3", "Normal2");
                command.Parameters.AddWithValue("@type4", "Explosive2");
                command.Parameters.AddWithValue("@quantity", fa);
                command.ExecuteNonQuery();  
            }

        }
        catch (SqlException _exception)
        {
            Debug.LogWarning(_exception.ToString());
        }
    }

    
    IEnumerator Example()
    {
        yield return new WaitForSeconds(5);

        string posx = (rigidbody.position.x).ToString();
        string posy = (rigidbody.position.y).ToString();
        string Date = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

        string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
        SqlConnection dbConnection = new SqlConnection(conn);
        try
        {
            dbConnection.Open();
            Debug.Log("Connected to database.");
            using (SqlCommand command = new SqlCommand("dbo.InsertLocation", dbConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@date", Date);
                command.Parameters.AddWithValue("@coordX", posx);
                command.Parameters.AddWithValue("@coordY", posy);
                command.ExecuteNonQuery();
            }

        }
        catch (SqlException _exception)
        {
            Debug.LogWarning(_exception.ToString());
        }


        yield return null;
        StartCoroutine(Example());

    }
    // Update is called once per frame
    void Update()
    {

        
        moveDirection = CrossPlatformInputManager.GetAxis("Horizontal");

        if ((grounded) && CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJumping");
            rigidbody.AddForce(new Vector2(0, jumpSpeed));

            audio.PlayOneShot(jumpAudio);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            typewep = "Hammer";
            Attack();

            ammoNormal1--;
            usedAmmoNormal1++;
            

            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
            SqlConnection dbConnection = new SqlConnection(conn);
            try
            {
                dbConnection.Open();
                using (SqlCommand command = new SqlCommand("dbo.addUsedAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Normal1");
                    command.Parameters.AddWithValue("@amount", usedAmmoNormal1);
                    command.ExecuteNonQuery();
                }
                
                using (SqlCommand command = new SqlCommand("dbo.addiAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Normal1");
                    command.Parameters.AddWithValue("@quantity", ammoNormal1);
                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException _exception)
            {
                Debug.LogWarning(_exception.ToString());
            }

        }



        if (Input.GetKeyDown(KeyCode.S))
        {
            typewep = "Hammer";
            Attack();

            ammoExplosive1--;
            usedAmmoExplosive1++;
            

            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
            SqlConnection dbConnection = new SqlConnection(conn);
            try
            {
                dbConnection.Open();
                using (SqlCommand command = new SqlCommand("dbo.addUsedAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Explosive1");
                    command.Parameters.AddWithValue("@amount", usedAmmoExplosive1);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("dbo.addiAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Explosive1");
                    command.Parameters.AddWithValue("@quantity", ammoExplosive1);
                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException _exception)
            {
                Debug.LogWarning(_exception.ToString());
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            typewep = "Sword";
            Attack();

            ammoNormal2--;
            usedAmmoNormal2++;
            

            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
            SqlConnection dbConnection = new SqlConnection(conn);
            try
            {
                dbConnection.Open();
                using (SqlCommand command = new SqlCommand("dbo.addUsedAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Normal2");
                    command.Parameters.AddWithValue("@amount", usedAmmoNormal2);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("dbo.addiAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Normal2");
                    command.Parameters.AddWithValue("@quantity", ammoNormal2);
                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException _exception)
            {
                Debug.LogWarning(_exception.ToString());
            }

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            typewep = "Sword";
            Attack();

            ammoExplosive2--;
            usedAmmoExplosive2++;
            

            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
            SqlConnection dbConnection = new SqlConnection(conn);
            try
            {
                dbConnection.Open();
                using (SqlCommand command = new SqlCommand("dbo.addUsedAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Explosive2");
                    command.Parameters.AddWithValue("@amount", usedAmmoExplosive2);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("dbo.addiAmmo", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", "Explosive2");
                    command.Parameters.AddWithValue("@quantity", ammoExplosive2);
                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException _exception)
            {
                Debug.LogWarning(_exception.ToString());
            }

        }


    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }

    void Attack()
    {
        animator.SetTrigger("Attacking");

        

    }

    public void CallFireProyectile()
    {

        if (typewep == "Hammer")
        {
            clone = Instantiate(Hammer, weaponSpawn.position, weaponSpawn.rotation) as Rigidbody;
        }
        else if (typewep == "Sword")
        {
            clone = Instantiate(Sword, weaponSpawn.position, Quaternion.Euler(0, 90, 90)) as Rigidbody;
        }
        clone.AddForce(weaponSpawn.transform.right * weaponSpeed);


        audio.PlayOneShot(swordAudio);
    } 

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        rigidbody.velocity = new Vector2(moveDirection * maxSpeed, rigidbody.velocity.y);

        if(moveDirection > 0.0f && !facingRight)
        {
            Flip();
        } else if (moveDirection < 0.0f && facingRight)
        {
            Flip();
        }
        animator.SetFloat("Speed", Mathf.Abs(moveDirection));
    }


    public string Wept
    {
        get { return typewep; }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using System.Data.SqlClient;
using System;

public class Text : MonoBehaviour
{
    //public int startingLives;
    //private int lifeCounter;
    //private Text lifeText;

    private GameObject player;
    //public GameObject gameOverScreen;
    //public float waitAfterGameOver;

    // Start is called before the first frame update
    void Start()
    {
        //lifeText = GetComponent<Text>();
        //lifeCounter = startingLives;

        player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        //lifeText.text = "X" + lifeCounter;


    }

    public void OnTriggerEnter(Collider other)
    {
        
        try
        {
            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                Console.WriteLine("Done.");

                string sql = "SELECT * FROM ViewEnemy"; //ya se ve más bonito


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //command.Parameters.Add()
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0].ToString());
                            Console.WriteLine(reader[1].ToString());
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

}

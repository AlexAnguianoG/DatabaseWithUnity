using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;




public class SQLCONN : MonoBehaviour
{

    private GameObject player;

    void Start()
    {
        player = GameManager.instance.Player;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {

            //string source = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=c:\x\x\documents\visual studio 2013\Projects\WebApplication3\WebApplication3\App_Data\Product.mdf;Integrated Security=True";

            //string conn = @"Server=local\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

            //string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;User Id=alexag; Password = 1234";


            //string conn = @"Data Source = 127.0.0.1; user id = assassin; password = creed; Initial Catalog = DemoUsandoCS;";

            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";

            SqlConnection dbConnection = new SqlConnection(conn);


            try
            {

                dbConnection.Open();
                Debug.Log("Connected to database.");
                using (SqlCommand command = new SqlCommand("dbo.InsertPlayer", dbConnection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@idPlayer", "4");
                    command.Parameters.AddWithValue("@health", "100");

                    command.ExecuteNonQuery();
                    Console.WriteLine("Lista escritura con un SP.");
                }

            }
            catch (SqlException _exception)
            {
                Debug.LogWarning(_exception.ToString());

            }
            
            /*
            try
            {
                string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();
                    Console.WriteLine("Done.");


                    using (SqlCommand command = new SqlCommand("dbo.InsertPlayer", connection))
                    { 
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idPlayer", "235");
                        command.Parameters.AddWithValue("@health", "100");

                        command.ExecuteNonQuery();
                        Console.WriteLine("Lista escritura con un SP.");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            */
            Destroy(gameObject);
        }
    }
}

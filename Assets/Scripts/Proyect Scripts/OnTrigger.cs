using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;

public class OnTrigger : MonoBehaviour
{
    private GameObject player;

    new Rigidbody rigidbody;
    string posx;
    string posy;
    string type;

    void Start()
    {
        player = GameManager.instance.Player;
        rigidbody = GetComponent<Rigidbody>();
        posx = (rigidbody.position.x).ToString();
        posy = (rigidbody.position.y).ToString();
        type = gameObject.name;

    }

    void Update()
    {

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            string conn = @"Data Source = 127.0.0.1; user id = GM; password = 1234; Initial Catalog = Videogame;";
            SqlConnection dbConnection = new SqlConnection(conn);
            try
            {
                dbConnection.Open();
                Debug.Log("Connected to database.");
                using (SqlCommand command = new SqlCommand("dbo.InsertPowerup", dbConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@color", "Brown");
                    command.Parameters.AddWithValue("@coordX", posx);
                    command.Parameters.AddWithValue("@coordY", posy);
                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException _exception)
            {
                Debug.LogWarning(_exception.ToString());
            }
        }

        Destroy(gameObject);
    }

}

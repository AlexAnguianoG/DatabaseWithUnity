using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameManager.instance.Player;
    }

    void Update()
    {

    }


}

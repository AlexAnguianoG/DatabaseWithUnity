﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range = 3f;
    [SerializeField] private float timeBetweenAttacks = 1f;

    private Animator animator;
    private GameObject player;
    private bool playerInRange;
    private BoxCollider weaponCollider;

    private Enemy01Health enemy01Health;


 


    // Start is called before the first frame update
    void Start()
    {
        
        enemy01Health = GetComponent<Enemy01Health>();
        animator = GetComponent<Animator>();
        player = GameManager.instance.Player;
        weaponCollider = GetComponentInChildren<BoxCollider>();
        StartCoroutine(attack());

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance (transform.position, player.transform.position) < range && enemy01Health.IsAlive)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
        //print("Player in range" + playerInRange);
    }

    IEnumerator attack()
    {
        if(playerInRange && !GameManager.instance.GameOver)
        {
            animator.Play("EnemyAttack");
            yield return new WaitForSeconds(timeBetweenAttacks);

        }

        yield return null;
        StartCoroutine(attack());
    }
}

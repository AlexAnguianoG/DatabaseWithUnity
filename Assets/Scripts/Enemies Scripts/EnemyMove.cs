using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform player;
    private NavMeshAgent nav;
    private Animator animator;

    private Enemy01Health enemy01Health;

    private void Awake()
    {
        Assert.IsNotNull(player);
    }

    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        enemy01Health = GetComponent<Enemy01Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, this.transform.position) < 6) {
            if (!GameManager.instance.GameOver && enemy01Health.IsAlive)
            {
                nav.SetDestination(player.position);
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
            }

        }

        if(GameManager.instance.GameOver || !enemy01Health.IsAlive)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
            nav.enabled = false;
        }
        
    }
}

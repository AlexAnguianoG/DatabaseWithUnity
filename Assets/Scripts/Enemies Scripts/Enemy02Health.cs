using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Enemy02Health : MonoBehaviour
{
    private NavMeshAgent nav;

    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 1.2f;
    [SerializeField] int currentHealth;

    private float timer = 0f;
    private Animator animator;
    private bool isAlive;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;

    private AudioSource audio;
    public AudioClip deadEnemyAudio;
    public AudioClip hurtEnemyAudio;

    private DropItems dropItems;

    public bool IsAlive
    {
        get { return isAlive; }
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        isAlive = true;
        currentHealth = startingHealth;

        dropItems = GetComponent<DropItems>();

        //audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if(other.tag == "Hammer" || other.tag == "Sword")
            {
                takeHit();
                timer = 0f;
            }
        }
    }

    void takeHit()
    {
        if (currentHealth > 0)
        {
            animator.Play("ArcherHurt");
            currentHealth -= 10;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            KillEnemy();
            //audio.PlayOneShot(hurtEnemyAudio);
        }
    }

    void KillEnemy()
    {
        capsuleCollider.enabled = false;
        animator.SetTrigger("ArcherDie");
        rigidbody.isKinematic = true;

        StartCoroutine(removeEnemy());

        dropItems.Drop();
        //audio.PlayOneShot(deadEnemyAudio);
    }

    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds(2f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

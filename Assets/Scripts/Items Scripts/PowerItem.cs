using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItem : MonoBehaviour
{
    private GameObject player;
    private AudioSource audio;
    public AudioClip pickItem;
    //private Invincible invincible;
    private PlayerHealth playerHealth;

    private ParticleSystem particleSystem;

    private MeshRenderer meshRenderer;
    private ParticleSystem brainParticles;

    public GameObject pickupEffect;

    private PowerItemExplode powerItemExplode;
    private SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.enabled = true;

        particleSystem = player.GetComponent<ParticleSystem>();
        particleSystem.enableEmission = false;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        brainParticles = GetComponent<ParticleSystem>();

        powerItemExplode = GetComponent<PowerItemExplode>();
        sphereCollider = GetComponent<SphereCollider>();

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            audio.PlayOneShot(pickItem);
            StartCoroutine(InvincibleRoutine());
            meshRenderer.enabled = false;
            
        }
    }

    public IEnumerator InvincibleRoutine()
    {
        //print("PARTICLES");
        powerItemExplode.Pickup();
        particleSystem.enableEmission = true;
        playerHealth.enabled = false;
        brainParticles.enableEmission = false;
        sphereCollider.enabled = false;
        

        yield return new WaitForSeconds(10f);
        //print("no more");
        particleSystem.enableEmission = false;
        playerHealth.enabled = true;
        Destroy(gameObject);
    }

    void Pickup()
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);
    }
}

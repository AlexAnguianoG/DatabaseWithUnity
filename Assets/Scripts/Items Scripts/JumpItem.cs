using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : MonoBehaviour
{
    private GameObject player;
    private CharacterMovement characterMovement;
    private MeshRenderer meshRenderer;

    private ParticleSystem jumpGemParticles;

    public GameObject pickUpEffect;

    private PowerItemExplode powerItemExplode;
    private SphereCollider sphereCollider;

    private AudioSource audio;
    public AudioClip pickItem;

    // Use this for initialization
    void Start()
    {

        player = GameManager.instance.Player;
        characterMovement = player.GetComponent<CharacterMovement>();

        jumpGemParticles = GetComponent<ParticleSystem>();

        meshRenderer = GetComponentInChildren<MeshRenderer>();

        powerItemExplode = GetComponent<PowerItemExplode>();
        sphereCollider = GetComponent<SphereCollider>();

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            audio.PlayOneShot(pickItem);
            StartCoroutine(JumpRoutine());
            meshRenderer.enabled = false;
            
        }
    }

    public IEnumerator JumpRoutine()
    {

        powerItemExplode.Pickup();
        characterMovement.jumpSpeed = 1200f;
        jumpGemParticles.enableEmission = false;
        sphereCollider.enabled = false;
        

        yield return new WaitForSeconds(10f);
        //print("No more jump");
        characterMovement.jumpSpeed = 800f;
        Destroy(gameObject);

    }

    void PickUp()
    {
        Instantiate(pickUpEffect, transform.position, transform.rotation);
    }
}

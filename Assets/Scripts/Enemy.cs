using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Player player;
    private Transform playerTransform;
    public NavMeshAgent agent;
    private int health = 3;
    private Color damageColor = new Color(255, 0, 0);
    private Color deathColor = new Color(0, 0, 0);
    private Color baseColor = Color.white;
    [SerializeField] private AudioSource hitMarkerSound;

    void Start()
    {
        player = GameObject.Find("/XR Player Character/AutoHandPlayer").GetComponent<Player>();
        playerTransform = Camera.main.GetComponent<Transform>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
            playerTransform = Camera.main.GetComponent<Transform>();
        }

        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void HitByRay()
    { 
        if(health > 1)
        {
            StartCoroutine("OnHit");
        }
        else
        {
            StartCoroutine("OnDeath");
        }
    }

    private IEnumerator OnHit()
    {
        health -= 1;
        player.SendMessage("OnZombieHit");
        GetComponent<Renderer>().material.color = damageColor;
        hitMarkerSound.Play();
        yield return new WaitForSeconds(0.25f);
        GetComponent<Renderer>().material.color = baseColor;
    }

    private IEnumerator OnDeath()
    {
        player.SendMessage("OnZombieDeath");
        GetComponent<Renderer>().material.color = deathColor;
        hitMarkerSound.Play();
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}

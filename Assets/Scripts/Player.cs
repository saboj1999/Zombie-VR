using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int points;
    private float maxHealth = 200f;
    private float health;
    private GameManager gameManager;
    private float regenCounter;
    private bool stopRegen = false;
    [SerializeField] private AudioSource playerDamageSound;
    [SerializeField] private AudioSource playerHealingSound;

    void Start()
    {
        gameManager = GameObject.Find("/--Managers--/Game Manager").GetComponent<GameManager>();
        health = maxHealth;
        points = 0;
    }

    void Update()
    {
        if(!gameManager.gameOver)
        {
            CheckGameOver();
            CheckRegen();
        }
    }

    private void CheckRegen()
    {
        regenCounter += Time.deltaTime;
        if(regenCounter >= 10f && health < maxHealth)
        {
            regenCounter = 0f;
            stopRegen = false;
            StartCoroutine("RegenerateHealth");
        }
    }

    private void CheckGameOver()
    {
        if(health < 1f) 
        {
            gameManager.GameOver();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy" && !gameManager.gameOver)
        {
            playerDamageSound.Play();
            stopRegen = true;
            health -= 25f;
            regenCounter = 0f;
            Debug.Log("(OUCH)Health: "+health);
        }
    }

    private void OnZombieHit()
    {
        points += 10;
    }

    private void OnZombieDeath()
    {
        points += 100;
    }

    private IEnumerator RegenerateHealth()
    {
        Debug.Log("Im healing!!");

        while(!stopRegen && health < maxHealth)
        {
            playerHealingSound.Play();
            yield return new WaitForSeconds(1);
            health += 12.5f;
            regenCounter = 0f;
            Debug.Log("(HEALING)Health: "+health);
        }
        Debug.Log("Healing was finished or interrupted-> Health: "+health);
        stopRegen = false;
    }
}

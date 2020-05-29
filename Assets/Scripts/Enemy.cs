using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject deadVFX;
    [SerializeField] float timeForDeadVFX = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;
    [SerializeField] int score = 44;
    private void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }
    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if( shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        projectileObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( !collision.GetComponent<DamageDealer>() ) return;
        float damage = collision.GetComponent<DamageDealer>().GetDamage();
        health -= damage;
        if( health <= 0)
        {
            Die();
        }
        Destroy(collision.gameObject);
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(deadVFX, transform.position, transform.rotation);
        Destroy(explosion, timeForDeadVFX);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<GameSession>().AddToScore(score);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configuration parameters")]
    [SerializeField] float speed = 10f;
    float minPosX, maxPosX, minPosY, maxPosY;
    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.25f;
    [Header("Player")]
    [SerializeField] float health = 200f;
    Camera gameCamera;
    float padding;
    Coroutine firingCoroutine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = Camera.main;
        padding = 0.7f;
        SetUpBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if( Input.GetButtonDown("Fire") )
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if(Input.GetButtonUp("Fire"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void Move()
    {
        float newPosX = transform.position.x + (Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);
        float newPosY = transform.position.y + (Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);
        newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);
        newPosY = Mathf.Clamp(newPosY, minPosY, maxPosY);
        transform.position = new Vector2(newPosX, newPosY);
    }

    private void SetUpBoundaries()
    {
        //x
        minPosX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxPosX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        //y
        minPosY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxPosY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<DamageDealer>()) return;
        float damage = collision.GetComponent<DamageDealer>().GetDamage();
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        Destroy(collision.gameObject);
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}

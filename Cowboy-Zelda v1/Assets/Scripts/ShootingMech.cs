﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingMech : MonoBehaviour
{
    private Rigidbody2D rb;

    public float offset;
    public float bulletCount;

    public int maxAmmo = 3;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;
    public Text ammoDisplay;

    public GameObject bulletPrefab;
    public Transform shotPoint;

    public Animator animator;

    private Vector3 movement;   // Must declare outside of functions because I implemented this additively
    private bool isGrounded = true;
    public float jumpTime = 1f;
    public float movementAccel = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        ammoDisplay.text = currentAmmo.ToString();

        movement += new Vector3(Input.GetAxis("Horizontal") * movementAccel, Input.GetAxis("Vertical") * movementAccel, 0.0f);//moves character

        //animation of sprites
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        transform.position = transform.position + movement * Time.deltaTime;

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            FindObjectOfType<AudioManager>().Play("Gun Reload");
            return;
        }

        if (Input.GetMouseButtonDown(0)){//button press to shoot
            GameObject projectile = Instantiate(bulletPrefab, shotPoint.position, Quaternion.FromToRotation(new Vector3(1, 0, 0), movement));
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.cowboy = gameObject;
            currentAmmo--;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
            StartCoroutine(Jump());

    }

    void OnDestroy()
    {
        FindObjectOfType<AudioManager>().Play("Cowboy Death");
        FindObjectOfType<GameManager>().EndGame();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        ammoDisplay.text = ("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator Jump()
    {
        // stubberino
        Debug.Log("JUMP FOR IT");
        isGrounded = false;
        // change animation to jump frames
        // disable collision for cowboy
        yield return new WaitForSeconds(jumpTime);
        // change animation to walk frames
        // enable collision for cowboy
        isGrounded = true;
    }

}

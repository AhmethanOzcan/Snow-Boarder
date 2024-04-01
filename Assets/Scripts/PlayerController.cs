using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D r2b2;
    [SerializeField] float torqueAmount = 1f; 
    [SerializeField] float loadDelay = 0.1f;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem finishParticle;
    [SerializeField] float boostSpeed = 1f;
    [SerializeField] float slowSpeed = 1f;
    [SerializeField] float baseSpeed = 1f;
    SurfaceEffector2D[] surfaceEffector2D;
    bool canMove = true;
    [SerializeField] ParticleSystem snowParticles;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        r2b2 = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindObjectsOfType<SurfaceEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            rotatePlayer();
            respondToBoost();
        }
        
    }

    

    private void respondToBoost()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            surfaceEffector2D[0].speed = boostSpeed;
            surfaceEffector2D[1].speed = boostSpeed;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            surfaceEffector2D[0].speed = slowSpeed;
            surfaceEffector2D[1].speed = slowSpeed;
        }
        else
        {
            surfaceEffector2D[0].speed = baseSpeed;
            surfaceEffector2D[1].speed = baseSpeed;
        }
    }

    void rotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            r2b2.AddTorque(torqueAmount);
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            r2b2.AddTorque(-torqueAmount);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(canMove)
        {
            if(other.tag == "Finish")
            {
                canMove = false;
                finishParticle.Play();
                other.GetComponent<AudioSource>().Play();
                Invoke("ReloadScene", loadDelay);
            }
            else
            {
                canMove = false;
                deathParticle.Play();
                GetComponent<AudioSource>().Play();
                Invoke("ReloadScene", loadDelay);
            }
        }
        
            
    }

    void OnCollisionExit2D(Collision2D other) {
        snowParticles.Stop();
    }

    void OnCollisionEnter2D(Collision2D other) {
        snowParticles.Play();
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("Level1");
    }
}


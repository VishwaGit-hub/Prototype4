using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float powerupStrength = 10.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup;
    public GameObject powerupIndicator;

   
    public  PowerUpType currentPowerUp= PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountDown;

    
  
   // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        StartCoroutine(PowerupCountdownCoroutine());
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
       
        if(currentPowerUp==PowerUpType.Rockets && Input.GetKey(KeyCode.F))
        {
            LaunchRockets();
        }

        


    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerupType;
            hasPowerup = true;
            powerupIndicator.SetActive(true);
        }
        if(powerupCountDown!=null)
        {
            StopCoroutine(powerupCountDown);
        }

        powerupCountDown = StartCoroutine(PowerupCountdownCoroutine());
    }

    IEnumerator PowerupCountdownCoroutine()
    {
            yield return new WaitForSeconds(7);
            hasPowerup = false;
        currentPowerUp = PowerUpType.None;
            powerupIndicator.SetActive(false);         


    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy")&& currentPowerUp==PowerUpType.Pushbacks)
        {
            Rigidbody enemyRigidbody=collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("collided with "+collision.gameObject.name+"with having powerup to set "+currentPowerUp.ToShortString());
        }
       

    }
    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up,
            Quaternion.identity);
            tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);
        }

    }
    
        
}

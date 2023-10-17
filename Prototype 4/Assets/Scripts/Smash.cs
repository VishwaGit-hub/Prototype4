using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Smash : MonoBehaviour
{
    public PlayerController playercontrollerscript;

    private float smashForce = 50.0f;
    private float explosionForce = 100.0f;
    private float explosionRadius = 75.0f;
    private float jumpForce = 15.0f;
    private bool isOnGround = true;
    private bool isJump = false;
    private Rigidbody playerRb;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb= GetComponent<Rigidbody>();
        playercontrollerscript= GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playercontrollerscript.currentPowerUp==PowerUpType.smash && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            isJump = true;
            StartCoroutine(SmashDown());
        }
    }

    IEnumerator SmashDown()
    {
        yield return new WaitForSeconds(0.5f);
        playerRb.AddForce(Vector3.down* smashForce, ForceMode.Impulse);
    }

    private void OnCollisonEnter(Collision collision)
        {
        if(collision.gameObject.CompareTag("Ground")&&isJump)
        {
            var enemies=FindObjectsOfType<Enemy>();
            for(int i=0;i<enemies.Length;i++)
            {
                if (enemies[i]!=null)
                {
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
                }
            }
            isOnGround = true;
            isJump = false;
        }
    }
}

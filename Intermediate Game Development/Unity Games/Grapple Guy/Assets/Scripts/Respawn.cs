using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] PlayerMovement playerMovement;
    public int deathCounter;

    public void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        deathCounter = 0;
    }

    // Player respawn
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            deathCounter++;
            player.transform.position = respawnPoint.transform.position; 
            player.transform.rotation = respawnPoint.transform.rotation;         
            Physics.SyncTransforms();
            playerMovement.StopHookshot();
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
        }
    }

    // <<Pseudocode for Respawning>>
    // - Create collider on ground for player to hit when they fall
    // CHECK player collision with ground
        // IF player to ground collision is true then....
            // ADD +1 to death counter
            // RESET palyer position and rotaion
            // CANCEL hookshot mecahnics
            // PLAY death audio
    // - Once respawned the player can try again from starting platform
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Tar tar;
    private GameObject player;
    [SerializeField] private float tarSpeedIncrease;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false; // Disable spriterender on play, allows setting up in scene view to be more intuiative        
        player = FindObjectOfType<PlayerController>().gameObject; // Finds reference to the player through finding which gameObject has the 'PlayerController' class attached. 
        tar = FindObjectOfType<Tar>();
    }

    private void Update()
    {
        if (player.transform.position.y >= transform.position.y) // If the player has passed the checkpoint (Is higher than its position)...
        {
            PassedCheckpoint();
        }
    }

    public void PassedCheckpoint()
    {
        if (tar == null)
        {
            tar = FindObjectOfType<Tar>();
        }

        tar.Speed += tarSpeedIncrease; // Add onto Tar's speed
        gameObject.SetActive(false); // Deactivate object
    }
}

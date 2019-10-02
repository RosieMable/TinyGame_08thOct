using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Tar tar;
    private GameObject player;
    [SerializeField] private float tarSpeedIncrease;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        tar = FindObjectOfType<Tar>();
        player = FindObjectOfType<PlayerController>().gameObject; // Finds reference to the player through finding which gameObject has the 'PlayerController' class attached. 
    }

    private void Update()
    {
        if (player.transform.position.y >= transform.position.y)
        {
            PassedCheckpoint();
        }
    }

    public void PassedCheckpoint()
    {
        tar.Speed += tarSpeedIncrease;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float transitionSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject; // Finds reference to the player through finding which gameObject has the 'PlayerMovement' class attached. 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = LerpToPosition(player.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);        
    }

    private Vector2 LerpToPosition(Vector2 newPosition)
    {
        return Vector2.Lerp(transform.position, newPosition, transitionSpeed * Time.deltaTime);
    }
}

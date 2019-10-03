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
        player = FindObjectOfType<PlayerController>().gameObject; // Finds reference to the player through finding which gameObject has the 'PlayerController' class attached. 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = LerpToPosition(player.transform.position); // Lerp position to make that of the player
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);  // Set Z axis to ensure nothing is behind or on top of the camera       
    }

    /// <summary>
    /// Returns lerped Vector2 based on transitionSpeed, used for updating position of gamera
    /// </summary>
    private Vector2 LerpToPosition(Vector2 newPosition)
    {
        return Vector2.Lerp(transform.position, newPosition, transitionSpeed * Time.deltaTime);
    }
}

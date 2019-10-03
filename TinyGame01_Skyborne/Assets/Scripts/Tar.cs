using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tar : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
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
        transform.position += Vector3.up * speed * Time.deltaTime; // Move upwards based on speed
        transform.position = new Vector3(ReturnLerpedValue(transform.position.x, player.transform.position.x), transform.position.y, -1); // Lerp to the players x position, to ensure that the tar is always beneath them
    }

    /// <summary>
    /// Returns the lerped value based on transitionSpeed
    /// </summary>
    private float ReturnLerpedValue(float currentPosition, float newPosition)
    {
        return Mathf.Lerp(currentPosition, newPosition, transitionSpeed * Time.deltaTime);
    }
}

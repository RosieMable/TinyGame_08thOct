using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tar : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
    private GameObject player;
    [SerializeField] private float transitionSpeed = 3;
    private bool movingToCheckpoint = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject; // Finds reference to the player through finding which gameObject has the 'PlayerMovement' class attached. 
        GameManager.instance.OnCheckpointReachedCallback += MoveOnCheckpointReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movingToCheckpoint)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            transform.position = new Vector3(LerpToXPosition(player.transform.position.x), transform.position.y, -1);
        }
    }

    private void MoveOnCheckpointReached(float desiredYPosition)
    {
        StartCoroutine(MoveToCheckpoint(desiredYPosition, 5));
    }

    private float LerpToXPosition(float newPosition)
    {
        return Mathf.Lerp(transform.position.x, newPosition, transitionSpeed * Time.deltaTime);
    }

    private float LerpToYPosition(float newPosition)
    {
        return Mathf.Lerp(transform.position.y, newPosition, transitionSpeed * Time.deltaTime);
    }

    private IEnumerator MoveToCheckpoint(float desiredYPosition, float movementLockoutTime)
    {
        movingToCheckpoint = true;
        transform.position = new Vector3(transform.position.x, LerpToYPosition(desiredYPosition), -1);
        print("Moving to" + desiredYPosition);
        yield return new WaitForSeconds(movementLockoutTime);
        movingToCheckpoint = false;
        print("Done");
    }
}

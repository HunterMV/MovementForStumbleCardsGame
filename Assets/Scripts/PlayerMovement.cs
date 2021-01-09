using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rayLength = 1f;
    public float rayOffsetX = 0.5f;
    public float rayOffsetY = 0.5f;
    public float rayOffsetZ = 0.5f;

    public GameObject Canvas; // reference to the canvas object

    Vector3 targetPosition;
    Vector3 startPosition;
    Vector3 v = Vector3.zero;

    public bool moving = false;
    public bool isPausing = false;
    public bool isRotating = false;

   public List<MovememtDirection> playMoves = new List<MovememtDirection>();

    public enum MovememtDirection // Sets up an enum for the movement types
    {
        FORWARD = 0,
        BACKWARD = 1,
        LEFT = 2,
        RIGHT = 3,
        ROTATE_LEFT = 4,
        ROTATE_RIGHT = 5
    }

    // Update is called once per frame
    void Update()
    {
        if (Canvas.GetComponent<CanvasController>().executing) // Checks to see if movement is being executed
        {
           if (playMoves.Count > 0 && !moving && !isPausing && !isRotating) // Checking that the MovememtDirection list is greater then 0 and that the Player isn't moving, pausing or rotating
            {
                // Get the first movement out of the list
                MovememtDirection moveType = playMoves[0];
                // Removes first movement from list
                playMoves.RemoveAt(0);

                // Movement code is the same for FORWARD, BACKWARD, RIGHT and LEFT. Rotation is different. 
                if (moveType == MovememtDirection.FORWARD)// Checking the MovememtDirection
                {
                    if (CanMove(transform.forward))// Checking if it can move in the MovememtDirection
                    {
                        targetPosition = transform.localPosition + transform.forward; // Setting targetPosition for movement
                        startPosition = transform.localPosition; // Setting startPosition  for movement

                        moving = true;  // Setting moving to true
                    }
                    else // If Player can't move in MovememtDirection then it calls the Stumble function 
                    {
                        Debug.Log("FORWARD Stumble");
                        Canvas.GetComponent<CanvasController>().Stumble();
                    }
                } else if (moveType == MovememtDirection.BACKWARD)
                {
                    if (CanMove(-transform.forward))
                    {
                        targetPosition = transform.position + -transform.forward;
                        startPosition = transform.position;

                        moving = true;
                    }
                    else
                    {
                        Debug.Log("BACKWARD Stumble");
                        Canvas.GetComponent<CanvasController>().Stumble();
                    }
                }else if (moveType == MovememtDirection.RIGHT)
                {
                    if (CanMove(transform.right))
                    {
                        targetPosition = transform.position + transform.right;
                        startPosition = transform.position;

                        moving = true;
                    }
                    else
                    {
                        Debug.Log("RIGHT Stumble");
                        Canvas.GetComponent<CanvasController>().Stumble();
                    }
                }
                else if (moveType == MovememtDirection.LEFT)
                {
                    if (CanMove(-transform.right))
                    {
                        targetPosition = transform.position + -transform.right;
                        startPosition = transform.position;

                        moving = true;
                    }
                    else
                    {
                        Debug.Log("LEFT Stumble");
                        Canvas.GetComponent<CanvasController>().Stumble();
                    }
                }
                else if (moveType == MovememtDirection.ROTATE_LEFT || moveType == MovememtDirection.ROTATE_RIGHT) // Checking the MovememtDirection
                {
                    isRotating = true; // Setting isRotating
                    StartCoroutine(RotateTime(moveType)); // Giving MovememtDirection to the coroutine
                }
            }
        }
       
        
        if(moving && !isPausing) // Making sure Player is moving 
        {
            if (Vector3.Distance(startPosition, transform.localPosition) >= 1f)// Checking how far Player is from targetPosition
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref v, moveSpeed); // Snapping Player to targetPosition
                isPausing = true;
                StartCoroutine(WaitTime());
            }
           transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref v, moveSpeed * Time.deltaTime); // Moving player
        }

        if (playMoves.Count <= 0) // Sets executing to false after player has moved 
        {
            Canvas.GetComponent<CanvasController>().executing = false;
        }

        // Draws rays for collision debugging 
        Debug.DrawRay(transform.position + transform.up * rayOffsetY - transform.right * rayOffsetX, -transform.right, Color.red); //Left Ray
        Debug.DrawRay(transform.position + transform.up * rayOffsetY + transform.right * rayOffsetX, transform.right, Color.blue); //Right Ray
        Debug.DrawRay(transform.position + transform.up * rayOffsetY + transform.forward * rayOffsetZ, transform.forward, Color.green); //Forward Ray
        Debug.DrawRay(transform.position + transform.up * rayOffsetY - transform.forward * rayOffsetZ, -transform.forward, Color.cyan); //Backward Ray

        bool CanMove(Vector3 direction) // Function to check if Player can move in the MovememtDirection
        {
            if (Vector3.Equals(transform.forward, direction) || Vector3.Equals(-transform.forward, direction))
            {
                if (Physics.Raycast(transform.position + transform.up * rayOffsetY + transform.forward * rayOffsetX, direction, rayLength)) return false;
                if (Physics.Raycast(transform.position + transform.up * rayOffsetY - transform.forward * rayOffsetX, direction, rayLength)) return false;
            }
            else if (Vector3.Equals(transform.right, direction) || Vector3.Equals(-transform.right, direction))
            {
                if (Physics.Raycast(transform.position + transform.up * rayOffsetY + transform.forward * rayOffsetZ, direction, rayLength)) return false;
                if (Physics.Raycast(transform.position + transform.up * rayOffsetY - transform.forward * rayOffsetZ, direction, rayLength)) return false; 
            }
            return true;
        }
    }

    IEnumerator WaitTime() // Coroutine to give time between movements
    {
        yield return new WaitForSeconds(1f);
        moving = false;
        isPausing = false;
    }
    IEnumerator RotateTime(MovememtDirection rotate) // Coroutine that rotates the Player and waits  
    {
        yield return new WaitForSeconds(1f);
        if(rotate == MovememtDirection.ROTATE_RIGHT)
        {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            isRotating = false;
        } else if (rotate == MovememtDirection.ROTATE_LEFT)
        {
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
            isRotating = false;
        }
        isRotating = false;
    }

    public void SetMovement (List<MovememtDirection> list) // Gets called by the CanvasController and sets playMoves to the list given by CanvasController
    {
        foreach(MovememtDirection move in list)
        {
            playMoves.Add(move);
        }
    }

}

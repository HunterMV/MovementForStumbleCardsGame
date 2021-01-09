using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerMovement;

public class CanvasController : MonoBehaviour
{
    List<MovememtDirection> playerMovement = new List<MovememtDirection>();

    public GameObject playerMovementOrderText; // Reference to text that displays MovememtDirection
    public GameObject StumbleText; // Reference to text that displays if Player stumbles
    public GameObject player; // Reference to Player object

    public Button executeButton; // Reference the execute button

    public bool executing = false;
    public bool autoExecute = false;
    public bool stumbling = false;

    int lastCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (!executing) // checking if movement is executing 
        {
            string resultOrder = "";
            foreach (MovememtDirection move in playerMovement) // Going through each move in MovememtDirection, adding it to resultOrder then displaying it on playerMovementOrderText 
            {
                resultOrder += move.ToString() + "\n";
                playerMovementOrderText.GetComponent<Text>().text = resultOrder;
            }
        }

        if(autoExecute && playerMovement.Count > lastCount) // Checking if autoExecute is turned on and that it only executes the movement onces per MovememtDirection then calls ExecuteMovement
        {
            ExecuteMovement(); 
            lastCount += 1;
        }
      
    }

    public void MovementType(int moveNumber) // Called by the MovememtDirection buttons to add their MovememtDirection type to the playerMovement list, but only if not executing
    {
        if (!executing)
        {
            MovememtDirection moveType = (MovememtDirection)moveNumber;
            playerMovement.Add(moveType);
        }
    }

    public void ClearMovement() // Clears MovememtDirection list from canvas, playerMovementOrderText and Player. This function is called by the clear button
    {
        lastCount = 0;
        playerMovement.Clear();
        playerMovementOrderText.GetComponent<Text>().text = "Movements Cleared";
        player.GetComponent<PlayerMovement>().playMoves.Clear();
    }

    public void ExecuteMovement() // This function is called by the execute button 
    {
        if (!executing) // Making sure Player isn't already moving
        {
            player.GetComponent<PlayerMovement>().SetMovement(playerMovement); // Calling SetMovement from PlayerMovement and passes it the playerMovement MovememtDirection list
            executing = true; // setting executing to true
        } else
        {
            Debug.Log("Player still moving");
        }
    }

    public void Stumble() // Shows Stumble text for time set in StumbleWaitTime, is called from playerMovement when player can't move
    {
        if (!stumbling)
        {
            stumbling = true;
            StumbleText.SetActive(true);
            StartCoroutine(StumbleWaitTime());
            executing = false;
            ClearMovement();
        } else
        {
            Debug.Log("Player already stumbling");
        }
    }

    IEnumerator StumbleWaitTime() // Time Stumble text is displayed
    {
        yield return new WaitForSeconds(4f);
        StumbleText.SetActive(false);
        stumbling = false;
    }

    public void AutoMove(bool execute) // Called by the AutoMove toggle button. Sets autoExecute to true or false, and turns on or off the executeButton
    {
        autoExecute = execute;

        if (execute)
        {
            executeButton.interactable = false;
        } else
        {
            executeButton.interactable = true;
        }
    }
}



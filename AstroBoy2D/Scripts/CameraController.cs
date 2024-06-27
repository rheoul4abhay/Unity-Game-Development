using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float offset;//To set a offset value which will be added or subtracted on cameras x axis to shift camera focus
    public float offsetSmoothing;//To determine the speed at which the camera follows up with the player's direction change 
    private Vector3 playerPosition;//To keep track of players x,y,z axis(for main camera we need 3 axis,theres no camera2D)


    void Start()
    {
          
    }


    void Update()
    {
        //To make camera follow the player along the x axis below code is used however we want to add smoothness and offset
        //so we will comment down the piece of code below.This is one way to make the camera follow the player.

        /*transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);*/

        playerPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        if(player.transform.localScale.x > 0f) //Means  player is facing right so show the view ahead of player
        {
            playerPosition = new Vector3(playerPosition.x + offset,playerPosition.y,playerPosition.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position,playerPosition,offsetSmoothing*Time.deltaTime);
        //Lerp means to linearly polate or transit between two points 
    }
}

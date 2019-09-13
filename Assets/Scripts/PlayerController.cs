﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 50.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = this.transform.position;                              //modify the player's current position
        pos.x += moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;  //by the value/directions of the input manager's key presses
        pos.z += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;    //(horizontal and vertical are tags for the arrow and WASD keys)
        this.transform.position = pos;                                      //(deltaTime is to make a frame rate independent scale for the speed per update loop)
    }
}

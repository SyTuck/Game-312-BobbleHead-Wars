﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWall : MonoBehaviour
{

    private Animator arenaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        GameObject arena = this.transform.parent.gameObject;
        arenaAnimator = arena.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // this.gameObject.
        arenaAnimator.SetBool("IsLowered", true);
    }

    void OnTriggerExit (Collider other)
    {
        arenaAnimator.SetBool("IsLowered", false);    
    }
}

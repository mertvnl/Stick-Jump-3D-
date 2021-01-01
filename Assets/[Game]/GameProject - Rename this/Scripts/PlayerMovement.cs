﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public float speed = 250f;
    public bool canMove = true;
    private Rigidbody rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
            }

            return rigidbody;
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameStarted && canMove)
        {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Rigidbody.velocity.y / 10 , 1);
            Rigidbody.velocity = dir * (speed * Time.fixedDeltaTime);
        }
    }

    public void Jump()
    {
        canMove = false;
        Rigidbody.velocity = new Vector3(0,1,1) * TheStick.Instance.transform.localScale.y * 5;

        TheStick.Instance.isJumping = false;
    }
}

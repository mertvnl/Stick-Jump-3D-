﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StickController : MonoBehaviour
{
    public float stickSizeToGain = 0.1f;
    public GameObject currentObstacle = null;
    public Transform StickTop;
    public GameObject stickPrefab;
    public Transform rightHand;

    public float speed = 250f;
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
        if (GameManager.Instance.isGameStarted)
        {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Rigidbody.velocity.y / 10 , 1);
            Rigidbody.velocity = dir * (speed * Time.fixedDeltaTime);
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.gameData.playerStick = this;
    }


    void Update()
    {
        //Follow hand
        Vector3 newPos = new Vector3(rightHand.position.x, transform.position.y, rightHand.position.z);
        transform.position = newPos;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StickUp();
        }
    }

    public void StickUp()
    {
        Vector3 newStickSize = new Vector3(0, stickSizeToGain, 0);
        transform.position += newStickSize;
        transform.localScale += newStickSize;
    }
    public void StickCut(GameObject obstacle)
    {
        float offset = StickTop.position.y - obstacle.gameObject.transform.position.y;
        GameManager.Instance.gameData.fallingStickSizeY = offset;
        Vector3 newStickSize = new Vector3(0, offset / 2, 0);
        transform.position -= newStickSize;
        transform.localScale -= newStickSize;
        
    }
    public void CreateFallingStick(float yPosToCreate)
    {
        Vector3 newPos = new Vector3(transform.position.x, yPosToCreate, transform.position.z);
        Instantiate(stickPrefab, newPos, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        ObstacleController obstacle = other.gameObject.GetComponent<ObstacleController>();
        if (obstacle != null)
        {
            StickCut(other.gameObject);
            CreateFallingStick(other.gameObject.transform.position.y + GameManager.Instance.gameData.fallingStickSizeY / 2);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D enemyRigidBody2D;
    BoxCollider2D enemyPeriscope;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyPeriscope = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            enemyRigidBody2D.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            enemyRigidBody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidBody2D.velocity.x)), 1f);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }



}

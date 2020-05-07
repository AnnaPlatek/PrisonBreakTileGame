using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 4f;
    [SerializeField] float acceleration = 1.5f;

    //State
    bool isAlive = true;

    //Cached Component
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        JumpValidation();
        FlipSprite();
        Climb();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 and 1
        //Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);

        if (Input.GetKey("left shift"))
        {
            myRigidBody.velocity = new Vector2(controlThrow * runSpeed * acceleration, myRigidBody.velocity.y);
        }

        else
        {
            myRigidBody.velocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        }
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void JumpValidation()
    {
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
            {
                Jump();
            }
            return;
        }
        Jump();
    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Climb()
    {
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // value is between -1 and 1
            if (controlThrow != 0)
            {
                Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
                myRigidBody.velocity = playerVelocity;

                bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
                myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
            }
            
        }
        else
        {
            myAnimator.SetBool("Climbing", false);
        }
    }


}

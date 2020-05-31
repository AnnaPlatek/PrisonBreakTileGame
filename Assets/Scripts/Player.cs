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
    [SerializeField] Vector2 deathKick = new Vector2(5f, 20f);

    //State
    bool isAlive = true;
    bool climbingState = false;

    //Cached Component
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myCollider2D;
    BoxCollider2D myFeetCollider2D;
    Transform myTransform;
    float gravityScaleAtStart;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        climbingState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        JumpValidation();
        FlipSprite();
        Climb();
        ShowPosition();
        Die();
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
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
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
            ReleaseLadder();
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
        //todo: stop climbing when you are at the top of ladder
        //todo: stop moving left and ringt on ladder
        //todo: automatically release the ladder when you touch the ground
        
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // value is between -1 and 1
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")) && controlThrow != 0)
        {
            climbingState = true;  
        }
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            ReleaseLadder();
        }

        if (climbingState == true)
        {
            if (controlThrow != 0)
            {
                myRigidBody.gravityScale = 0f;
                Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
                myRigidBody.velocity = playerVelocity;

                bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
                myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
            }
            else if (controlThrow == 0)
            {
                //myTransform.transform.position = new Vector2();
                //climbSpeed = 0f;
                myRigidBody.gravityScale = 0f;
                Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, 0f);
                myRigidBody.velocity = playerVelocity;

                return;
            }

        }
        
    }

    private void ReleaseLadder()
    {
        climbingState = false;
        myAnimator.SetBool("Climbing", false);
        myRigidBody.gravityScale = gravityScaleAtStart;
    }

    private void ShowPosition()
    {
        Debug.Log(myTransform.transform.position); 
    }

    private void Die()
    {
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }


}

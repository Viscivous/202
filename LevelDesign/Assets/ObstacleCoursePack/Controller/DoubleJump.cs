using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
   // public Animator anim;
    public Transform pivot;
    public float rotateSpeed;
   // public GameObject playerModel;
    public float knockBackForce;
    public float knockBackTime;
    public float knockBackCounter;
    public int maxJumps = 1; // Maximum number of jumps allowed
   // public Animator animator;
    private int jumpCount = 0; // Current jump count

    private Vector3 moveDirection;
    public float gravityScale;

    private Vector3 currentRotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter <= 0)
        {
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            if (controller.isGrounded)
            {
                moveDirection.y = 0f;
                jumpCount = 0; // Reset jump count when grounded

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    jumpCount++; // Increment jump count when jumping
                }
            }
            else
            {
                // Check if the player can perform another jump
                if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
                {
                    moveDirection.y = jumpForce;
                    jumpCount++; // Increment jump count when jumping
                    
                   // if(jumpCount == 2)
                   // {
                       // animator.SetTrigger("SecondJump");
                   // }
                }
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        // Move player in different directions based on camera look direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
          //  playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

       // anim.SetBool("isGrounded", controller.isGrounded);
       // anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }

    public void Knockback(Vector3 direction)
    {
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Camera cam;
    public float speed = 6f;
    
    
    private float gravitySpeed = 9.81f;
    private Animator animator;
    private Transform playerTransform;
    private bool isMovementEnalbed = true;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        playerTransform = GetComponent<Transform>();
    }
    
    // Update iCharacterMovement ms called once per frame
    void Update() {
        if (!characterController.isGrounded)
        {
            UseGravity(gravitySpeed);
        }

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var inputDirection  = new Vector3(horizontal, 0, vertical).normalized;
        

        if (inputDirection.magnitude > 0.1f && isMovementEnalbed) {
            var camDirection = cam.transform.right * inputDirection.x + cam.transform.forward * inputDirection.z;
            characterController.Move(camDirection * speed * Time.deltaTime);
            animator.SetFloat("BlendMove", 0.5f);
            transform.forward = new Vector3(camDirection.x, 0, camDirection.z);
        } else {
            animator.SetFloat("BlendMove", 0f);
        }
    }

    public void DisableMovement()
    {
        isMovementEnalbed = false;
    }

    public void EnableMovement()
    {
        isMovementEnalbed = true;
    }

    private void UseGravity(float gravitySpeed)
    {
        var force = Vector3.down * gravitySpeed * Time.deltaTime;
        characterController.Move(force);
    }
}

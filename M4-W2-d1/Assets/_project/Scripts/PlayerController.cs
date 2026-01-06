using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedRun;
    [SerializeField] private float speedRotation;

    [SerializeField] private float jumpForce;
    [SerializeField]private int numJump;


    private Vector2 direction;
    private Vector3 move;
    private Vector3 velocity;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool isGroundend;
    [SerializeField] private Vector3 drag;

    [SerializeField] private Transform checkerGround;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask ground;



    CharacterController movement;

    private void Awake()
    {
        movement = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGroundend = Physics.CheckSphere(checkerGround.position,radius,ground, QueryTriggerInteraction.Ignore);
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float vecLength = direction.magnitude;
        if (vecLength > 1)
        {
            direction /= vecLength;
        }
        move = new Vector3(direction.x, 0, direction.y);
        if (isGroundend)
        {
            velocity.y = 0;
            numJump = 0;
        }
        if (Input.GetButtonDown("Jump") && (isGroundend || numJump<2))
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2 * gravity);
            numJump++;
        }
        velocity.y += gravity * Time.deltaTime;

        velocity.x /= 1 + drag.x * Time.deltaTime;
        velocity.y /= 1 + drag.y * Time.deltaTime;
        velocity.z /= 1 + drag.z * Time.deltaTime;

        movement.Move(velocity * Time.deltaTime);

    }
    private void FixedUpdate()
    {
        if (move != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, move, speedRotation * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement.Move(move * speedRun * Time.deltaTime);
        }
        else
        {
            movement.Move(move * speed * Time.deltaTime);

        }

    }
}

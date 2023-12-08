using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net;

public class PlayerMovemant : MonoBehaviour
{

    



    public Camera mainCamera;

    public PlayerCOntroller ggControll;


    [Header("Sounds")]
    public AudioSource playerSourse;
    public AudioClip takeDamageSound;
    public AudioClip blockedDamage;

    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftControl;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;



    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }


    private void Start()
    {


        ggControll = GetComponentInChildren<PlayerCOntroller>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;


    }

    private void Update()
    {

        bool isWPressed = Input.GetKey(KeyCode.W);
        bool isAPressed = Input.GetKey(KeyCode.A);
        bool isSPressed = Input.GetKey(KeyCode.S);
        bool isDPressed = Input.GetKey(KeyCode.D);
        bool isDashKeyPressed = Input.GetKey(KeyCode.LeftShift);




        //handsAnim.isBlocked = false;
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        
        if (isDashKeyPressed && ggControll.canDash && ggControll.currentEnergi > ggControll.needEnergiForDash)
        {
            if (isDashKeyPressed)
            {
                // Якщо натиснуто лівий Ctrl, викликаємо функцію Dash з обраного напрямку
                Vector3 dashDirection = Vector3.zero;

                if (isWPressed)
                {
                    dashDirection += mainCamera.transform.forward;
                }
                if (isAPressed)
                {
                    dashDirection -= mainCamera.transform.right;
                }
                if (isSPressed)
                {
                    dashDirection -= mainCamera.transform.forward;
                }
                if (isDPressed)
                {
                    dashDirection += mainCamera.transform.right;
                }

                // Якщо обрано хоча б один напрямок, викликаємо функцію Dash
                if (dashDirection != Vector3.zero)
                {
                    StartCoroutine(DashWithCooldown(dashDirection));
                }
            }
        }

        Vector3 moveDirectionFlat = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
    }






    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            if (ggControll.currentEnergi > 0)
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
            }
            else
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
            
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }


    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon"))
        {
            if (ggControll.canTakeDamage)
            {
                playerSourse.PlayOneShot(takeDamageSound);
                ggControll.currentHP -= 1;
            }
            else
            {
                playerSourse.PlayOneShot(blockedDamage);

            }

        }
    }

    IEnumerator DashWithCooldown(Vector3 dashDirection)
    {
        ggControll.canDash = false; // Забороняємо нові виклики "дешу" до закінчення затримки

        // Викликаємо функцію Dash
        Dash(dashDirection);

        // Чекаємо затримку перед тим, як дозволити новий виклик "дешу"
        yield return new WaitForSeconds(ggControll.dashCooldown);

        ggControll.canDash = true; // Дозволяємо нові виклики "дешу"
    }



    public void Dash(Vector3 dashDirection)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dashDirection, out hit, ggControll.maxDash))
        {
            Vector3 tpPoint = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
            StartCoroutine(SmoothDashMovement(tpPoint));
            ggControll.currentEnergi -= ggControll.needEnergiForDash;
        }
        else
        {
            Vector3 endTeleportPoint = transform.position + dashDirection * ggControll.maxDash;
            endTeleportPoint.y = Mathf.Max(transform.position.y, endTeleportPoint.y + 1);

            StartCoroutine(SmoothDashMovement(endTeleportPoint));
            ggControll.currentEnergi -= ggControll.needEnergiForDash;
        }
    }

    IEnumerator SmoothDashMovement(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * ggControll.smoothDashSpeed;
            yield return null;
        }

        // Опціонально:
        // Виконуємо додаткові дії після закінчення "деша".
        yield return null;
    }
}
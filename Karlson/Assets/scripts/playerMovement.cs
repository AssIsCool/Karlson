
using UnityEngine;
using System;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float wallRunForce;
    public LayerMask wallLayer;
    

    public Transform playerCam;
    public Transform orientation;

    private Rigidbody rb;
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1.5f;

    private float x, y;
    private bool jumping;

    private bool grounded;
    private bool crouching;
    private bool surfing;
    private bool onWall;
    

    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    private Vector3 normalVector = Vector3.up;

    public bool isWallRunning;
    private Vector3 wallNormalVector;

    private float actualWallRotation;
    private float wallRotationVel;
    private float wallRunRotation;
    public float wallRunRotateAmount = 10f;
    public bool useWallrunning = true;
    public float wallRunGravity = 1;
    public float initialForce = 20f;
    public float escapeForce = 600f;
    public string wallTag = "Wall";

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Set up physics material
        PhysicMaterial mat = new PhysicMaterial("tempMat");
        mat.bounceCombine = PhysicMaterialCombine.Average;
        mat.bounciness = 0;
        mat.frictionCombine = PhysicMaterialCombine.Minimum;
        mat.staticFriction = 0;
        mat.dynamicFriction = 0;
        gameObject.GetComponent<Collider>().material = mat;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        readyToJump = true;
        wallNormalVector = Vector3.up;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        MyInput();
        Look();
        WallRunning();
        WallRunRotate();
    }

    private void WallRunning()
    {
        if (onWall == true)
            if (isWallRunning)
            {
                rb.AddForce(-wallNormalVector * Time.deltaTime * speed);
                rb.AddForce(Vector3.up * Time.deltaTime * rb.mass * wallRunGravity * -Physics.gravity.y * 0.4f);
                Debug.Log("is wallruning");
            }
    }

    private void FindWallRunRotation()
    {
        if (!isWallRunning)
        {
            wallRunRotation = 0f;
            return;
        }

        float current = playerCam.transform.rotation.eulerAngles.y;

        float num2 = Mathf.DeltaAngle(current, Vector3.SignedAngle(Vector3.forward, wallNormalVector, Vector3.up));
        wallRunRotation = (-num2 / 90f) * wallRunRotateAmount;
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void WallRunRotate()
    {
        FindWallRunRotation();
        float num = 12f;
        actualWallRotation = Mathf.SmoothDamp(actualWallRotation, wallRunRotation, ref wallRotationVel, num * Time.deltaTime);
        playerCam.localRotation = Quaternion.Euler(playerCam.rotation.eulerAngles.x, playerCam.rotation.eulerAngles.y, actualWallRotation);
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButtonDown("space");

        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch()
    {
        transform.localScale = new Vector3(1, 0.5f, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.2f && grounded)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * wallRunForce);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        if (readyToJump && jumping) Jump();

        float maxSpeed = 20;

        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        // Jump logic - Add this part
        if (onGround && jumping)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        float multiplier = 1f, multiplierV = 1f;

        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (grounded && crouching) multiplierV = 0f;

        rb.AddForce(orientation.transform.forward * y * speed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * speed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if ((grounded || isWallRunning || surfing) && readyToJump)
        {
            Vector3 velocity = rb.velocity;
            readyToJump = false;
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
            }
            else if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector3(velocity.x, velocity.y / 2f, velocity.z);
            }

            if (isWallRunning)
            {
                rb.AddForce(wallNormalVector * jumpForce * 3f);
            }

            Invoke("ResetJump", jumpCooldown);

            if (isWallRunning)
            {
                isWallRunning = false;
            }
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
        onGround = true;
    }
    string floorTag = "Floor";
    private float desiredX;
    private bool onGround;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(floorTag))
        {
            onGround = true;
            ResetJump();
        }
        else if (collision.gameObject.CompareTag(wallTag))
        {
            GameObject wall = collision.gameObject;

            isWallRunning = true;
            Debug.Log("is wallruning");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag(floorTag))
        {
            onGround = true;
        }
        if(collision.gameObject.CompareTag(wallTag))
        {
            isWallRunning = false;
        }
    }
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        float clamp = 89.5f;
        xRotation = Mathf.Clamp(xRotation, -clamp, clamp);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        if (crouching)
        {
            rb.AddForce(speed * Time.deltaTime * -rb.velocity.normalized * wallRunForce);
            return;
        }
    }
}   
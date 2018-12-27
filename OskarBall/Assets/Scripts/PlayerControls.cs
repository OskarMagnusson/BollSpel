using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class PlayerControls : MonoBehaviour
{
    #region SerializedFields

    [SerializeField]
    [Tooltip("Horizontal movement acceleration.")]
    float horizontalForceMultiplier;
    
    [SerializeField]
    [Tooltip("Maximum horizontal movement speed.")]
    float maxHorizontalVelocity;

    [SerializeField]
    [Tooltip("Amount of force added when dash is executed.")]
    float dashSpeed;

    [SerializeField]
    [Tooltip("Controls how much the player bounces on the environment.")]
    float bounciness;

    [SerializeField]
    [Tooltip("Amount of force added when the Jump button is held down.")]
    float jumpForceMultiplier;

    [SerializeField]
    [Tooltip("How long the Jump button may be held to increase jump height.")]
    float jumpTime;

    [SerializeField]
    [Tooltip("Amount of force added when the ump button is first pressed.")]
    float initialJumpForce;

    [SerializeField]
    [Tooltip("Horizontal movement speed while airborne (0-1).")]
    float airborneHorizontalMultiplier;

    [SerializeField]
    PhysicMaterial bounceMat, noBounceMat, environmentMat;

    #endregion

    #region PrivateFields

    float verticalForce = 0f, horizontalForce, radius;

    bool jumping;

    delegate void Movement();

    Movement currentMovement;

    Rigidbody rb;

    Collider coll;

    #endregion

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<CameraBehavior>().PC = this;
        rb = GetComponent<Rigidbody>();
        radius = GetComponent<SphereCollider>().radius;
        coll = GetComponent<Collider>();
        currentMovement = DefaultMovement;
    }

    void Update()               //Determines which Movement to run depending on player input and whether the ball is still moving
    {
        //if paused: return
        coll.material = Input.GetAxisRaw("Vertical") < 0f ? noBounceMat : bounceMat;
        environmentMat.bounciness = Input.GetAxisRaw("Vertical") < 0f ? 0.0f : bounciness;
        currentMovement();
    }

    void DefaultMovement()
    {
        if (Input.GetButtonDown("Dash") && !Grounded() && (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f))
        {
            Dash();
            return;
        }
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            StartCoroutine("Jump");
        }
        else if (Input.GetButtonUp("Jump"))
        {
            StopCoroutine("Jump");
            jumping = false;
        }
        if (jumping)
        {
            rb.AddForce(Vector3.up * jumpForceMultiplier);
        }
        float horizontalMultiplier = Grounded() ? 1f : airborneHorizontalMultiplier;
        horizontalForce = Input.GetAxisRaw("Horizontal");
        if (!Grounded() || Input.GetAxisRaw("Vertical") >= 0f)
        {
            rb.AddForce(new Vector3(horizontalForce * horizontalForceMultiplier * horizontalMultiplier, 0f, 0f));
        }
        else if (Grounded() && Input.GetAxisRaw("Vertical") < 0f)
        {
            float hSpeed = Input.GetAxisRaw("Horizontal") == 0f ? rb.velocity.x : horizontalForce * horizontalForceMultiplier * horizontalMultiplier;
            rb.velocity = new Vector3(hSpeed, 0f, 0f);
        }
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxHorizontalVelocity, maxHorizontalVelocity), rb.velocity.y, 0f);
    }

    void DashMovement()
    {
        if (Grounded() && Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            currentMovement = DefaultMovement;
        }
    }

    bool Grounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, radius / 2, Vector3.down, out hit, radius / 2);
    }

    IEnumerator Jump()
    {
        float jumpStart = Time.time;
        rb.AddForce(Vector3.up * initialJumpForce);
        jumping = true;
        yield return new WaitUntil(() => Time.time >= jumpStart + jumpTime);
        jumping = false;
    }

    public IEnumerator Launch(Vector3 direction, float force)
    {
        currentMovement = LaunchMovement;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * force);
        yield return new WaitForSeconds(0.5f);
        currentMovement = DashMovement;
    }

    void LaunchMovement()
    {
        return;
    }

    void Dash()
    {
        currentMovement = DashMovement;
        rb.velocity = Vector3.zero;
        rb.AddForce(Input.GetAxisRaw("Horizontal") * dashSpeed, Input.GetAxisRaw("Vertical") * dashSpeed, 0f);
    }

    public void Kill()
    {
        StartCoroutine(GameManager.RespawnWait());
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class PlayerControls : MonoBehaviour
{
    #region SerializedFields
    [SerializeField]
    float verticalForceMultiplier, horizontalForceMultiplier, jumpCheckDistance, maxHorizontalVelocity, dashSpeed, bounciness, jumpForceMultiplier, jumpTime, initialJumpForce;

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
        rb = GetComponent<Rigidbody>();
        radius = GetComponent<SphereCollider>().radius;
        coll = GetComponent<Collider>();
        currentMovement = DefaultMovement;
    }

    void Update()               //Determines which Movement to run depending on player input and whether the ball is still moving
    {
        //if paused: return
        currentMovement();
    }

    void DefaultMovement()
    {
        //verticalForce = 0f;
        //rb.inertiaTensorRotation = new Quaternion(0.1f, 0.1f, 0.1f, 1f);
        coll.material = Input.GetAxisRaw("Vertical") < 0f ? noBounceMat : bounceMat;
        environmentMat.bounciness = Input.GetAxisRaw("Vertical") < 0f ? 0.0f : bounciness;
        if (Input.GetButtonDown("Dash") && !Grounded() && (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f))
        {
            Dash();
            return;
        }
        /*
        if (Input.GetButton("Jump"))
        {
            verticalForce += 5f;
            if (verticalForce > maxJumpForce)
                verticalForce = maxJumpForce;
        }
        else if (Input.GetButtonUp("Jump") && Grounded())
        {
            Jump();
        }
        */
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            StartCoroutine("Jump");
        }
        else if (Input.GetButtonUp("Jump"))
        {
            StopCoroutine("Jump");
            jumping = false;
        }
        //horizontalForce = Grounded() ? Input.GetAxisRaw("Horizontal") : 0f;
        if (jumping)
        {
            rb.AddForce(Vector3.up * jumpForceMultiplier);
        }
        //rb.AddTorque(-rb.angularVelocity * brakeForce);
        horizontalForce = Input.GetAxisRaw("Horizontal");
        rb.AddForce(new Vector3(horizontalForce * horizontalForceMultiplier, 0f, 0f));
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxHorizontalVelocity, maxHorizontalVelocity), rb.velocity.y, 0f);
    }

    void DashMovement()
    {
        if (Mathf.Abs(rb.velocity.x) < 2f && Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            print("Dash done");
            currentMovement = DefaultMovement;
        }
    }

    bool Grounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, radius / 2, Vector3.down, out hit, radius + jumpCheckDistance);
    }

    IEnumerator Jump()
    {
        float jumpStart = Time.time;
        rb.AddForce(Vector3.up * initialJumpForce);
        jumping = true;
        yield return new WaitUntil(() => Time.time >= jumpStart + jumpTime);
        jumping = false;
    }

    /*
    void Jump()
    {
        //verticalForce = verticalForceMultiplier;
        rb.AddForce(new Vector3(0f, verticalForce * verticalForceMultiplier, 0f));
        verticalForce = 0f;
    }
    */



    void Dash()
    {
        currentMovement = DashMovement;
        rb.AddForce(Input.GetAxisRaw("Horizontal") * dashSpeed, Input.GetAxisRaw("Vertical") * dashSpeed, 0f);
    }
}

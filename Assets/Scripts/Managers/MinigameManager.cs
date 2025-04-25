using System;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    // on ship propulsion, rotate body ref on local y

    [Header("Ship stuff:")]
    public Rigidbody2D shipRb;
    public Transform shipBody;
    public Rigidbody2D shipTargetRotRb;
    public Transform shipSpinBody;
    private Vector2 movementInput;
    public float shipSpeed;
    public Vector2 shipWorldSize;
    public float shipSpinSpeed;
    public float shipBodySmoothSpeed;

    public float stunTimer;
    public float stunDefaultTime;
    private void FixedUpdate()
    {
        stunTimer = Mathf.Max(stunTimer - Time.fixedDeltaTime, 0f);
        movementInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

        if (stunTimer == 0f)
        {
            shipRb.AddForce(movementInput * shipSpeed * Time.fixedDeltaTime);
        }

        Vector2 lookDir = -shipRb.linearVelocity.normalized;
        float angle = MathF.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f + 180f;
        shipTargetRotRb.rotation = angle;
    }
    private void Update()
    {
        if (shipRb.linearVelocity.magnitude != 0)
        {
            shipBody.rotation = Quaternion.Lerp(shipBody.rotation, shipTargetRotRb.transform.rotation, shipBodySmoothSpeed * Time.deltaTime);
            shipSpinBody.Rotate(shipRb.linearVelocity.magnitude * shipSpinSpeed * shipSpeed * Time.deltaTime * Vector3.up);
        }
    }
    public void hit(float stunMult = 1f)
    {
        stunTimer = stunDefaultTime * stunMult; // more expensive asteroids will give a minor mult (weight), max is 1.3f
    }
}

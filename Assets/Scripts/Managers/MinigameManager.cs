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
    [Header("5 Ship Speed levels:\n1 - 300;\n2 - 600;\n3 - 900;\n4 - 1200;\n5 - 1500.")]
    public float shipSpeed;
    public Vector2 shipWorldSize;
    public float shipSpinSpeed;
    public float shipBodySmoothSpeed;

    public float stunTimer;
    public float stunDefaultTime;

    public ShipCollision shipCollision;

    public float miningRigRewardMult = 1f;

    public float shipSpinMinimumSpeed;

    [Space]
    [Header("Asteroids:")]
    public float asteroidSpawnCooldown;

    // Asteroid types:
    // - Regular
    // - Silver
    // - Gold
    // - Green Gems
    // - 

    private void FixedUpdate()
    {
        bool oneTimeForBump = stunTimer != 0;
        stunTimer = Mathf.Max(stunTimer - Time.fixedDeltaTime, 0f);
        movementInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

        if (stunTimer == 0)
        {
            shipRb.AddForce(movementInput * shipSpeed * Time.fixedDeltaTime);
            if (oneTimeForBump)
            {
                shipCollision.switchLights(true);
            }
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
            //shipSpinBody.Rotate(Mathf.Max(shipRb.linearVelocity.magnitude * shipSpinSpeed * shipSpeed, shipSpinMinimumSpeed) * Time.deltaTime * Vector3.up);
        }
        shipSpinBody.Rotate(Mathf.Max(shipRb.linearVelocity.magnitude * shipSpinSpeed * shipSpeed, shipSpinMinimumSpeed) * Time.deltaTime * Vector3.up);
    }
    public void hit(float stunMult = 1f)
    {
        if (stunTimer == 0)
        {
            stunTimer = stunDefaultTime * stunMult; // more expensive asteroids will give a minor mult (weight), max is 1.3f
        }
    }
}

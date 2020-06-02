using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Possessable : MonoBehaviour, IPossessable
{
    public Rigidbody RB;
    public float MaxSpeed = 10;
    public float TorqueMultiplier = 10;
    public float MaxAngularVelocity = 10;
    public float HorizontalMultiplier, JumpMultiplier;
    public ForceMode forceMode;
    void Start()
    {
        RB.maxAngularVelocity = MaxAngularVelocity; 
        RB = GetComponent<Rigidbody>();
    }
    public void MoveByForceHorizontal()
    {
        Vector3 force = new Vector3(PlayerController.Instance.HorizontalInput * HorizontalMultiplier, 0);

        float multiplier = 1;

        if (PlayerController.Instance.HorizontalInput > 0)
        {
            multiplier = 1 - (Mathf.Abs(Mathf.Clamp(RB.velocity.x, 0, MaxSpeed)) / MaxSpeed);
        }
        if (PlayerController.Instance.HorizontalInput < 0)
        {
            multiplier = 1 - (Mathf.Abs(Mathf.Clamp(RB.velocity.x, -MaxSpeed, 0)) / MaxSpeed);
        }

        RB.AddForce(force * multiplier, ForceMode.Force);
        //RB.MovePosition( new Vector2( transform.position.x + force.x, transform.position.y + force.y));
        Debug.Log("Force: " + force * multiplier);
    }
    public void ApplyTorque()
    {
        Vector3 torque = PlayerController.Instance.TorqueInput * TorqueMultiplier;

      
        //Vector3 scale = Vector3.Scale(Camera.main.transform.forward, torque);
        //Debug.DrawRay(transform.position , scale);
        RB.AddTorque((-Camera.main.transform.forward * torque.x) * Time.deltaTime, forceMode);
        RB.AddTorque((Camera.main.transform.right * torque.y) * Time.deltaTime, forceMode);
        //Debug.Log(RB.angularVelocity);
        //Debug.DrawRay(transform.position, Camera.main.transform.forward * 1000);
        //Debug.DrawRay(transform.position, Camera.main.transform.right * 1000);
    }
    
}

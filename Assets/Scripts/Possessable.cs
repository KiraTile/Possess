using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]

[RequireComponent(typeof(Collider))]
public class Possessable : Interactable

{
    
    [Header("General"), Tooltip("How game object should be moved?"), SerializeField]
    MoveType moveType = MoveType.Both;

    [Header("Movement by torque"), Tooltip("Force mode of torque-type movement"), SerializeField]
    ForceMode forceModeTorque;


    [SerializeField]
    float TorqueMultiplier = 10;


    [SerializeField]
    float MaxAngularVelocity = 10;

    [Header("Movement by force"), Tooltip("Force mode of force-type movement"), SerializeField]
    ForceMode forceModeForce;


    [SerializeField]
    float MaxSpeed = 10;


    [SerializeField]
    float ForcePower = 1;


    [Header("Jumping"), Tooltip("Force mode of the jump"), SerializeField]
    ForceMode forceModeJump = ForceMode.VelocityChange;


    [SerializeField]
    float JumpForce = 1;

    bool onGround;

    Rigidbody RB;
    Collider _collider;

    override protected void Awake()
    {
        RB = GetComponent<Rigidbody>();
        RB.maxAngularVelocity = MaxAngularVelocity;
        _collider = GetComponent<Collider>();
        base.Awake();
    }

    public void Move()
    {
        switch (moveType)
        {
            case MoveType.Both:
                {
                    MoveByForceHorizontal();
                    ApplyTorque();
                    break;
                }
            case MoveType.Force:
                {
                    MoveByForceHorizontal();
                    break;
                }
            case MoveType.Torque:
                {
                    ApplyTorque();
                    break;
                }

        }
           
      
        
    }

     void MoveByForceHorizontal()
    {
        Vector3 force = PlayerController.Instance.TorqueInput * ForcePower;

        RB.AddForce((PlayerFollower.Instance.transform.right * force.x) * Time.deltaTime, forceModeTorque);
        RB.AddForce((PlayerFollower.Instance.transform.forward * force.y) * Time.deltaTime, forceModeTorque);

    }
     void ApplyTorque()
    {
        Vector3 torque = PlayerController.Instance.TorqueInput * TorqueMultiplier;

        RB.AddTorque((-PlayerFollower.Instance.transform.forward * torque.x) * Time.deltaTime, forceModeTorque);
        RB.AddTorque((PlayerFollower.Instance.transform.right * torque.y) * Time.deltaTime, forceModeTorque);
       
    }



     public void Jump()
    {
        
        if (onGround)
        {
            RB.AddForce(Vector3.up * JumpForce, forceModeJump);
        }
    }
   

    enum MoveType
    {
        [Tooltip("Movement by torque")]
        Torque,
        [Tooltip("Movement by force")]
        Force,
        [Tooltip("Movement by torque and force")]
        Both
          
    }


    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {

       
        EvaluateCollision(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;

    }
}


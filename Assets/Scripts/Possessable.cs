using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class Possessable : MonoBehaviour

{
    Rigidbody RB;
        [Header("General")]

  

    [SerializeField]
    ForceMode forceMode;

    [SerializeField]
    MoveType moveType = MoveType.Both;

        [Header("Movement by torque")]

    [SerializeField]
    float TorqueMultiplier = 10;
    [SerializeField]
    float MaxAngularVelocity = 10;

        [Header("Movement by force")]

    [SerializeField]
    float MaxSpeed = 10;
    [SerializeField]
    float ForcePower = 1;



   
   
    
    void Start()
    {
        
        RB = GetComponent<Rigidbody>();
        RB.maxAngularVelocity = MaxAngularVelocity;
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

        RB.AddForce((PlayerFollower.Instance.transform.right * force.x) * Time.deltaTime, forceMode);
        RB.AddForce((PlayerFollower.Instance.transform.forward * force.y) * Time.deltaTime, forceMode);

    }
     void ApplyTorque()
    {
        Vector3 torque = PlayerController.Instance.TorqueInput * TorqueMultiplier;

        RB.AddTorque((-PlayerFollower.Instance.transform.forward * torque.x) * Time.deltaTime, forceMode);
        RB.AddTorque((PlayerFollower.Instance.transform.right * torque.y) * Time.deltaTime, forceMode);
       
    }
     enum MoveType
    {
        Torque,
        Force,
        Both
          
    }
    
}

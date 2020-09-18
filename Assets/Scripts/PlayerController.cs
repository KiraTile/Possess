using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Tooltip("Which object is possessed at the start of the scene")]
    [SerializeField]
    Possessable StartingPossessed;
    [Tooltip("Currently possessed object")]
    [SerializeField]
    public static Possessable CurrentPossessed;
    [Tooltip("Possessable object you are currently aiming at")]
    [SerializeField]
    Possessable AimedAt;
    [SerializeField]
    float AimDistance = 3;
    [Tooltip("Radius of the circle at the end of the aim raycast. Used to pick up objects you are not directly aiming at(aim assist)")]
    [SerializeField]
    float AimRadius = 1;
    [SerializeField]
    LayerMask layerMask;
    [Tooltip("Offset of the aim")]
    [SerializeField]
    Vector3 offset;

   
    Vector3 LastAimDirection;
    float HorizontalInput;
    [HideInInspector]
    public Vector3 MovementInput;
    [HideInInspector]
    public Vector3 AimDestination;

    public static UnityEvent OnPossession = new UnityEvent();
    Ray ray;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

        if (StartingPossessed == null)
            StartingPossessed = FindObjectOfType(typeof(Possessable)) as Possessable;

        StartingPossessed.Possess();

    }


    // Update is called once per frame
    void Update()
    {
        Aim();

        
    }
    private void FixedUpdate()
    {
        if (CurrentPossessed)
        {
            CurrentPossessed.Move();
        }
    }

    public void GetHorizontalInput(InputAction.CallbackContext context)
    {
         HorizontalInput = context.ReadValue<float>();
    }

    public void UpdatePlayerLookingDir(InputAction.CallbackContext context)
    {
        LastAimDirection = (CurrentPossessed.transform.position - Camera.main.transform.position).normalized + offset;
    }
    public void UpdateTorqueInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector3>();
    }
    public void Aim()
    {

        ray = new Ray(PlayerFollower.Instance.Target.bounds.center, LastAimDirection);
        RaycastHit hit;
        //Physics.Raycast(CurrentPossessed.transform.position, LastAimDirection, out hit, AimDistance, layerMask,QueryTriggerInteraction.Collide);
        Physics.Raycast(ray,out hit, AimDistance, layerMask, QueryTriggerInteraction.Collide);


        if (hit.collider)
        {
            AimDestination = hit.point;
        }
        else
        {
            AimDestination = ray.GetPoint(AimDistance);
        }
          



        List<Collider> Colliders = new List<Collider>(Physics.OverlapSphere(AimDestination, AimRadius,layerMask));
        Colliders.Remove(CurrentPossessed.GetComponent<Collider>());
        if(Colliders.Count > 0)
        {
            AimedAt = Colliders.Where(obj => obj.gameObject != CurrentPossessed.gameObject).OrderBy(
                x => Vector3.Distance(x.transform.position, AimDestination)
                ).FirstOrDefault().GetComponent<Possessable>();

            OutlineController.Instance.OutlineSingleRenderer(AimedAt);
        }
        else
        {
            AimedAt = null;
            OutlineController.Instance.DisableOutlines();
        }
        
        //Debug.Log(Colliders.Capacity);
    }

    public void Jump()
    {
        CurrentPossessed?.Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
        Gizmos.DrawSphere(AimDestination, AimRadius);
    }

    public void PossessAction(InputAction.CallbackContext context)
    {
        Aim();
        if(context.started && AimedAt)
        {
            AimedAt.Possess();
        }
        
    }
    public void SwitchToGhost()
    {
        UnpossessedPlayer.Instance.Possess();
    }

   


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Tooltip("Currently possessed object")]
    [SerializeField]
    public static Possessable CurrentPossessed;

    [Tooltip("Which object is possessed at the start of the scene")]
    [SerializeField]
    Possessable StartingPossessed;
    
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
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

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
    /// <summary>
    /// originaly this function had frame limiter since its pretty expensive, but for clarity purposes it have been removed
    /// </summary>
    public void Aim()
    {

        ray = new Ray(PlayerFollower.Instance.Target.bounds.center, LastAimDirection);
        RaycastHit hit;
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
        Aim(); //we aim first so we have a target, since we only update our aim at fracture of all frames given 
        if(context.started && AimedAt)
        {
            AimedAt.Possess();
        }
        
    }
    public void SwitchToGhost()
    {
        Debug.Assert(UnpossessedPlayer.Instance, "Unpossessed player(aka ghost) is not initiallized.");
            UnpossessedPlayer.Instance.Possess();
      
        
    }

   


}

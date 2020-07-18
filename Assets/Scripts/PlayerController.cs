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
    public Possessable StartingPossessed;
    [Tooltip("Currently possessed object")]
    public static Possessable CurrentPossessed;
    [Tooltip("Possessable object you are currently aiming at")]
    public Possessable AimedAt;
    public float AimDistance = 3;
    [Tooltip("Radius of the circle at the end of the aim raycast. Used to pick up objects you are not directly aiming at(aim assist)")]
    public float AimRadius = 1;
    
    public LayerMask layerMask;
    [Tooltip("Offset of the aim")]
    public Vector3 offset;

    [HideInInspector]
    public Vector3 LastAimDirection;
    [HideInInspector]
    public float HorizontalInput;
    [HideInInspector]
    public Vector3 TorqueInput;
    public int CurrentlyPossessedLayer;
    public int PossessableLayer;
    public Vector3 AimDestination;

    public static UnityEvent OnPossession = new UnityEvent();
    Ray ray;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

        Possess(StartingPossessed);
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
        TorqueInput = context.ReadValue<Vector2>();
    }
    public void Aim()
    {

        ray = new Ray(CurrentPossessed.GetComponent<Collider>().bounds.center, LastAimDirection);
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

        if(Colliders.Count > 0)
        {
            AimedAt = Colliders.Where(obj => obj.gameObject.layer == PossessableLayer).FirstOrDefault().GetComponent<Possessable>();
            OutlineController.Instance.OutlineSingleRenderer(AimedAt.GetComponent<Renderer>());
        }
        else
        {
            AimedAt = null;
            OutlineController.Instance.DisableOutlines();
        }
        
        //Debug.Log(Colliders.Capacity);
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
            Possess(AimedAt);
        }
        
    }

    public void Possess(Possessable possessable)
    {
        if(CurrentPossessed)
        CurrentPossessed.gameObject.layer = PossessableLayer;
        CurrentPossessed = possessable;
        CurrentPossessed.gameObject.layer = CurrentlyPossessedLayer;
        OnPossession.Invoke();

    }


}

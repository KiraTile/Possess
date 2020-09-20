

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Transform targetObject;

    [SerializeReference]
    float CameraDistance = 3;

    [SerializeReference]
    float CameraPhysicalSize = 0.5f;
  

    [SerializeReference]
    [Range(0f, 1f)]
    float cameraFollowSmoothness = 0.1f;
    [SerializeReference]
    float cameraFollowSpeed;
    Vector2 CameraInput;

    [SerializeReference]
    LayerMask CameraCollisionLayer;

    [SerializeReference]
    float rotationSpeed;
   
    float sizeOfCurrentFollowedObject;


    float y_rotate;
    float x_rotate;

    public static CameraController Instance;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        PlayerController.OnPossession.AddListener(UpdateSizeOfPossessable);
    }
    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {

        //We do an error check
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned. Please assign a target object in the inspector.");
            return;
        }

        OrbitCamera(CameraInput);
    }


     void UpdateSizeOfPossessable()
    {
         sizeOfCurrentFollowedObject = PlayerController.CurrentPossessed.GetComponent<Collider>().bounds.extents.sqrMagnitude;
    }
     void UpdateCameraInput(InputAction.CallbackContext context)
    {
        CameraInput = context.ReadValue<Vector2>();
    }

     void OrbitCamera(Vector2 input)
    {
        y_rotate = input.x * rotationSpeed;
        x_rotate = input.y * rotationSpeed;
      


        transform.LookAt(targetObject);

       

       transform.RotateAround(targetObject.position, Vector3.up, y_rotate * cameraFollowSmoothness);
       transform.RotateAround(targetObject.position, -PlayerFollower.Instance.PlayerRotationFollower.transform.right,  x_rotate * cameraFollowSmoothness);


        


        Ray ray = new Ray(targetObject.transform.position, transform.position - targetObject.transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, CameraDistance + sizeOfCurrentFollowedObject, CameraCollisionLayer, QueryTriggerInteraction.Collide);
       

        Vector3 desiredPosition;
       
        if (hit.collider)
        {
             desiredPosition = (transform.position - targetObject.position).normalized * 
             Vector3.Distance(targetObject.transform.position, hit.point + (hit.point - targetObject.position) * CameraPhysicalSize) + targetObject.position;

            transform.position = desiredPosition;
        }
        else
        {
          
            desiredPosition = (transform.position - targetObject.position).normalized * (CameraDistance + sizeOfCurrentFollowedObject) + targetObject.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.smoothDeltaTime * cameraFollowSpeed);

        }

      

    }


    private void OnDestroy()
    {
        PlayerController.OnPossession.RemoveListener(UpdateSizeOfPossessable);
    }

}


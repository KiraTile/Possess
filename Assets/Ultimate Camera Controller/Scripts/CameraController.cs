

using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    public float CameraDistance = 3;
    

    public float CameraPhysicalSize = 0.5f;
    public Transform targetObject;


    [Range(0f, 1f)]
    public float cameraFollowSmoothness = 0.1f;
    public float cameraFollowSpeed;
    public Vector2 CameraInput;

    public LayerMask CameraCollisionLayer;

    public float rotationSpeed;


    float y_rotate;
    float x_rotate;

    public static CameraController Instance;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


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

    public void UpdateCameraInput(InputAction.CallbackContext context)
    {
        CameraInput = context.ReadValue<Vector2>();
    }

    public void OrbitCamera(Vector2 input)
    {
        y_rotate = input.x * rotationSpeed;
        x_rotate = input.y * rotationSpeed;
      


        transform.LookAt(targetObject);

       

       transform.RotateAround(targetObject.position, Vector3.up, y_rotate * cameraFollowSmoothness);
       transform.RotateAround(targetObject.position, -PlayerFollower.Instance.transform.right,  x_rotate * cameraFollowSmoothness);


        


        Ray ray = new Ray(targetObject.transform.position, transform.position - targetObject.transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, CameraDistance, CameraCollisionLayer, QueryTriggerInteraction.Collide);
       

        Vector3 desiredPosition;
       
        if (hit.collider)
        {
             desiredPosition = (transform.position - targetObject.position).normalized * 
             Vector3.Distance(targetObject.transform.position, hit.point + (hit.point - targetObject.position) * CameraPhysicalSize) + targetObject.position;
            
           
        }
        else
        {
          
            desiredPosition = (transform.position - targetObject.position).normalized * CameraDistance + targetObject.position;
           
        }

        transform.position =  Vector3.MoveTowards(transform.position, desiredPosition, Time.smoothDeltaTime * cameraFollowSpeed);

    }




}




using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    public float CameraDistance = 3;


    public float CameraAnglesConstainUp = 300, CameraAnglesConstainDown = 100;

    public float CameraMaxYDelta = 2, CameraMinYDelta = 0;

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
        transform.RotateAround(targetObject.position, transform.right,  x_rotate * cameraFollowSmoothness);

        float clampedFloat = Mathf.Clamp(transform.eulerAngles.x, CameraAnglesConstainDown, CameraAnglesConstainUp);

        transform.eulerAngles = new Vector3(clampedFloat, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 desiredPosition;
        Ray ray = new Ray(targetObject.transform.position, transform.position - targetObject.transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, CameraDistance, CameraCollisionLayer, QueryTriggerInteraction.Collide);
        Debug.DrawLine(targetObject.transform.position, transform.position);
        if (hit.collider)
        {
             desiredPosition = (transform.position - targetObject.position).normalized * 
                Vector3.Distance(targetObject.transform.position, hit.point + (hit.point - targetObject.position) * CameraPhysicalSize) + targetObject.position;
            Vector3 ConstainedDiseredPosition = new Vector3(desiredPosition.x, Mathf.Clamp(desiredPosition.y, CameraMinYDelta + targetObject.position.y, CameraMaxYDelta + targetObject.position.y),
                desiredPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, ConstainedDiseredPosition, Time.deltaTime * cameraFollowSpeed);
        }
        else
        {
          
            desiredPosition = (transform.position - targetObject.position).normalized * CameraDistance + targetObject.position;
            Vector3 ConstainedDiseredPosition = new Vector3(desiredPosition.x, Mathf.Clamp(desiredPosition.y, CameraMinYDelta + targetObject.position.y, CameraMaxYDelta + targetObject.position.y),
                desiredPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, ConstainedDiseredPosition, Time.deltaTime * cameraFollowSpeed);
        }
       
    }




}


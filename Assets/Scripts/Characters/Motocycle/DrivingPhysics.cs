using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingPhysics : MonoBehaviour
{
    public float accelerationSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float defaultDrag = 1.0f;
    public float brakingDrag = 2.0f;
    public bool debug;
    
    public float currentSpeed;
    private float speed;
    private float rotation;
    private float currentRotation;
    private float speedThresholdToStop = 0.001f;
    private float rotationThresholdToStop = 0.001f;
    private float driftDirection;
    private bool drifting;
    private Rigidbody rb;
    private Transform sphere;
    private Transform model;
    Vector2 input;

    void Start()
    {
        // grabs the needed components
        sphere = transform.GetChild(0);
        rb = sphere.GetComponent<Rigidbody>();
        model = transform.GetChild(1); // this could be better updated to grab the right model but I'm unsure of a way to do so yet
        SetDrag(defaultDrag);
    }

    void Update()
    {
        speed = input.y * accelerationSpeed;
        Steer(input.x, 1);

        if(drifting)
        {
            float control = (driftDirection == 1) ? Mathf.Abs(input.x + 1) : Mathf.Abs(input.x - 1);
            Steer(driftDirection, control);
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        currentRotation = Mathf.Lerp(currentRotation, rotation, Time.deltaTime * 4f);

        if(Mathf.Abs(currentSpeed) < speedThresholdToStop && currentSpeed != 0)
        {
            currentSpeed = 0;
        }
        if(Mathf.Abs(currentRotation) < rotationThresholdToStop && currentRotation != 0)
        {
            currentRotation = 0;
        }
        if(debug)
        {
            Debug.Log("speed:" + speed);
            Debug.Log("currentSpeed:" + currentSpeed);
            Debug.Log("rotation:" + rotation);
            Debug.Log("currentRotation:" + currentRotation);
            Debug.Log("drifting:" + drifting);
            Debug.Log("driftDirection:" + driftDirection);
            Debug.Log("sphere:" + sphere);
            Debug.Log("rb:" + rb);
            Debug.Log("model:" + model);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(model.transform.forward * currentSpeed, ForceMode.Acceleration);
        model.transform.eulerAngles = Vector3.Lerp(model.transform.eulerAngles, new Vector3(0, model.transform.eulerAngles.y + currentRotation), Time.deltaTime * 5f);
    }

    public void MoveEvent(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            input = context.ReadValue<Vector2>();
        }
        else
        {
            input = Vector2.zero;
        }
    }

    public void BrakeEvent(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SetDrag(brakingDrag);
        }
        else
        {
            SetDrag(defaultDrag);
        }
    }

    public void DriftEvent(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            drifting = true;
            driftDirection = input.x > 0 ? 1 : -1;
        }
        else
        {
            drifting = false;
            driftDirection = 0;
        }
    }

    void Steer(float direction, float amount)
    {
        rotation = (rotationSpeed * direction) * amount;
    }

    void SetDrag(float dragVal)
    {
        rb.linearDamping = dragVal;
        rb.angularDamping = dragVal * 0.1f;
    }
}

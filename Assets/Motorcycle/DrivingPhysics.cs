using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingPhysics : MonoBehaviour
{
    public InputActionReference move;
    public float accelerationSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float defaultDrag = 1.0f;
    public float brakingDrag = 2.0f;
    public bool debug;
    private float speed;
    private float currentSpeed;
    private float rotation;
    private float currentRotation;
    private float speedThresholdToStop = 0.001f;
    private float rotationThresholdToStop = 0.001f;
    private float driftDirection;
    private bool drifting;
    private Rigidbody rb;
    private Vector3 movementDirection;
    private Transform model;
    Vector2 input;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
       model = transform.parent.GetChild(1);
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
            rb.linearDamping = brakingDrag;
        }
        else
        {
            rb.linearDamping = defaultDrag;
        }
    }

    public void DriftEvent(InputAction.CallbackContext context)
    {
        drifting = context.performed;
        if(drifting)
        {
            driftDirection = input.x > 0 ? 1 : -1;
        }
        else
        {
            driftDirection = 0;
        }
    }

    void Steer(float direction, float amount)
    {
        rotation = (rotationSpeed * direction) * amount;
    }
}

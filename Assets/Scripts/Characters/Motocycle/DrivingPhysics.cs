using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;
namespace ChemKart
{
    public class DrivingPhysics : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float m_AITurnValue = 1f;
        public DrivingMode drivingMode = DrivingMode.Player;
        public float driftFactor = 0.8f;
        public float accelerationSpeed = 1.0f;
        public float rotationSpeed = 1.0f;
        public float defaultDrag = 1.0f;
        public float brakingDrag = 2.0f;
        public float respawnTime = 3.0f;
        public bool debug;
        public bool shielded;
        public float currentSpeed;
        public enum DrivingMode{AI, Player};
        private float speed;
        private float rotation;
        private float currentRotation;
        private float speedThresholdToStop = 0.001f;
        private float rotationThresholdToStop = 0.001f;
        private float driftDirection;
        private bool drifting;
        private bool damaged;
        private Rigidbody rb;
        private Transform sphere;
        private Transform model;
        private Waypoint mostRecentWaypoint;
        private Vector2 input;

        void Start()
        {
            // grabs the needed components
            sphere = transform.GetChild(0);
            rb = sphere.GetComponent<Rigidbody>();
            model = transform.GetChild(1); // this could be better updated to grab the right model but I'm unsure of a way to do so yet
            mostRecentWaypoint = WaypointManager.m_Waypoints[0];
            //navMeshAgent.acceleration = accelerationSpeed;
            //navMeshAgent.destination = mostRecentWaypoint.m_NextWaypoint.transform.position;
        }

        void Update()
        {
            if(drivingMode == DrivingMode.Player)
            {
                if(!damaged)
                {
                    speed = input.y * accelerationSpeed;
                }
                else
                {
                    speed = 0;
                }
                Steer(input.x, 1);

                if(drifting)
                {
                    float control = (driftDirection == 1) ? Mathf.Abs(input.x + 1) : Mathf.Abs(input.x - 1);
                    Steer(driftDirection, control * driftFactor);
                }
            }
            if (drivingMode == DrivingMode.AI)
            {
                if (!damaged)
                {
                    // Get the direction to the next waypoint
                    Vector3 targetDirection = mostRecentWaypoint.m_NextWaypoint.m_NextWaypoint.transform.position - sphere.position;
                    targetDirection.y = 0; // Keep steering in the horizontal plane
                    targetDirection.Normalize();

                    // Calculate the angle to the target direction
                    Vector3 currentForward = model.forward;
                    currentForward.y = 0; // Ensure horizontal comparison
                    float angleToTarget = Vector3.SignedAngle(currentForward, targetDirection, Vector3.up);

                    // Steer towards the target
                    float steeringInput = Mathf.Clamp(angleToTarget / 45f, -1f, 1f); // Normalize steering input
                    Debug.DrawLine(sphere.position, sphere.position + currentForward * 5, Color.blue);
                    Steer(steeringInput, 1);

                    // Adjust speed based on steering angle
                    if (Mathf.Abs(angleToTarget) > 45f)
                    {
                        Break();
                        //speed = accelerationSpeed / 3; // Slow down on sharp turns
                    }
                    else
                    {
                        speed = accelerationSpeed; // Maintain normal speed otherwise
                    }
                }
                else
                {
                    speed = 0;
                }
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
                Break();
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

        void Break()
        {
            rb.AddForce(-model.transform.forward * currentSpeed/2, ForceMode.Acceleration);
        }
        
        public void Respawn()
        {
            sphere.transform.position = mostRecentWaypoint.transform.position;
            currentSpeed = 0;
        }

        public async void Damage()
        {
            if(shielded)
            {
                Debug.Log("Prevented damage");
                shielded = false;
                return;
            }
            currentSpeed = 0;
            damaged = true;
            await UniTask.Delay(TimeSpan.FromSeconds(respawnTime));
            damaged = false;
            Debug.Log("Damaged");
        }

        public Waypoint GetWaypoint() {return mostRecentWaypoint;}
        public void SetWaypoint(Waypoint waypoint) 
        {
            mostRecentWaypoint = waypoint;
            //navMeshAgent.destination = waypoint.m_NextWaypoint.m_NextWaypoint.transform.position;
        }
    }
}
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;
namespace ChemKart
{
    public class DrivingPhysics : MonoBehaviour
    {
        public enum DrivingMode{AI, Player};
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
        public bool passedRequiredWaypoint;
        private float m_Speed;
        private float m_Rotation;
        private float m_CurrentRotation;
        private float m_SpeedThresholdToStop = 0.001f;
        private float m_RotationThresholdToStop = 0.001f;
        private float m_InputClampThreshold = 0.1f;
        private float m_DriftDirection;
        private bool m_Drifting;
        private bool m_Damaged;
        public bool canAttack; 
        private Rigidbody rb;
        private Transform m_Sphere;
        private Transform m_Model;
        private Waypoint m_MostRecentWaypoint;
        private Vector2 m_Input;
        private long seed = DateTime.Now.Ticks;
        private System.Random random;

        public void Respawn()
        {
            m_Sphere.transform.position = m_MostRecentWaypoint.transform.position;
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
            m_Damaged = true;
            await UniTask.Delay(TimeSpan.FromSeconds(respawnTime));
            m_Damaged = false;
            Debug.Log("Damaged");
        }
        
        public Waypoint GetWaypoint() {return m_MostRecentWaypoint;}
        public void SetWaypoint(Waypoint waypoint) {m_MostRecentWaypoint = waypoint;}

        void Start()
        {
            // grabs the needed components
            m_Sphere = transform.GetChild(0);
            rb = m_Sphere.GetComponent<Rigidbody>();
            m_Model = transform.GetChild(1); // this could be better updated to grab the right model but I'm unsure of a way to do so yet
            m_MostRecentWaypoint = WaypointManager.m_Waypoints[0];
            random = new System.Random((int)seed);
            
            
        }

        void Update()
        {
            if(drivingMode == DrivingMode.Player)
            {
                if(!m_Damaged)
                {
                    m_Speed = m_Input.y * accelerationSpeed;
                }
                else
                {
                    m_Speed = 0;
                }
                Steer(m_Input.x, 1);

                if(m_Drifting)
                {
                    float control = (m_DriftDirection == 1) ? Mathf.Abs(m_Input.x + 1) : Mathf.Abs(m_Input.x - 1);
                    Steer(m_DriftDirection, control * driftFactor);
                }
            }
            if (drivingMode == DrivingMode.AI)
            {
                if (!m_Damaged)
                {
                    float angleToTarget = AISteer();

                    if (Mathf.Abs(angleToTarget) > 45f)
                    {
                        Break();
                    }
                    else
                    {
                        m_Speed = accelerationSpeed;
                    }
                }
                else
                {
                    m_Speed = 0;
                }
            }

            currentSpeed = Mathf.SmoothStep(currentSpeed, m_Speed, Time.deltaTime * 12f);
            m_CurrentRotation = Mathf.Lerp(m_CurrentRotation, m_Rotation, Time.deltaTime * 4f);

            StopIfBelowTheThreshold();

            if(debug)
            {
                PrintMembers();
            }
        }

        void FixedUpdate()
        {
            rb.AddForce(m_Model.transform.forward * currentSpeed, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * 9.8f);
            m_Model.transform.eulerAngles = Vector3.Lerp(m_Model.transform.eulerAngles, new Vector3(0, m_Model.transform.eulerAngles.y + m_CurrentRotation), Time.deltaTime * 5f);
        }

        public void MoveEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                m_Input = context.ReadValue<Vector2>();
                if(Mathf.Abs(m_Input.x) < m_InputClampThreshold)
                {
                    m_Input.x = 0;
                }
                if(Mathf.Abs(m_Input.y) < m_InputClampThreshold)
                {
                    m_Input.y = 0;
                }
            }
            else
            {
                m_Input = Vector2.zero;
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
                m_Drifting = true;
                m_DriftDirection = m_Input.x > 0 ? 1 : -1;
            }
            else
            {
                m_Drifting = false;
                m_DriftDirection = 0;
            }
        }

        void Steer(float direction, float amount)
        {
            m_Rotation = (rotationSpeed * direction) * amount;
        }

        float AISteer()
        {
            Vector3 targetDirection = m_MostRecentWaypoint.nextWaypoint.nextWaypoint.transform.position - m_Sphere.position;
            targetDirection.y = 0; 
            targetDirection.Normalize();

            Vector3 currentForward = m_Model.forward;
            currentForward.y = 0;
            float angleToTarget = Vector3.SignedAngle(currentForward, targetDirection, Vector3.up);

            float steeringInput = Mathf.Clamp(angleToTarget / 45f, -1f, 1f);
            steeringInput += (float)random.NextDouble() * 0.2f; // this is to attempt to add some sort of randomness to the AI's driving
            Steer(steeringInput, 1);
            return angleToTarget;
        }

        void Break()
        {
            rb.AddForce(-m_Model.transform.forward * currentSpeed/2, ForceMode.Acceleration);
        }

        void StopIfBelowTheThreshold()
        {
            if(Mathf.Abs(currentSpeed) < m_SpeedThresholdToStop && currentSpeed != 0)
            {
                currentSpeed = 0;
            }
            if(Mathf.Abs(m_CurrentRotation) < m_RotationThresholdToStop && m_CurrentRotation != 0)
            {
                m_CurrentRotation = 0;
            }
        }

        public void StopMovement()
        {
            currentSpeed = 0;
            m_Drifting = false;
            m_DriftDirection = 0;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public void ResetPosition(Vector3 newPosition)
        {
            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics temporarily
            }

            m_Sphere.transform.position = newPosition;

            if (rb != null)
            {
                rb.isKinematic = false; // Re-enable physics
            }
        }

        public void ResetRotation(Quaternion newRotation)
        {
            // Stop rotation-related forces
            if (rb != null)
            {
                rb.angularVelocity = Vector3.zero;
            }

            // Apply new rotation
            m_Model.rotation = newRotation;
        }

        public void MaintainVelocity(Vector3 forwardDirection)
        {
            if (rb != null)
            {
                rb.linearVelocity = forwardDirection * currentSpeed;
            }
        }

        void PrintMembers()
        {
            Debug.Log("speed:" + m_Speed);
            Debug.Log("currentSpeed:" + currentSpeed);
            Debug.Log("rotation:" + m_Rotation);
            Debug.Log("currentRotation:" + m_CurrentRotation);
            Debug.Log("drifting:" + m_Drifting);
            Debug.Log("driftDirection:" + m_DriftDirection);
            Debug.Log("sphere:" + m_Sphere);
            Debug.Log("rb:" + rb);
            Debug.Log("model:" + m_Model);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class KartDrivingController : MonoBehaviour
    {
        private KartController controller;

        public float accelerationRate = 1.0f;
        public float decelerationRate = 1.0f;
        public bool canDrive;
        [SerializeField] private float rotationSpeed = 1.0f;
        [SerializeField] private float driftFactor = 0.8f;
        [SerializeField] private float weight = 1.0f;
        private float speed;
        private float rotation;
        private float currentRotation;
        private float speedThreshold = 0.001f;
        private float rotationThreshold = 0.001f;

        private void Start()
        {
            controller = GetComponent<KartController>();
            CountdownManager.onCountdownFinished += OnCountdownFinished;
        }

        private void Update()
        {
            Vector2 input = controller.InputHandler.MoveInput;

            if (!controller.DamageHandler.IsDamaged && canDrive)
                speed = input.y * accelerationRate;
            else
                speed = 0;

            rotation = rotationSpeed * input.x;

            if (controller.InputHandler.IsDrifting)
            {
                float control = controller.InputHandler.DriftDirection == 1 ? Mathf.Abs(input.x + 1) : Mathf.Abs(input.x - 1);
                rotation = rotationSpeed * controller.InputHandler.DriftDirection * control * driftFactor;
            }

            controller.CurrentSpeed = Mathf.MoveTowards(controller.CurrentSpeed, speed, Time.deltaTime * (speed > controller.CurrentSpeed ? accelerationRate : decelerationRate));

            currentRotation = Mathf.Lerp(currentRotation, rotation, Time.deltaTime * 4f);

            RotateWheels();
            StopIfBelowThreshold();
        }

        private void FixedUpdate()
        {
            controller.Rigidbody.AddForce(controller.Model.forward * controller.CurrentSpeed, ForceMode.Acceleration);
            controller.Rigidbody.AddForce(Vector3.down * 9.8f * weight);
            controller.Model.transform.eulerAngles = Vector3.Lerp(
                controller.Model.transform.eulerAngles,
                new Vector3(0, controller.Model.transform.eulerAngles.y + currentRotation, 0),
                Time.deltaTime * 5f);
        }

        public void Brake()
        {
            controller.Rigidbody.AddForce(-controller.Model.forward * controller.CurrentSpeed / 2f, ForceMode.Acceleration);
        }

        private void StopIfBelowThreshold()
        {
            if (Mathf.Abs(controller.CurrentSpeed) < speedThreshold && controller.CurrentSpeed != 0)
                controller.CurrentSpeed = 0;

            if (Mathf.Abs(currentRotation) < rotationThreshold && currentRotation != 0)
                currentRotation = 0;
        }

        private void RotateWheels()
        {
            if (controller.FrontWheel != null)
                controller.FrontWheel.Rotate(Vector3.forward * controller.CurrentSpeed * Time.deltaTime * 360f);
            if (controller.RearWheel != null)
                controller.RearWheel.Rotate(Vector3.forward * controller.CurrentSpeed * Time.deltaTime * 360f);
        }

        public void Boost(float factor)
        {
            controller.CurrentSpeed = accelerationRate * factor;
        }

        public void StopMovement()
        {
            controller.CurrentSpeed = 0;
            controller.InputHandler.IsDrifting = false;
            controller.InputHandler.DriftDirection = 0;

            if (controller.Rigidbody != null)
            {
                controller.Rigidbody.linearVelocity = Vector3.zero;
                controller.Rigidbody.angularVelocity = Vector3.zero;
            }
        }

        public void ResetPosition(Vector3 newPosition)
        {
            if (controller.Rigidbody != null)
                controller.Rigidbody.isKinematic = true;

            controller.Sphere.transform.position = newPosition;

            if (controller.Rigidbody != null)
                controller.Rigidbody.isKinematic = false;
        }

        public void ResetRotation(Quaternion newRotation)
        {
            if (controller.Rigidbody != null)
                controller.Rigidbody.angularVelocity = Vector3.zero;

            controller.Model.rotation = newRotation;
        }

        private void OnCountdownFinished()
        {
            canDrive = true;
        }
    }
}
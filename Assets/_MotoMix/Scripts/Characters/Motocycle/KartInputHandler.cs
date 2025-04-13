using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class KartInputHandler : MonoBehaviour
    {
        private KartController controller;

        public Vector2 MoveInput;
        public PlayerInput playerInput { get; private set; }
        public bool IsDrifting { get; set; }
        public int DriftDirection { get; set; }

        private void Start()
        {
            controller = GetComponent<KartController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveInput = context.ReadValue<Vector2>();
                if (!controller.EngineRevving.isPlaying)
                    controller.EngineRevving.PlayClip();
            }
            else if (context.canceled)
            {
                MoveInput = Vector2.zero;
                controller.EngineRevving.StopLooping();
            }
        }

        public void OnDrift(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsDrifting = true;
                DriftDirection = MoveInput.x > 0 ? 1 : -1;
            }
            else if (context.canceled)
            {
                IsDrifting = false;
                DriftDirection = 0;
            }
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            if (context.performed)
                controller.DrivingController.Brake();
        }

        public void OnCameraToggle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector3 pos = controller.KartCamera.transform.localPosition;
                pos.z *= -1;
                controller.KartCamera.transform.localPosition = pos;

                float y = controller.KartCamera.transform.eulerAngles.y;
                y = (y + 180f) % 360f;
                controller.KartCamera.transform.eulerAngles = new Vector3(0, y, 0);
            }
        }
    }
}
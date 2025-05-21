using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class ModelBehavior : MonoBehaviour
    {
        private KartController controller;

        [SerializeField] private Transform character;
        [SerializeField] private Transform bikeBody;
        [SerializeField] private Transform handleBars;
        [SerializeField] private Transform frontWheel;
        [SerializeField] private Transform backWheel;
        [SerializeField] private float leanSpeed = 5f;
        [SerializeField] private float leanLimit = 15f;
        [SerializeField] private float leanAdd = 45f;
        [SerializeField] private float leanWheelLimit = 20f;

        private Quaternion targetLean;
        private Quaternion targetLeanWheel;
        private Quaternion targetLeanBike;
        private Quaternion targetLeanHandle;

        private void Start()
        {
            controller = GetComponent<KartController>();
            Debug.Log("ModelBehavior script started");
        }

        private void Update()
        {
            Vector2 input = controller.InputHandler.MoveInput;
            if (input.x != 0)
            {
                float leanAngle = -input.x * leanLimit;
                float leanUpdate = leanAngle + leanAdd;
                float leanWheel = -input.x * leanWheelLimit;
                targetLean = Quaternion.Euler(0, 0, leanAngle);
                targetLeanWheel = Quaternion.Euler(-leanWheel, 90, -90);
                targetLeanBike = Quaternion.Euler(leanUpdate, -90, 90);
                targetLeanHandle = Quaternion.Euler(leanAngle, 0, 180);

            }
            else
            {
                targetLean = Quaternion.identity;
                targetLeanWheel = Quaternion.Euler(0, 90, -90);
                targetLeanBike = Quaternion.Euler(90, 0, 180);
                targetLeanHandle = Quaternion.Euler(90, 0, 180);
            }
            character.localRotation = Quaternion.Slerp(character.localRotation, targetLean, Time.deltaTime * leanSpeed);
            bikeBody.localRotation = Quaternion.Slerp(bikeBody.localRotation, targetLeanBike, Time.deltaTime * leanSpeed);
            handleBars.localRotation = Quaternion.Slerp(handleBars.localRotation, targetLeanBike, Time.deltaTime * leanSpeed);
            frontWheel.localRotation = Quaternion.Slerp(frontWheel.localRotation, targetLeanWheel, Time.deltaTime * leanSpeed);
            backWheel.localRotation = Quaternion.Slerp(backWheel.localRotation, targetLeanWheel, Time.deltaTime * leanSpeed);

        }
        
    }
}

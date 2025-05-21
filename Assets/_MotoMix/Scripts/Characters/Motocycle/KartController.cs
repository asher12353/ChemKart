using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class KartController : MonoBehaviour
    {
        [SerializeField] private Camera kartCamera;
        [SerializeField] private AudioClipPlayer engineRevving;
        [SerializeField] private Transform frontWheel;
        [SerializeField] private Transform rearWheel;

        public Rigidbody Rigidbody { get; private set; }
        public Transform Sphere { get; private set; }
        public Transform Model { get; private set; }
        public Camera KartCamera => kartCamera;
        public AudioClipPlayer EngineRevving => engineRevving;
        public Transform FrontWheel => frontWheel;
        public Transform RearWheel => rearWheel;

        public Waypoint CurrentWaypoint { get; set; }
        public bool PassedRequiredWaypoint { get; set; }
        public float CurrentSpeed { get; set; }

        public KartInputHandler InputHandler { get; private set; }
        public KartDrivingController DrivingController { get; private set; }
        public KartDamageHandler DamageHandler { get; private set; }
        public ModelBehavior Behavior { get; private set; }

        private void Awake()
        {
            Sphere = GetComponentInChildren<Sphere>().transform;
            Rigidbody = Sphere.GetComponent<Rigidbody>();
            Model = GetComponentInChildren<MotorcycleModel>().transform;
            CurrentWaypoint = WaypointGenerator.waypoints[0];

            InputHandler = gameObject.GetComponent<KartInputHandler>();
            DrivingController = gameObject.GetComponent<KartDrivingController>();
            DamageHandler = gameObject.GetComponent<KartDamageHandler>();
            Behavior = gameObject.GetComponent<ModelBehavior>();
        }
    }
}
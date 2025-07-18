using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace ChemKart
{
    public class Portal : MonoBehaviour
    {
        [Header("Reference")]
        public GameObject connectedPortal; // reference to the connected
        public enum Type
        {
            Enter, 
            Exit
        }; 

        public enum PortalColor
        {
            R, // red  
            Y, // yellow
            G, // green
            B // blue
        };

        public Type type;
        public PortalColor color;

        private Dictionary<PortalColor, Color> colorDictionary;

        [SerializeField]
        private Material material;

        void Start()
        {
            colorDictionary = new Dictionary<PortalColor, Color>
            {
                { PortalColor.R, Color.red },
                { PortalColor.Y, Color.yellow },
                { PortalColor.G, Color.green },
                { PortalColor.B, Color.blue }

            };

            Renderer renderer = GetComponent<Renderer>();

            if(renderer != null ) 
            {
                material = renderer.material;
                UpdateMaterialColor(); 
            } 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (type == Type.Enter && other.gameObject.tag == "Motorcycle")
            {
                Debug.Log("Trigger Entered!"); 
                Teleport(other.transform.parent); // this should be the motorcycle
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exited the portal trigger"); 
        }

        private void UpdateMaterialColor()
        {
            if(material != null && colorDictionary.ContainsKey(color)) 
            {
                material.SetColor("Color_739931D1", colorDictionary[color]);
                Debug.Log("Color Updated"); 
            }
            else
            {
                Debug.LogError("Material not assigned, or color is not defined in the dictionary"); 
            }
        }

        private void Teleport(Transform target)
        {
            if (target == null || connectedPortal == null)
                return;

            KartDrivingController drivingController = target.GetComponent<KartDrivingController>();
            if (drivingController != null)
            {
                // store speed at the time the portal is entered 
                float speed = drivingController.GetComponent<KartController>().CurrentSpeed; 
                // Stop movement
                drivingController.StopMovement();

                // Reset Rigidbody physics (if any)
                Rigidbody rb = target.GetComponentInChildren<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                // Teleport position
                drivingController.ResetPosition(connectedPortal.transform.position);

                // Adjust rotation
                Quaternion portalRotation = connectedPortal.transform.rotation * Quaternion.Euler(0, -90, 0); // Optional adjustment
                drivingController.ResetRotation(portalRotation);

                // give back current speed 
                rb.linearVelocity = portalRotation * Vector3.forward * speed;


            }

            Debug.Log($"Teleported to: {connectedPortal.transform.position} with rotation: {connectedPortal.transform.rotation}");
        }
    }
}

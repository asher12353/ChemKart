using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static ChemKart.Portal;

namespace ChemKart
{
    public class PortalTrigger : MonoBehaviour
    {
        [Header("References")]
        public GameObject enterPortal; 
        public GameObject exitPortal;

        private Portal portalInfo;
        private float portalResetTime;

        private Dictionary<PortalColor, Color> colorDictionary; 
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            portalInfo = enterPortal.GetComponent<Portal>(); 
            DeactivatePortals();
            portalResetTime = 5f;

            colorDictionary = new Dictionary<PortalColor, Color>
            {
                { PortalColor.R, Color.red },
                { PortalColor.Y, Color.yellow },
                { PortalColor.G, Color.green },
                { PortalColor.B, Color.blue }

            };

            UpdateTriggerColor(); 
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                List<Collectable> items = inventory.M_Items();
                foreach (Collectable item in items)
                {
                    if(item is Chemical)
                    {
                        Chemical chemical = item as Chemical;
                        Debug.Log(chemical.chemicalValue);
                        Debug.Log(portalInfo.color.ToString()); 
                        if(chemical.chemicalValue == portalInfo.color.ToString())
                        {
                            ActivatePortals();
                            inventory.RemoveItem(item);
                            return;
                        }
                    }

                }
            }
        }

        private void ActivatePortals()
        {
            enterPortal.SetActive(true);
            exitPortal.SetActive(true); 
            StartCoroutine(ResetPortals());
        }

        private void DeactivatePortals()
        {
            enterPortal.SetActive(false); 
            exitPortal.SetActive(false); 
        }

        private IEnumerator ResetPortals()
        {
            yield return new WaitForSeconds(portalResetTime);
            DeactivatePortals(); 
        }

        private void UpdateTriggerColor()
        {
            Renderer renderer = GetComponent<Renderer>();
            Material material = renderer.material;
            material.color = colorDictionary[portalInfo.color]; 
            
        }
    }
}

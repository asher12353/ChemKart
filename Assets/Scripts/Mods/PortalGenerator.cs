using UnityEngine;
using System.Collections.Generic;

namespace ChemKart
{
    public class PortalGenerator : MonoBehaviour
    {
        public GameObject portalTransforms;
        public GameObject portalPrefab;

        public void GeneratePortals()
        {
            if(portalTransforms == null)
            {
                Debug.LogError("Can't find the portal parent, aborting");
                return;
            }
            List<GameObject> portals = new List<GameObject>();
            foreach(Transform child in portalTransforms.transform)
            {
                GameObject portal = Instantiate(portalPrefab, child.position, child.rotation);
                Transform enterPortal = child.GetChild(0).GetChild(0);
                Transform exitPortal = child.GetChild(0).GetChild(1);
                Transform trigger = child.GetChild(1);

                Vector3 enterPortalVector = enterPortal.position + new Vector3(0, 0.5f, 0);
                portal.transform.GetChild(0).GetChild(0).transform.position = enterPortalVector;
                portal.transform.GetChild(0).GetChild(0).transform.rotation = enterPortal.rotation * Quaternion.Euler(0, 90f, 0);

                Vector3 exitPortalVector = exitPortal.position + new Vector3(0, 0.5f, 0);
                portal.transform.GetChild(0).GetChild(1).transform.position = exitPortalVector;
                portal.transform.GetChild(0).GetChild(1).transform.rotation = exitPortal.rotation * Quaternion.Euler(0, 90f, 0);

                portal.transform.GetChild(1).transform.position = enterPortal.position;
                portal.transform.GetChild(1).transform.rotation = enterPortal.rotation;

                Portal portalComponent = portal.GetComponent<Portal>();

                if(portal.transform.GetChild(0).name == "R")
                {
                    portalComponent.color = Portal.PortalColor.R;
                }
                if(portal.transform.GetChild(0).name == "Y")
                {
                    portalComponent.color = Portal.PortalColor.Y;
                }
                if(portal.transform.GetChild(0).name == "B")
                {
                    portalComponent.color = Portal.PortalColor.B;
                }

                portals.Add(portal);
            }

            foreach(GameObject portal in portals)
            {
                portal.transform.SetParent(portalTransforms.transform);
            }
        }
    }
}

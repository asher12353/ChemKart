using System.Collections;
using UnityEngine;

namespace ChemKart
{
    public class Portal : MonoBehaviour
    {
        [Header("Reference")]
        public GameObject connectedPortal; 
        public enum Type
        {
            Enter, 
            Exit
        }; 

        public enum Color
        {
            Red, 
            Yellow, 
            Green
        };

        public Type type;
        public Color color; 
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

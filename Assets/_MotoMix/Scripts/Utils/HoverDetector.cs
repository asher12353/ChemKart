using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ChemKart
{
    public class HoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsHovered { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Mouse>() != null)
                IsHovered = true;
            else
                IsHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
        }
    }
}

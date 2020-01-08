using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent onDown;
        public UnityEvent onUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onUp?.Invoke();
        }
    }
}

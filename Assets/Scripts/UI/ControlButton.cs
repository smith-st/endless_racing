using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnDown;
        public UnityEvent OnUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
        }
    }
}

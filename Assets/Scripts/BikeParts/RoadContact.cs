using UnityEngine;

namespace BikeParts
{
    [RequireComponent(typeof(Collider2D))]
    public class RoadContact : MonoBehaviour
    {
        public bool IsContact { get; private set; }

        private IRoadContactListener _listener;

        public void SetListener(IRoadContactListener listener)
        {
            _listener = listener;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Road")) return;
            _listener?.StartContactWithRoad(gameObject.tag);
            IsContact = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Road")) return;
            _listener?.StopContactWithRoad(gameObject.tag);
            IsContact = false;
        }
    }
}
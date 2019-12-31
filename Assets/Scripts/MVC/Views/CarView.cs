using CarParts;
using UnityEngine;

namespace MVC.Views
{
    [RequireComponent(typeof(WheelJoint2D))]
    public class CarView : MonoBehaviour
    {
        public WheelJoint2D wheelJointRear;
        public WheelJoint2D wheelJointFront;
        public Rigidbody2D wheelRigidbodyRear;
        public Rigidbody2D wheelRigidbodyFront;
        public RoadContact wheelRear;
        public RoadContact wheelFront;
        public RoadContact roof;
    }
}

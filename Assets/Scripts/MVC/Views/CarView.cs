using System;
using System.Collections;
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

        private SpriteRenderer[] _sprites;
        private void Awake()
        {
            _sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        }

        public void Blink()
        {
            StartCoroutine(BlinkRoutine());
        }

        private IEnumerator BlinkRoutine()
        {
            const float delay = 0.1f;
            for (int i = 1; i <= 5; i++)
            {
                ShowForAllSprites(false);
                yield return new WaitForSeconds(delay);
                ShowForAllSprites(true);
                yield return new WaitForSeconds(delay);
            }
        }

        private void ShowForAllSprites(bool isShow)
        {
            foreach (var sprite in _sprites)
            {
                sprite.color = isShow?Color.white : Color.clear;
            }
        }
    }
}

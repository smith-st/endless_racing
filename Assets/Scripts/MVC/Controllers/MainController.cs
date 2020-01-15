using System;
using MVC.Models;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Controllers
{
    public class MainController : MonoBehaviour
    {
        public GameObject roadPrefab;
        public GameObject bikePrefab;
        [Space]
        public Text textDistance;
        public WheelieText wheelie;
        public Slider speedSlider;

        private const float DistanceDivisor = 10f;

        private Transform _cameraTransform;
        private RoadModel _road;
        private BikeModel _bike;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            CheckPrefabs();
            _cameraTransform = Camera.main.transform;
            _road = new RoadModel(roadPrefab);
            _bike = new BikeModel(bikePrefab, new Vector2(0f, 1f));
            ChangeBikeMaxSpeed();
        }

        private void Update()
        {
            _cameraTransform.position = new Vector3(_bike.Position.x+3f, _bike.Position.y+1.5f, _cameraTransform.position.z);
            _road.BikePosition(_bike.Position);
            textDistance.text = Mathf.Max(0f ,Mathf.Round(_bike.Position.x/DistanceDivisor)).ToString();
            _bike.Update();
        }
        private void FixedUpdate()
        {
            wheelie.Display(_bike.onRearWheel);
            _bike.FixedUpdate();
        }

        private void CheckPrefabs()
        {
            if (bikePrefab == null)
            {
                throw new Exception("Bike prefab not assigned");
            }

            if (roadPrefab == null)
            {
                throw new Exception("Road prefab not assigned");
            }
        }

        public void Accelerate(bool value)
        {
            if (value)
            {
                _bike.Accelerate();
            }
            else
            {
                _bike.Neutral();
            }
        }

        public void Break(bool value)
        {
            if (value)
            {
                _bike.Break();
            }
            else
            {
                _bike.Neutral();
            }
        }

        public void ChangeBikeMaxSpeed()
        {
            _bike.ChangeMaxSpeed(speedSlider.value);
        }
    }
}

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
        public GameObject carPrefab;
        [Space]
        public Text textDistance;
        public WheelieText wheelie;
        public Slider speedSlider;

        private const float DistanceDivisor = 10f;

        private Transform _cameraTransform;
        private RoadModel _road;
        private CarModel _car;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            CheckPrefabs();
            _cameraTransform = Camera.main.transform;
            _road = new RoadModel(roadPrefab);
            _car = new CarModel(carPrefab, new Vector2(0f, 1f));
            ChangeCarMaxSpeed();
        }

        private void Update()
        {
            _cameraTransform.position = new Vector3(_car.Position.x+3f, _car.Position.y+1.5f, _cameraTransform.position.z);
            _road.CarPosition(_car.Position);
            textDistance.text = Mathf.Round(_car.Position.x/DistanceDivisor).ToString();
            _car.Update();
        }
        private void FixedUpdate()
        {
            wheelie.Display(_car.InAir);
            _car.FixedUpdate();
        }

        private void CheckPrefabs()
        {
            if (carPrefab == null)
            {
                throw new Exception("Car prefab not assigned");
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
                _car.Accelerate();
            }
            else
            {
                _car.Neutral();
            }
        }

        public void Break(bool value)
        {
            if (value)
            {
                _car.Break();
            }
            else
            {
                _car.Neutral();
            }
        }

        public void ChangeCarMaxSpeed()
        {
            _car.ChangeMaxSpeed(speedSlider.value);
        }
    }
}

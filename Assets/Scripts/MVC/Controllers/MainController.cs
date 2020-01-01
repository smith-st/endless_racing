using System;
using MVC.Models;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _roadPrefab;
        [SerializeField]
        private GameObject _carPrefab;
        [Space]
        [SerializeField]
        private Text _textDistance;
        [SerializeField]
        private WheelieText _wheelie;
        [SerializeField]
        private Slider _speedSlider;

        private const float DistanceDivisor = 10f;

        private Camera _camera;
        private Transform _cameraTransform;
        private RoadModel _road;
        private CarModel _car;
        private bool? _accelerate;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            CheckPrefabs();
            _camera = Camera.main;
            _cameraTransform = _camera.transform;
            _road = new RoadModel(_roadPrefab);
            _car = new CarModel(_carPrefab, new Vector2(0f, 1f));
        }

        private void Update()
        {
            _cameraTransform.position = new Vector3(_car.Position.x+3f, _car.Position.y+1.5f, _cameraTransform.position.z);
            _road.CarPosition(_car.Position);
            _textDistance.text = Mathf.Round(_car.Position.x/DistanceDivisor).ToString();
            _car.Update();
        }
        private void FixedUpdate()
        {
            _wheelie.Display(_car.InAir);
            _car.FixedUpdate();
        }

        private void CheckPrefabs()
        {
            if (_carPrefab == null)
            {
                throw new Exception("Car prefab not assigned");
            }

            if (_roadPrefab == null)
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
            _car.ChangeMaxSpeed(_speedSlider.value);
        }
    }
}

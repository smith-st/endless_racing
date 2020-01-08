using System.Collections;
using CarParts;
using MVC.Views;
using UnityEngine;

namespace MVC.Models
{
    public class CarModel: BaseModel, IRoadContactListener
    {
        private const float SpeedUpStep = 0.007f;
        private const float RespawnDelay = 3f;
        private const float AllowedRollbackDistance = 20f;

        public Vector2 Position => _car.transform.position;
        public bool InAir => !_car.wheelFront.IsContact && !_car.wheelRear.IsContact;

        private enum Status
        {
            Accelerate,
            Break,
            Neutral
        }

        private CarView _car;
        private JointMotor2D _wheelFrontMotor;
        private JointMotor2D _wheelRearMotor;
        private Coroutine _roofCoroutine;
        private float _maxDistance;
        private Status _status;
        private float _currentSpeedPercent;
        private float _maxSpeed;
        public CarModel(GameObject prefab, Vector2 startupPosition)
        {
            _car = Object.Instantiate(prefab, startupPosition, Quaternion.identity).GetComponent<CarView>();
            CheckView(_car);
            ReceiveMotorsFromWheels();
            _car.roof.SetListener(this);
            SetSpeed(0f);
            Neutral();
        }

        public void Update()
        {
            _maxDistance = Mathf.Max(_maxDistance, _car.transform.position.x);
            if (_status == Status.Neutral && _maxDistance - Position.x > AllowedRollbackDistance)
            {
                Break();
            }
        }

        public void FixedUpdate()
        {
            if (_currentSpeedPercent < 1f)
            {
                _currentSpeedPercent += SpeedUpStep;
                SetSpeedInPercent(_currentSpeedPercent);
            }
        }

        public void ChangeMaxSpeed(float value)
        {
            _maxSpeed = value;
        }

        public void Accelerate()
        {
            Debug.Log("Accelerate");
            _status = Status.Accelerate;
            UseMotor(true);
            _currentSpeedPercent = GetCurrentSpeedInPercent();
        }

        public void Break()
        {
            Debug.Log("Break");
            _status = Status.Break;
            UseMotor(true);
            SetSpeed(0f);
            _currentSpeedPercent = 1f;
        }

        public void Neutral()
        {
            Debug.Log("Neutral");
            _status = Status.Neutral;
            UseMotor(false);
            _currentSpeedPercent = 1f;
        }

        private void SetSpeedInPercent(float percent)
        {
            SetSpeed(-_maxSpeed * percent);
        }

        private void SetSpeed(float value)
        {
            _wheelFrontMotor.motorSpeed = value;
            _wheelRearMotor.motorSpeed = value;
            _car.wheelJointFront.motor = _wheelFrontMotor;
            _car.wheelJointRear.motor = _wheelRearMotor;
        }

        public void StartContactWithRoad(string tag)
        {
            if (_roofCoroutine != null)
            {
                return;
            }

            _roofCoroutine = _car.StartCoroutine(CarOnRoof());
        }

        public void StopContactWithRoad(string tag)
        {
            if (_roofCoroutine != null)
            {
                _car.StopCoroutine(_roofCoroutine);
                _roofCoroutine = null;
            }
        }

        private IEnumerator CarOnRoof()
        {
            yield return new WaitForSeconds(RespawnDelay);
            RespawnCar();
            _roofCoroutine = null;
        }

        private void RespawnCar()
        {
            var transform = _car.transform;
            var position = transform.position;
            position = new Vector3(position.x, position.y + 2f);
            transform.position = position;
            transform.rotation = Quaternion.identity;
            _car.Blink();
        }

        private void UseMotor(bool value)
        {
            _car.wheelJointFront.useMotor = value;
            _car.wheelJointRear.useMotor = value;
        }

        private float GetCurrentSpeedInPercent()
        {
            return (_car.wheelRigidbodyRear.angularVelocity + _car.wheelRigidbodyFront.angularVelocity) / 2f / _maxSpeed * -1f;
        }

        private void ReceiveMotorsFromWheels()
        {
            _wheelFrontMotor = _car.wheelJointFront.motor;
            _wheelRearMotor = _car.wheelJointRear.motor;
        }
    }
}

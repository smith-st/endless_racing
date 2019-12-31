using System.Collections;
using CarParts;
using MVC.Views;
using UnityEngine;

namespace MVC.Models
{
    public class CarModel: BaseModel, IRoadContactListener
    {
        private const float Speed = 2000f;
        private const float SpeedUpStep = 0.007f;
        private const float RespawnDelay = 3f;
        private const float AllowedBackDistance = 20f;
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
        public CarModel(GameObject prefab, Vector2 startupPosition)
        {
            _car = GameObject.Instantiate(prefab, startupPosition, Quaternion.identity).GetComponent<CarView>();
            CheckView(_car);
            _wheelFrontMotor = _car.wheelJointFront.motor;
            _wheelRearMotor = _car.wheelJointRear.motor;
            _car.roof.SetListener(this);
            _car.wheelFront.SetListener(this);
            _car.wheelRear.SetListener(this);
            SetZeroSpeed();
            Neutral();
        }

        public void Update()
        {
            _maxDistance = Mathf.Max(_maxDistance, _car.transform.position.x);
            if (_status == Status.Neutral && _maxDistance - Position.x > AllowedBackDistance)
            {
                Break();
            }
        }

        public void FixedUpdate()
        {
            if (_currentSpeedPercent < 1f)
            {
                _currentSpeedPercent += SpeedUpStep;
                SetIntermediateSpeed(_currentSpeedPercent);
            }
        }
        public void Accelerate()
        {
            Debug.Log("Accelerate");
            _status = Status.Accelerate;
            _car.wheelJointFront.useMotor = true;
            _car.wheelJointRear.useMotor = true;
            _currentSpeedPercent = (_car.wheelRigidbodyRear.angularVelocity + _car.wheelRigidbodyFront.angularVelocity) / 2f / Speed * -1f;
        }

        public void Break()
        {
            Debug.Log("Break");
            _status = Status.Break;
            _car.wheelJointFront.useMotor = true;
            _car.wheelJointRear.useMotor = true;
            SetZeroSpeed();
            _currentSpeedPercent = 1f;
        }

        public void Neutral()
        {
            Debug.Log("Neutral");
            _status = Status.Neutral;
            _car.wheelJointFront.useMotor = false;
            _car.wheelJointRear.useMotor = false;
            _currentSpeedPercent = 1f;
        }

        public void SetIntermediateSpeed(float percent)
        {
            SetSpeed(-Speed*percent);
        }

        private void SetZeroSpeed()
        {
            SetSpeed(0f);
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
            if (tag == "CarRoof")
            {
                if (_roofCoroutine != null)
                {
                    return;
                }

                _roofCoroutine = _car.StartCoroutine(CarOnRoof());
            }
        }

        public void StopContactWithRoad(string tag)
        {
            if (tag == "CarRoof")
            {
                if (_roofCoroutine != null)
                {
                    _car.StopCoroutine(_roofCoroutine);
                    _roofCoroutine = null;
                }
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
    }
}

using System.Collections;
using BikeParts;
using MVC.Views;
using UnityEngine;

namespace MVC.Models
{
    public class BikeModel: BaseModel, IRoadContactListener
    {
        private const float SpeedUpStep = 0.01f;
        private const float RespawnDelay = 3f;
        private const float AllowedRollbackDistance = 20f;

        public Vector2 Position => _bike.transform.position;
        public bool onRearWheel => !_bike.wheelFront.IsContact && _bike.wheelRear.IsContact;

        private enum Status
        {
            Accelerate,
            Break,
            Neutral
        }

        private BikeView _bike;
        private JointMotor2D _wheelFrontMotor;
        private JointMotor2D _wheelRearMotor;
        private Coroutine _roofCoroutine;
        private float _maxDistance;
        private Status _status;
        private float _currentSpeedPercent;
        private float _maxSpeed;
        public BikeModel(GameObject prefab, Vector2 startupPosition)
        {
            _bike = Object.Instantiate(prefab, startupPosition, Quaternion.identity).GetComponent<BikeView>();
            CheckView(_bike);
            ReceiveMotorsFromWheels();
            _bike.roof.SetListener(this);
            SetSpeed(0f);
            Neutral();
        }

        public void Update()
        {
            _maxDistance = Mathf.Max(_maxDistance, _bike.transform.position.x);
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
            _bike.wheelJointFront.motor = _wheelFrontMotor;
            _bike.wheelJointRear.motor = _wheelRearMotor;
        }

        public void StartContactWithRoad(string tag)
        {
            if (_roofCoroutine != null)
            {
                return;
            }

            _roofCoroutine = _bike.StartCoroutine(BikeOnRoof());
        }

        public void StopContactWithRoad(string tag)
        {
            if (_roofCoroutine != null)
            {
                _bike.StopCoroutine(_roofCoroutine);
                _roofCoroutine = null;
            }
        }

        private IEnumerator BikeOnRoof()
        {
            yield return new WaitForSeconds(RespawnDelay);
            Respawn();
            _roofCoroutine = null;
        }

        private void Respawn()
        {
            var transform = _bike.transform;
            var position = transform.position;
            position = new Vector3(position.x, position.y + 2f);
            transform.position = position;
            transform.rotation = Quaternion.identity;
            _bike.Blink();
        }

        private void UseMotor(bool value)
        {
            _bike.wheelJointFront.useMotor = value;
            _bike.wheelJointRear.useMotor = value;
        }

        private float GetCurrentSpeedInPercent()
        {
            return (_bike.wheelRigidbodyRear.angularVelocity + _bike.wheelRigidbodyFront.angularVelocity) / 2f / _maxSpeed * -1f;
        }

        private void ReceiveMotorsFromWheels()
        {
            _wheelFrontMotor = _bike.wheelJointFront.motor;
            _wheelRearMotor = _bike.wheelJointRear.motor;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Interfaces;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IAttacker, IDamageable
    {
        [SerializeField] private Camera _camera;
        [field: SerializeField] public int Health { get; private set; }
        [SerializeField] private UIController _gameUI;
        [SerializeField] private GameObject[] _weapons;
        [SerializeField] private GameObject _weaponSlot;

        public int Score { get; set; }
        
        private CharacterController _controller;
        private GameObject _currentWeapon;
        private int _damage = 0;
        private float _fireRate = 0;
        private RaycastHit _hit;
        private float _lastHitTime = 0;

        private const float _speedScale = 5f,
            _jumpForce = 8f,
            _turnSpeed = 90f,
            _gravityScale = 15f;
    
        private float _verticalSpeed = 0f,
            _mouseX = 0f,
            _mouseY = 0f,
            _currentAngleX = 0f;

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            SetWeapon(0);
        }

        private void Update()
        {
            MoveCharacter();
            RotateCharacter();
            
            if (Input.GetKeyDown("1"))
                SetWeapon(0);

            if (Input.GetKeyDown("2"))
                SetWeapon(1);

            if (Input.GetMouseButton(0))
                Attack();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Heart"))
            {
                Health += 5;
                Destroy(collision.gameObject);
            }
        }

        private void RotateCharacter()
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");
            transform.Rotate(new Vector3(0f, _mouseX * _turnSpeed * Time.deltaTime, 0f));
            _currentAngleX += _mouseY * _turnSpeed * Time.deltaTime * -1f;
            _currentAngleX = Mathf.Clamp(_currentAngleX, -90f, 90f);
            _camera.transform.localEulerAngles = new Vector3(_currentAngleX, 0f, 0f);
        }

        private void MoveCharacter()
        {
            var velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            velocity = transform.TransformDirection(velocity) * _speedScale;
            if (_controller.isGrounded)
            {
                _verticalSpeed = 0f;
                if (Input.GetButton("Jump"))
                    _verticalSpeed = _jumpForce;
            }

            _verticalSpeed -= _gravityScale * Time.deltaTime;
            velocity.y = _verticalSpeed;
            _controller.Move(velocity * Time.deltaTime);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 1)
            {
                _gameUI.Death();
            }
        }

        private void SetWeapon(int number)
        {
            foreach (Transform weapon in _weaponSlot.transform)
                Destroy(weapon.gameObject);

            _currentWeapon = Instantiate(_weapons[number], _weaponSlot.transform, false);
            var currentWeapon = _currentWeapon.GetComponent<Weapon.Weapon>();
            _damage = currentWeapon.Damage;
            _fireRate = currentWeapon.FireRate;
        }
        
        public void Attack()
        {
            if (Time.time - _lastHitTime > _fireRate)
            {
                Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _hit);
                try
                {
                    var hit = _hit.transform.GetComponent<EnemyController>();
                    hit.TakeDamage(_damage);
                    _lastHitTime = Time.time;
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }

    }
}

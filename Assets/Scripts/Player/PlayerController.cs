using Interfaces;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IAttacker, IDamageable
    {
        [SerializeField] private Camera _camera;
        [field: SerializeField] public int Health { get; private set; }
        [SerializeField] private int _damage;
        [SerializeField] private UIController _gameUI;

        private CharacterController _controller;

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
        
        }
    
        private void Update()
        {
            MoveCharacter();
            RotateCharacter();
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

        public void Attack()
        {
            
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 1)
            {
                _gameUI.Death();
                Time.timeScale = 0;
            }
        }
    }
}

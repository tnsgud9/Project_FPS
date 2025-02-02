using Collections;
using UnityEngine;

namespace Units.Entities.Player
{
    [RequireComponent(typeof(IController))]
    public class PlayerMovement : BaseBehaviour, IMovement
    {
        private void Update()
        {
            Movement();
            Gravity();
        }

        private void OnDrawGizmos()
        {
            // Gizmos.color = CheckGround() ? Color.green : Color.red;
            // Gizmos.DrawWireSphere(transform.position, _characterController.radius);
        }


        public float CurrentSpeed => CalculateSpeed(); // Calculate Speed Logic


        public void Move(Vector2 hvDirection)
        {
            _inputDirection = hvDirection;
        }

        private void Movement()
        {
            if (_inputDirection == Vector2.zero) return;

            var moveDir = (_camera.CameraTransform.forward * _inputDirection.y +
                           _camera.CameraTransform.right * _inputDirection.x).normalized;
            moveDir.y = 0; // 수직 이동 방지

            _characterController.Move(moveDir * (CurrentSpeed * Time.deltaTime));
        }

        private void Gravity()
        {
            Debug.Log($"{_characterController.isGrounded} : {_velocity}");
            if (CheckGround() && _velocity.y < 0) _velocity.y = -2f; // 작은 값으로 지면에 붙도록 설정
            _velocity += Physics.gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        # region SubFunctions

        private bool CheckGround()
        {
            return Physics.CheckSphere(transform.position, _characterController.radius, groundLayer);
        }

        private float CalculateSpeed()
        {
            return Sprint ? DefaultSpeed * SprintMultiplier : DefaultSpeed;
        }

        #endregion

        #region Components

        [Inject] private ICamera _camera;
        [Inject] private CharacterController _characterController;

        #endregion

        # region Fields

        private Vector2 _inputDirection;
        private Vector3 _velocity;

        [SerializeField] private LayerMask groundLayer;

        [field: SerializeField] public bool IsGrounded { get; private set; }
        [field: SerializeField] public float DefaultSpeed { get; set; } = 3f;
        [field: SerializeField] public bool Sprint { get; set; }
        [field: SerializeField] public float SprintMultiplier { get; set; } = 2f;

        #endregion
    }
}
using Collections;
using UnityEditor;
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


        public float CurrentSpeed => CalculateSpeed(); // Calculate Speed Logic


        public void Move(Vector2 hvDirection)
        {
            _inputDirection = hvDirection;
        }

        private void Movement()
        {
            if (IsGrounded && _inputDirection == Vector2.zero) return;

            var moveDir = (_camera.CameraTransform.forward * _inputDirection.y +
                           _camera.CameraTransform.right * _inputDirection.x).normalized;

            _characterController.Move(moveDir * (CurrentSpeed * Time.deltaTime));
        }

        private void Gravity()
        {
            if (IsGrounded)
            {
                _velocity = Vector3.zero;
                return;
            }

            _velocity += gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        # region SubFunctions

        private bool CheckGround()
        {
            return Utilities.Physics.CylinderCast(transform.position, GroundRadius, Vector3.down,
                out _, _characterController.skinWidth);
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.down, GroundRadius);
            _characterController = GetComponent<CharacterController>();
            Handles.DrawWireDisc(transform.position + Vector3.down * _characterController.skinWidth, Vector3.down,
                GroundRadius);

            if (Utilities.Physics.CylinderCast(transform.position, GroundRadius, Vector3.down,
                    out var hit, _characterController.skinWidth))
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position + Vector3.down * hit.distance, Vector3.down, GroundRadius);
            }
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
        [SerializeField] private Vector3 gravity = Physics.gravity;

        public bool IsGrounded => CheckGround();

        [field: SerializeField] public float DefaultSpeed { get; set; } = 3f;
        [field: SerializeField] public float GroundRadius { get; set; } = 0.5f;
        [field: SerializeField] public bool Sprint { get; set; }
        [field: SerializeField] public float SprintMultiplier { get; set; } = 2f;

        #endregion
    }
}
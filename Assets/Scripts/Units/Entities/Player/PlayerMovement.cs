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

            // 카메라의 forward와 right 벡터에서 y축 성분을 제외하고 x, z 방향만 고려
            var forward = _camera.CameraTransform.forward;
            var right = _camera.CameraTransform.right;

            // y축 성분을 0으로 설정하여 수평 방향만 사용
            forward.y = 0;
            right.y = 0;

            // 벡터 정규화
            forward.Normalize();
            right.Normalize();

            // 이동 방향 계산 (x, z 방향만 고려)
            var moveDir = forward * _inputDirection.y + right * _inputDirection.x;

            // 이동 방향에 따른 이동
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
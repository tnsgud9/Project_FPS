using Collections;
using UnityEngine;

namespace Units.Entities.Player
{
    [RequireComponent(typeof(IController))]
    public class PlayerMovement : BaseBehaviour, IMovement
    {
        [Inject] private ICamera _camera;
        [Inject] private CharacterController _characterController;
        private Vector2 _inputDirection;

        private void Update()
        {
            if (_inputDirection == Vector2.zero) return;

            var moveDir = (_camera.CameraTransform.forward * _inputDirection.y +
                           _camera.CameraTransform.right * _inputDirection.x).normalized;
            moveDir.y = 0; // 수직 이동 방지

            _characterController.Move(moveDir * (Speed * Time.deltaTime));
        }

        public void Move(Vector2 hvDirection)
        {
            _inputDirection = hvDirection;
        }

        [field: SerializeField] public float Speed { get; set; } = 5f;
    }
}
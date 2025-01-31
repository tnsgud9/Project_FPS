using Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units.Entities.Player
{
    public class PlayerController : BaseBehaviour, IController
    {
        [field: SerializeField] public bool IsPlayable { get; private set; }
        [Inject] [CanBeNull] private ICamera _camera;
        [Inject] [CanBeNull] private IMovement _movement;
        [Inject] private PlayerInput _playerInput;

        private void Start()
        {
            PlayerInputActions playerInputActions = new();
            _playerInput.actions = playerInputActions.asset;

            playerInputActions.Player.Move.started += OnMoveStarted;
            playerInputActions.Player.Move.performed += OnMovePerformed;
            playerInputActions.Player.Move.canceled += OnMoveCanceled;

            playerInputActions.Player.Look.performed += context =>
            {
                _camera.RotateView(context.ReadValue<Vector2>());
            };
        }

        // 입력이 시작되었을 때 호출
        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            // Move 입력이 시작되었을 때 (예: W, A, S, D)
            _movement?.Move(context.ReadValue<Vector2>());
        }

        // 입력이 계속 유지되는 동안 호출
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            // 계속해서 누르고 있는 동안 이동
            _movement?.Move(context.ReadValue<Vector2>());
        }

        // 입력이 취소되었을 때 호출
        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            // 버튼을 떼면 멈추기
            _movement?.Move(Vector2.zero);
        }
    }
}
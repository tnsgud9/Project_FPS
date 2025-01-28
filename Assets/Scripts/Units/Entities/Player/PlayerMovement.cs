using Collections;
using UnityEngine;

namespace Units.Entities.Player
{
    [RequireComponent(typeof(IController))]
    public class PlayerMovement : BaseBehaviour, IMovement
    {
        [Inject] private CharacterController _characterController;

        private Vector3 _movementDirection = Vector3.zero;

        private void Update()
        {
            _characterController.Move(new Vector3(_movementDirection.y, 0, _movementDirection.x) *
                                      (Speed * Time.deltaTime));
        }

        [field: SerializeField] public float Speed { get; set; }


        public void Move(Vector2 hvDirection)
        {
            _movementDirection = hvDirection;
        }
    }
}
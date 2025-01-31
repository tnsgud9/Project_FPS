using Collections;
using UnityEngine;

namespace Units.Entities.Player
{
    [RequireComponent(typeof(IController))]
    public class PlayerCamera : BaseBehaviour, ICamera
    {
        [SerializeField] private float mouseSensitivity = 100f;

        private float _xRotation;

        public void RotateView(Vector2 input)
        {
            var mouseX = input.x;
            var mouseY = input.y;

            // X축 회전값을 제한
            _xRotation -= mouseY * mouseSensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            // 카메라의 X축 회전 (위아래 회전)
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            // 카메라의 Y축 회전 (좌우 회전)
            transform.Rotate(Vector3.up * mouseX * mouseSensitivity);
        }
    }
}
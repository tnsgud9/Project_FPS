using Collections;
using UnityEngine;

namespace Units.Entities.Player
{
    [RequireComponent(typeof(IController))]
    public class PlayerCamera : BaseBehaviour, ICamera
    {
        [SerializeField] private float mouseSensitivity = 10f;

        private float _xRotation;
        [field: SerializeField] public Transform CameraTransform { get; set; }


        public void RotateView(Vector2 input)
        {
            var mouseX = input.x * mouseSensitivity * Time.deltaTime;
            var mouseY = input.y * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -89.99f, 89.99f); // 상하 회전 제한

            CameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f); // 카메라 상하 회전
            transform.Rotate(Vector3.up * mouseX); // 플레이어 좌우 회전
        }
    }
}
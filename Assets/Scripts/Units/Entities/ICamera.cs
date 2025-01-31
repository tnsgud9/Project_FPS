using UnityEngine;

namespace Units.Entities
{
    public interface ICamera
    {
        public Transform CameraTransform { get; set; }
        public void RotateView(Vector2 input);
    }
}
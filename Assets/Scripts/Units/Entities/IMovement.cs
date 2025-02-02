using UnityEngine;

namespace Units.Entities
{
    public interface IMovement
    {
        public float CurrentSpeed { get; }
        public float DefaultSpeed { get; set; }
        public bool Sprint { get; set; }
        public float SprintMultiplier { get; set; }
        public bool IsGrounded { get; }
        public void Move(Vector2 hvDirection);
    }
}
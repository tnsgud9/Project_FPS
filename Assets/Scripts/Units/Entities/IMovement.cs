using UnityEngine;

namespace Units.Entities
{
    public interface IMovement
    {
        public float Speed { get; set; }
        public void Move(Vector2 hvDirection);
    }
}
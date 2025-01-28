using Collections;
using UnityEngine;

namespace Units.Entities.Player
{
    [RequireComponent(typeof(IController))]
    public class PlayerMovement : BaseBehaviour, IMovement
    {
    }
}
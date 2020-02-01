using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Movement
{
    public interface IMovement
    {
        Vector3 Velocity { get; }
        void Move();
    }
}
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    public interface IDirectionalInput : IInput
    {
        Quaternion LookRotation { get; }
        
        float Vertical { get; }
        float Horizontal { get; }
    }
}
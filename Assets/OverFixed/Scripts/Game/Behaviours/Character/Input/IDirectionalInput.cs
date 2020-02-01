using System;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    public interface IDirectionalInput : IInput
    {
        event Action OnChanged;

        Quaternion LookRotation { get; }
        
        float Vertical { get; }
        float Horizontal { get; }
    }
}
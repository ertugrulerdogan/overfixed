using UnityEngine;

namespace OverFixed.Scripts.Game.Controllers.Input
{
    public interface IDirectionalInput
    {
        Quaternion LookRotation { get; }
        
        float Vertical { get; }
        float Horizontal { get; }
    }
}
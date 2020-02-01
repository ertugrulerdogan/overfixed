using System;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    public interface IInteractionInput : IInput
    {
        event Action OnPicked;
        event Action OnUseStarted;
        event Action OnUseEnded;
    }
}
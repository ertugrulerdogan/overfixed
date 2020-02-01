using System;

namespace OverFixed.Scripts.Game.Behaviours.Character.Input
{
    public interface IInteractionInput : IInput
    {
        event Action OnPick;
        event Action OnUse;
    }
}
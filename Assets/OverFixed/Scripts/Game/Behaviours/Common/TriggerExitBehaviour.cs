using System;
using UnityEngine;
using UnityEngine.Events;

namespace OverFixed.Scripts.Game.Behaviours.Common
{
    [Serializable] public class TriggerEvent : UnityEvent<Collider>{}
    
    public class TriggerExitBehaviour : MonoBehaviour
    {
        [SerializeField] private TriggerEvent _onExit;
        
        private void OnTriggerExit(Collider other)
        {
            _onExit?.Invoke(other);
        }
    }
}
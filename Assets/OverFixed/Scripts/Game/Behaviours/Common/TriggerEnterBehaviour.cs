using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Common
{
    public class TriggerEnterBehaviour : MonoBehaviour
    {
        [SerializeField] private TriggerEvent _onEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            _onEnter?.Invoke(other);
        }
    }
}
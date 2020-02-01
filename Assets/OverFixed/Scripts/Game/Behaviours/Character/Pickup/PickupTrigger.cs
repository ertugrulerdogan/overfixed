using OverFixed.Scripts.Game.Behaviours.Pickupables;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Pickup
{
    public class PickupTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<IPickupable>()?.Pickup();
        }
    }
}

using UnityEngine;

namespace OverFixed.Scripts.Game.Controllers.Input
{
    public class DefaultDirectionalInput : MonoBehaviour, IDirectionalInput
    {
        public float Vertical { get; private set; }
        public float Horizontal { get; private set; }

        public void Update()
        {
            Vertical = UnityEngine.Input.GetAxis("Vertical");
            Horizontal = UnityEngine.Input.GetAxis("Horizontal");
        }
    }
}
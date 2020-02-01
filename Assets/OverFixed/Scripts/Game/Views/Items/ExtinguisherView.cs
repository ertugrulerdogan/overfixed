using OverFixed.Scripts.Game.Models.Items;
using UnityEngine;

namespace OverFixed.Scripts.Game.Views.Items
{
    public class ExtinguisherView : ItemView<Extinguisher>
    {
        [SerializeField] private ParticleSystem _particles;
        
        private void Update()
        {
            _particles.gameObject.SetActive(Item.Using);
        }
    }
}
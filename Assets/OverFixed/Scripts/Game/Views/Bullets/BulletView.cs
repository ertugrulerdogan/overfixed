using System;
using OverFixed.Scripts.Game.Behaviours.Bullets;
using UnityEngine;

namespace OverFixed.Scripts.Game.Views.Bullets
{
    public class BulletView : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<BulletBehaviour>().OnHit += OnHit;
        }

        private void OnEnable()
        {
            
        }

        private void OnHit(Vector3 position, Vector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
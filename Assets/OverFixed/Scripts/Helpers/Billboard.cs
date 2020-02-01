using System;
using UnityEngine;

namespace OverFixed.Scripts.Helpers
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraTransform;
        
        private void Start()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = _cameraTransform.rotation;
        }
    }
}
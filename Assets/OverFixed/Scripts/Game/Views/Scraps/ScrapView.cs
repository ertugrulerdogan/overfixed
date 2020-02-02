using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OverFixed.Scripts.Game.Views.Scraps
{
    public class ScrapView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _models;

        private void OnEnable()
        {
            foreach (var model in _models)
            {
                model.SetActive(false);
            }
            
            _models[Random.Range(0, _models.Length)].SetActive(true);
        }
    }
}
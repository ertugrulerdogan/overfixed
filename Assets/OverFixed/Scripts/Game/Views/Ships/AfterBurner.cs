using UnityEngine;

namespace OverFixed.Scripts.Game.Views.Ships
{
    [RequireComponent(typeof (ParticleSystem))]
    public class AfterBurner : MonoBehaviour
    {
        public Color minColour; 
        private ParticleSystem _ps; 
        private float _startSize; 
        private float _lifeTime; 
        private Color _color; 
        private float _thrustAmount;

        private void Start()
        {
            _ps = GetComponent<ParticleSystem>();
            _lifeTime = _ps.main.startLifetime.constant;
            _startSize = _ps.main.startSize.constant;
            _color = _ps.main.startColor.color;
        }

        private void Update()
        {
			var mainModule = _ps.main;
			mainModule.startLifetime = Mathf.Lerp(0.0f, _lifeTime, _thrustAmount);
			mainModule.startSize = Mathf.Lerp(_startSize*.3f, _startSize, _thrustAmount);
			mainModule.startColor = Color.Lerp(minColour, _color, _thrustAmount);
        }

        public void SetThrustAmount(float amount)
        {
            _thrustAmount = amount;
        }
    }
}

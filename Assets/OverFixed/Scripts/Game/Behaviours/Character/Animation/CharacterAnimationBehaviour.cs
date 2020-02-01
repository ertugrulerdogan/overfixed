using OverFixed.Scripts.Game.Behaviours.Character.Movement;
using UnityEngine;

namespace OverFixed.Scripts.Game.Behaviours.Character.Animation
{
    public static class CharacterAnimationHashes
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
    }
    
    [RequireComponent(typeof(IMovement))]
    public class CharacterAnimationBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IMovement _movement;
        private IMovement Movement
        {
            get
            {
                if (_movement == null) _movement = GetComponent<IMovement>();
                return _movement;
            }
        }

        private void Update()
        {
            _animator.SetFloat(CharacterAnimationHashes.Speed, Movement.Velocity.magnitude);
        }

        #region Event Listeners

        public void OnItemPickUp()
        {
            
        }

        public void OnItemUsing()
        {
            
        }

        #endregion        
    }
}
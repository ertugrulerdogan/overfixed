using System;
using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Character.Movement;
using OverFixed.Scripts.Game.Behaviours.Items;
using UnityEngine;
using Zenject;

namespace OverFixed.Scripts.Game.Behaviours.Character.Animation
{
    //Wrench = 0, Extinguisher = 1, Rifle = 2, Cutter = 3
    public static class CharacterAnimationHashes
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int ItemIndex = Animator.StringToHash("ItemIndex");
        public static readonly int Pick = Animator.StringToHash("Pick");
        public static readonly int Drop = Animator.StringToHash("Drop");
        public static readonly int UseBegin = Animator.StringToHash("UseBegin");
        public static readonly int UseEnd = Animator.StringToHash("UseEnd");
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

        private int GetItemIndex(Type itemType)
        {
            if (itemType == typeof(WrenchBehaviour)) return 0;
            else if (itemType == typeof(ExtinguisherBehaviour)) return 1;
            else if (itemType == typeof(RifleBehaviour)) return 2;
            else if (itemType == typeof(CutterBehaviour)) return 3;
            else return -1;
        }
        
        #region Event Listeners

        public void OnItemPickUp(Type itemType)
        {
            _animator.SetFloat(CharacterAnimationHashes.ItemIndex, GetItemIndex(itemType));
            _animator.SetTrigger(CharacterAnimationHashes.Pick);
        }

        public void OnItemDrop(Type itemTypes)
        {
            _animator.SetTrigger(CharacterAnimationHashes.Drop);   
        }
        
        public void OnItemUseBegin(Type itemType)
        {
            _animator.SetTrigger(CharacterAnimationHashes.UseBegin);
        }

        public void OnItemUseEnd(Type itemType)
        {
            _animator.SetTrigger(CharacterAnimationHashes.UseEnd);            
        }

        #endregion
    }
}
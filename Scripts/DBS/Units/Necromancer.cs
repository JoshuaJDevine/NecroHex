using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Types = DBS.utils.Types;

namespace DBS.Units
{
    public class Necromancer : MonoBehaviour
    {
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public Animator animator;
        public Queue<Game.GridObject> objectsToCastTo = new();
        private void Awake()
        {
            animator = GetComponent<Animator>();
            Mana = 4;
            MaxMana = 4;
        }

        public void PlayAnimation(Types.NecromancerAnimations necromancerAnimation, UnityAction onAnimationEndEvent)
        {
            switch (necromancerAnimation)
            {
                case Types.NecromancerAnimations.Cast:
                    animator.SetTrigger("Cast");
                    onAnimationEndEvent.Invoke();
                    break;
            }
        }

        public void AnimationEnd()
        {
            if (objectsToCastTo.Count > 0)
            {
                while (objectsToCastTo.Count > 0)
                {
                    Game.GridObject newGridObjectToCastToo = objectsToCastTo.Dequeue();
                    newGridObjectToCastToo.Unit.CreateUnit(Types.Units.SkeletonWarrior, false);
                }
            }
        }
        
    }
}
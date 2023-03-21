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
        private Animator animator;
        public Queue<Game.GridObject> objectsToCastTo = new();
        public Image greenManaImage;
        public Sprite greenManaSprite;
        private void Awake()
        {
            greenManaImage.fillAmount = 1;
            animator = GetComponent<Animator>();
            Mana = 4;
            MaxMana = 4;
            greenManaImage.sprite = greenManaSprite;
            greenManaImage.color = Color.white;
        }

        public void PlayAnimation(Types.NecromancerAnimations necromancerAnimation, UnityAction onAnimationEndEvent)
        {
            switch (necromancerAnimation)
            {
                case Types.NecromancerAnimations.Cast:
                    animator.SetTrigger("Cast");
                    onAnimationEndEvent.Invoke();
                    
                    //Use Mana
                    Mana -= 1;
                    int manaPercentage = Mana * 100 / MaxMana;
                    greenManaImage.DOFillAmount(((float)manaPercentage / 100), animator.GetCurrentAnimatorClipInfo(0).Length+.1f).SetEase(Ease.Linear);
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
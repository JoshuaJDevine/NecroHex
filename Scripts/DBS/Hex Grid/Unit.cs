using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Types = DBS.Utilities.Types;

namespace DBS.HexGrid
{
    public class Unit : MonoBehaviour
    {
        public Animator animator;
        public List<AnimatorController> unitAnimators;
        public List<UnitStats> unitStatsList;
        public SpriteRenderer spriteRenderer;
        public UnitStats unitStats;
        private void Awake()
        {
            CreateUnit(Types.Units.None, false);
        }

        public void KillUnit()
        {
            animator.SetBool("Dead", true);
        }

        public void CreateUnit(Types.Units unitType, bool isEnemy)
        {
            animator.runtimeAnimatorController = unitAnimators[(int)unitType];
            animator.SetBool("Run", false);
            animator.SetTrigger("Appear");

            unitStats = unitStatsList[(int)unitType];

            if (isEnemy)
            {
                spriteRenderer.flipX = true;
            }
        }

        public void Attack()
        {
            animator.SetTrigger("Attack");
        }
        
        public void Run()
        {
            animator.SetBool("Run", true);
        }

        public void Idle()
        {
            animator.SetBool("Run", false);
        }
    }
}
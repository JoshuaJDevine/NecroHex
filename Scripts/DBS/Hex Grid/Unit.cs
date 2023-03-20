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

        private void Awake()
        {
            CreateUnit(Types.Units.None);
        }

        public void KillUnit()
        {
            animator.SetBool("Dead", true);
        }

        public void CreateUnit(Types.Units unitType)
        {
            animator.runtimeAnimatorController = unitAnimators[(int)unitType];
            animator.SetBool("Run", false);
            animator.SetTrigger("Appear");
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
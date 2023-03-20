using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

        public void AttackUnit(Vector2 unitsPosition)
        {
            animator.SetBool("Run", true);
        }

        [Button("ATTACK!")]
        public void Attack()
        {
            Game.GridObject unitsGridObject = Game.Instance.GetHex(new Vector2(1,0));
            Debug.Log("Found " + unitsGridObject.Unit.unitStats.name);
            if (unitsGridObject.Unit.unitStats.name != "None")
            {
                Debug.Log("Attack unit at " +  unitsGridObject.GridPosition);
                Debug.Log("The unit is " + unitsGridObject.Unit.unitStats.name);
                animator.SetBool("Run", true);
                
                Vector3 newPos = Game.Instance.GetHex(new Vector2(1,0), Types.HexDirections.E, 1).HexObject.transform.position;

                transform.DOMove(newPos, unitStats.movementSpeed).OnComplete(() =>
                {
                    unitsGridObject.Unit.animator.SetTrigger("Attack");
                    animator.SetTrigger("Attack");
                    animator.SetBool("Run", false);
                });
            }
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
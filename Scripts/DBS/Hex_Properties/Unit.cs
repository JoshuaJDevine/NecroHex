using System;
using System.Collections.Generic;
using DBS.Units;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;
using Types = DBS.utils.Types;

namespace DBS.HexProperties
{
    public class Unit : MonoBehaviour
    {
        public Game.GridObject gridObject;
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
            //TODO
        }

        public void CreateUnit(Types.Units unitType, bool isEnemy)
        {
            animator.runtimeAnimatorController = unitAnimators[(int)unitType];
            unitStats = unitStatsList[(int)unitType];

            if (unitType == Types.Units.None) return;
            
            PlayAnimation(Types.UnitAnimations.Idle);
            PlayAnimation(Types.UnitAnimations.Reborn);
            
            if (isEnemy)
            {
                Game.Instance.activeEnemyUnits.Add(this);
                spriteRenderer.flipX = true;
            }
            else
            {
                Game.Instance.activePlayerUnits.Add(this);
                spriteRenderer.flipX = false;
            }
            
        }

        [Button("Attack random player unit")]
        public void AttackRandomPlayerUnit()
        {
            if (Game.Instance.activePlayerUnits.Count < 1) return;

            Unit attackingUnit = this;
            Unit defendingUnit = Game.Instance.activePlayerUnits[Random.Range (0, Game.Instance.activePlayerUnits.Count)];
            

            attackingUnit.PlayAnimation(Types.UnitAnimations.Run);
            
            Vector3 defndingUnitPosPlusOne = Game.Instance.GetHex(Game.Instance.PlayerBoard, defendingUnit.gridObject.GridPosition, Types.HexDirections.E, 1).HexObject.transform.position;

            transform.DOMove(defndingUnitPosPlusOne, unitStats.movementSpeed).OnComplete(() =>
            {
                defendingUnit.PlayAnimation(Types.UnitAnimations.Attack);
                attackingUnit.PlayAnimation(Types.UnitAnimations.Attack);
                attackingUnit.PlayAnimation(Types.UnitAnimations.Idle);
            });
            
        }
        
        public void PlayAnimation(Types.UnitAnimations unitAnimation)
        {
            switch (unitAnimation)
            {
                case Types.UnitAnimations.Attack:
                    animator.SetTrigger("Attack");
                    break;
                case Types.UnitAnimations.Reborn:
                    animator.SetTrigger("Reborn");
                    break;
                case Types.UnitAnimations.Run:
                    animator.SetBool("Run", true);
                    break;
                case Types.UnitAnimations.Idle:
                    animator.SetBool("Run", false);
                    break;
            }
        }
    }
}
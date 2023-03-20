using System;
using System.Collections.Generic;
using DBS.Units;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Types = DBS.utils.Types;

namespace DBS.HexProperties
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
            //TODO
        }

        public void CreateUnit(Types.Units unitType, bool isEnemy)
        {
            animator.runtimeAnimatorController = unitAnimators[(int)unitType];
            PlayAnimation(Types.UnitAnimations.Idle);
            PlayAnimation(Types.UnitAnimations.Reborn);

            unitStats = unitStatsList[(int)unitType];

            if (isEnemy)
            {
                spriteRenderer.flipX = true;
            }
        }

        [Button("ATTACK!")]
        public void EnemyAttackPlayer()
        {
            Game.GridObject unitsGridObject = Game.Instance.GetHex(Game.Instance.PlayerBoard, new Vector2(1,0));
            Debug.Log("Found " + unitsGridObject.Unit.unitStats.name);
            if (unitsGridObject.Unit.unitStats.name != "None")
            {
                PlayAnimation(Types.UnitAnimations.Run);
                
                Vector3 newPos = Game.Instance.GetHex(Game.Instance.PlayerBoard, new Vector2(1,0), Types.HexDirections.E, 1).HexObject.transform.position;

                transform.DOMove(newPos, unitStats.movementSpeed).OnComplete(() =>
                {
                    unitsGridObject.Unit.PlayAnimation(Types.UnitAnimations.Attack);
                    PlayAnimation(Types.UnitAnimations.Attack);
                    PlayAnimation(Types.UnitAnimations.Idle);
                });
            }
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
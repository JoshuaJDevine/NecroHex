using UnityEditor;
using UnityEngine;

namespace DBS
{
    [CreateAssetMenu(menuName = "Necromerge/Unit Stats")]
    public class UnitStats : ScriptableObject
    {
        public float attackSpeedMin;
        public float attackSpeedMax;
        public float movementSpeed;
        public float attackSpeed;
        public int hitPoints;
        public int attackDamage;
        public int threatLevel;
    }
}
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Types = DBS.utils.Types;

namespace DBS.Cauldron
{
    public class Cauldron : MonoBehaviour
    {
        public Types.ManaColors currentAvailableColor;
        public SpriteRenderer spriteRenderer;

        private void Awake()
        {
            SetCauldronColor((Types.ManaColors)Random.Range(0, System.Enum.GetValues(typeof(Types.ManaColors)).Length));
        }

        [Button("CREATE MANA")]
        public void CreateMana(Types.ManaColors manaColor, Game.GridObject hexToCreateManaOn)
        {
            hexToCreateManaOn.Mana.SetManaColor(manaColor);
        }

        public void SetCauldronColor(Types.ManaColors manaColor)
        {
            currentAvailableColor = manaColor;
            switch (manaColor)
            {
                case Types.ManaColors.Blue:
                    spriteRenderer.color = Color.blue;
                    break;
                case Types.ManaColors.Green:
                    spriteRenderer.color = Color.green;
                    break;
                case Types.ManaColors.Purple:
                    spriteRenderer.color = Color.magenta;
                    break;
                case Types.ManaColors.Red:
                    spriteRenderer.color = Color.red;
                    break;
                case Types.ManaColors.Yellow:
                    spriteRenderer.color = Color.yellow;
                    break;
            }
        }
    }
}
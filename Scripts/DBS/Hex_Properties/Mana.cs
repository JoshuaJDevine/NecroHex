using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Types = DBS.utils.Types;

namespace DBS.Hex_Properties
{
    public class Mana : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public List<Sprite> manaSprites;
        public Types.ManaColors manaColor;
        public int manaEnergy;
        private void Awake()
        {
            RadnomizeColor();
        }

        private void RadnomizeColor()
        {
            manaColor = (Types.ManaColors)Random.Range(0, System.Enum.GetValues(typeof(Types.ManaColors)).Length);
            SetManaColor(manaColor);
        }

        public void DisableMana()
        {
            manaEnergy = 0;
            spriteRenderer.enabled = false;
        }

        public void SetManaColor(Types.ManaColors newColor)
        {
            manaEnergy = 1;
            spriteRenderer.enabled = true;

            manaColor = newColor;
            switch (newColor)
            {
                case Types.ManaColors.Red:
                    spriteRenderer.sprite = manaSprites[0];
                    break;
                case Types.ManaColors.Yellow:
                    spriteRenderer.sprite = manaSprites[1];
                    break;
                case Types.ManaColors.Blue:
                    spriteRenderer.sprite = manaSprites[2];
                    break;
                case Types.ManaColors.Green:
                    spriteRenderer.sprite = manaSprites[3];
                    break;
                case Types.ManaColors.Purple:
                    spriteRenderer.sprite = manaSprites[4];
                    break;
            }
        } 
    }
}


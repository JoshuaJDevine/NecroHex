using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Types = DBS.Utilities.Types;

namespace DBS.HexGrid
{
    public class Mana : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public List<Sprite> manaSprites;
        public Types.ManaColors manaColor;
        private void Awake()
        {
            RadnomizeColor();
        }

        private void RadnomizeColor()
        {
            manaColor = (Types.ManaColors)Random.Range(0, System.Enum.GetValues(typeof(Types.ManaColors)).Length);
            SetManaColor(manaColor);
        }

        public void SetManaColor(Types.ManaColors newColor)
        {
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


using System;
using System.Collections.Generic;
using UnityEngine;

namespace DBS.HexGrid
{
    public class Unit : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public List<Sprite> unitSprites;

        private void Awake()
        {
            KillUnit();
        }

        public void KillUnit()
        {
            spriteRenderer.enabled = false;
        }

        public void CreateUnit(int unitNumber)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = unitSprites[unitNumber];
        }
    }
}
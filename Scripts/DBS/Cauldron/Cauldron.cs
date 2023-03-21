using Sirenix.OdinInspector;
using UnityEngine;
using Types = DBS.utils.Types;

namespace DBS.Cauldron
{
    public class Cauldron : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        [Button("CREATE MANA")]
        public void CreateMana()
        {
            Game.GridObject gridObject = Game.Instance.GetHex(Game.Instance.PlayerBoard, new Vector2(
                Random.Range(0,Game.Instance.PlayerBoard.GetWidth()), 
                Random.Range(0,Game.Instance.PlayerBoard.GetHeight())));
            
            Types.ManaColors newColor = gridObject.Mana.RadnomizeColor();

            switch (newColor)
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
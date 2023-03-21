using Sirenix.OdinInspector;
using UnityEngine;

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
            
            gridObject.Mana.RadnomizeColor();
        }
    }
}
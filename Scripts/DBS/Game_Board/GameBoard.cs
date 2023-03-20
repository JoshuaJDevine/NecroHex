using DBS.utils;
using UnityEngine;
using Types = DBS.utils.Types;

namespace DBS
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private Transform pfHex;
        private GridHexXZ<Game.GridObject> gridHexXZ;

        public int width = 2;
        public int height = 2;

        public bool isPlaceholderEnemyBoard;
        private void Awake()
        {
            //CREATE GRID
            gridHexXZ = 
                new GridHexXZ<Game.GridObject>(width, height, Constants.CellSize, gameObject.transform.position, (GridHexXZ<Game.GridObject> g, int x, int y) => new Game.GridObject());

            for (int x = 0; x < width; x++) {
                for (int z = 0; z < height; z++) {
                    Transform visualTransform = Instantiate(pfHex, gridHexXZ.GetWorldPosition(x, z), Quaternion.identity);
                    Game.GridObject newGridObj = gridHexXZ.GetGridObject(x, z);
                    newGridObj.HexObject = visualTransform;
                    newGridObj.Mana = newGridObj.HexObject.gameObject.GetComponent<Hex_Properties.Mana>();
                    newGridObj.Unit = newGridObj.HexObject.gameObject.GetComponent<DBS.HexProperties.Unit>();
                    newGridObj.GridPosition = new Vector2(x, z);
                    newGridObj.Deselect();

                    //Placeholder to create a few enemies for testing
                    if (isPlaceholderEnemyBoard)
                    {
                        newGridObj.Mana.DisableMana();
                        newGridObj.Unit.CreateUnit(Types.Units.HumanFootman, true);
                    }
                }
            }

            if (!isPlaceholderEnemyBoard)
            {
                Game.Instance.PlayerBoard = gridHexXZ;
            }
            else
            {
                Game.Instance.EnemyBoard = gridHexXZ;
            }
        }
    }
}
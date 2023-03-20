using UnityEngine;
using Types = DBS.Utilities.Types;

namespace DBS
{
    public class EnemyBoard : MonoBehaviour
    {
        [SerializeField] private Transform pfHex;
        private GridHexXZ<Game.GridObject> gridHexXZ;

        private void Awake()
        {
            //CREATE GRID
            int width = 2;
            int height = 2;
            float cellSize = 1f;
            gridHexXZ = 
                new GridHexXZ<Game.GridObject>(width, height, cellSize, gameObject.transform.position, (GridHexXZ<Game.GridObject> g, int x, int y) => new Game.GridObject());

            for (int x = 0; x < width; x++) {
                for (int z = 0; z < height; z++) {
                    Transform visualTransform = Instantiate(pfHex, gridHexXZ.GetWorldPosition(x, z), Quaternion.identity);
                    Game.GridObject newGridObj = gridHexXZ.GetGridObject(x, z);
                    newGridObj.HexObject = visualTransform;
                    newGridObj.Mana = newGridObj.HexObject.gameObject.GetComponent<DBS.HexGrid.Mana>();
                    newGridObj.Unit = newGridObj.HexObject.gameObject.GetComponent<DBS.HexGrid.Unit>();
                    newGridObj.GridPosition = new Vector2(x, z);
                    newGridObj.Deselect();
                    
                    newGridObj.Mana.DisableMana();
                    newGridObj.Unit.CreateUnit(Types.Units.HumanFootman, true);
                }
            }
        }
    }
}
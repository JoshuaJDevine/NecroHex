/*
 * Notes:
 * Upgrade Tree:
 * --- Skill tree idea: Mana Control
 * --- Initially you can swap mana and they move back if no match
 * --- Swap a mana up to two positions
 * --- Swap a mana up to three positions
 * --- Place your own trio of mana (random)
 * --- Place your own duo of mana (random)
 * --- Place your own mana (random)
 * --- Place your own mana (choose color)
 * --- Move a mana anywhere you want
 *
 * --- Matches = units
 * --- Matching units = upgrade
 *
 * --- Skill tree idea: Unit control
 * --- Choose the upgrade path instead of it being random
 *
 * --- Add Challenge by hiding (fog of war) parts of the grid
 * --- Add Challenfe by making the grid larger and smaller
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Types = DBS.Utilities.Types;

namespace DBS
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Transform pfHex;

        private GridHexXZ<GridObject> gridHexXZ;
        
        //Setup Singleton Pattern
        private static Game _instance;
        public static Game Instance => _instance;
        
        //Variables
        [ShowInInspector] private GridObject selectedHex;
        [ShowInInspector] private GridObject swappedHex;
        [ShowInInspector] private Types.PlayerAction playersCurrentAction;
        [ShowInInspector] private List<GridObject> availableHexes = new();
        private void Awake()
        {
            //Manage Singleton Pattern
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
            
            //CREATE GRID
            int width = 8;
            int height = 8;
            float cellSize = 1f;
            gridHexXZ = 
                new GridHexXZ<GridObject>(width, height, cellSize, Vector3.zero, (GridHexXZ<GridObject> g, int x, int y) => new GridObject());

            for (int x = 0; x < width; x++) {
                for (int z = 0; z < height; z++) {
                    Transform visualTransform = Instantiate(pfHex, gridHexXZ.GetWorldPosition(x, z), Quaternion.identity);
                    GridObject newGridObj = gridHexXZ.GetGridObject(x, z);
                    newGridObj.HexObject = visualTransform;
                    newGridObj.Mana = newGridObj.HexObject.gameObject.GetComponent<DBS.HexGrid.Mana>();
                    newGridObj.GridPosition = new Vector2(x, z);
                    newGridObj.Deselect();
                }
            }

            selectedHex = null;
            swappedHex = null;
            playersCurrentAction = Types.PlayerAction.Idle;
        }
        private void Update() {
            if (Input.GetMouseButtonDown(0))
            {
                GridObject newSelectedHex = gridHexXZ.GetGridObject(Mouse3D.GetMouseWorldPosition());

               if (newSelectedHex == selectedHex)
               {
                   selectedHex.Deselect();
                   return;
               }

               if (selectedHex != null) {
                   selectedHex.Deselect();
               }

               selectedHex = newSelectedHex;
               selectedHex.Select();
            }

            if (Input.GetMouseButton(0))
            {
                playersCurrentAction = Types.PlayerAction.Dragging;
            }

            
            if (Input.GetMouseButtonUp(0))
            {
                if (playersCurrentAction == Types.PlayerAction.Dragging)
                {
                    if (Input.GetMouseButtonUp(0) && selectedHex != null)
                    {
                        GridObject newSelectedHex = gridHexXZ.GetGridObject(Mouse3D.GetMouseWorldPosition());
                        if (newSelectedHex != selectedHex)
                        {
                            swappedHex = newSelectedHex;
                            SwapHexColors();
                        }
                    }
                }
                
                playersCurrentAction = Types.PlayerAction.Idle;
            }
        }

        public void SwapHexColors()
        {
            if (selectedHex != null && swappedHex != null)
            {
                Types.ManaColors selectedManaColor = selectedHex.Mana.manaColor;
                selectedHex.Mana.SetManaColor(swappedHex.Mana.manaColor);
                swappedHex.Mana.SetManaColor(selectedManaColor);
                
                selectedHex.Deselect();
                swappedHex.Deselect();

                FindMatches(swappedHex.GridPosition, 2);

                selectedHex = null;
                swappedHex = null;
            }

        }

        public void FindMatches(Vector2 startPosition, int maxDistance)
        {                    
            GridObject startingGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.y));

            availableHexes.Clear();
            
            GridObject swGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x-1), Mathf.RoundToInt(startPosition.y-1));
            if (swGridObject != null)
            {
                availableHexes.Add(swGridObject);
            }
            
            GridObject wGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x-1), Mathf.RoundToInt(startPosition.y));
            if (wGridObject != null)
            {
                availableHexes.Add(wGridObject);
            }
            
            GridObject nwGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x-1), Mathf.RoundToInt(startPosition.y+1));
            if (nwGridObject != null)
            {
                availableHexes.Add(nwGridObject);
            }
            
            GridObject neGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.y+1));
            if (neGridObject != null)
            {
                availableHexes.Add(neGridObject);
            }
            
            GridObject eGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x+1), Mathf.RoundToInt(startPosition.y));
            if (eGridObject != null)
            {
                availableHexes.Add(eGridObject);
            }
            
            GridObject seGridObject = gridHexXZ.GetGridObject(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.y-1));
            if (seGridObject != null)
            {
                availableHexes.Add(seGridObject);
            }
        }
        private class GridObject {
            public Transform HexObject;
            public DBS.HexGrid.Mana Mana;
            public Vector2 GridPosition;

            public void Select() {
                Instance.selectedHex = this;
                Debug.Log("Selected Hex is now color is " + Mana.manaColor + " gridPos: " + GridPosition.x + ", " + GridPosition.y);
                HexObject.Find("Selected").gameObject.SetActive(true);
            }

            public void Deselect()
            {
                if (Instance.selectedHex == this) Instance.selectedHex = null;
                HexObject.Find("Selected").gameObject.SetActive(false);
            }
        }
    }
}
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
using UnityEngine;
using Types = DBS.utils.Types;

namespace DBS
{
    public class Game : MonoBehaviour
    {
        //Setup Singleton
        public static Game Instance { get; private set; }

        //Public Variables
        public GridHexXZ<Game.GridObject> PlayerBoard;
        public GridHexXZ<Game.GridObject> EnemyBoard;
        
        //Private Variables
        [ShowInInspector] private GridObject selectedHex;
        [ShowInInspector] private GridObject swappedHex;
        [ShowInInspector] private Types.PlayerAction playersCurrentAction;
        [ShowInInspector] private List<GridObject> consecutiveColors = new();
        [ShowInInspector] private int distanceToCheckForMatches = 2;

        private void Awake()
        {
            //Manage Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
            
            selectedHex = null;
            swappedHex = null;
            playersCurrentAction = Types.PlayerAction.Idle;
        }
        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            //Left Click Down
            if (Input.GetMouseButtonDown(0))
            {
                GridObject newSelectedHex = PlayerBoard.GetGridObject(Mouse3D.GetMouseWorldPosition());

                if (newSelectedHex == null) return;

                if (newSelectedHex.Mana.manaEnergy > 0)
                {
                    if (newSelectedHex == selectedHex)
                    {
                        selectedHex.Deselect();
                        return;
                    }

                    if (selectedHex != null)
                    {
                        selectedHex.Deselect();
                    }

                    selectedHex = newSelectedHex;
                    selectedHex.Select();
                }
            }

            //Left Click Hold
            if (Input.GetMouseButton(0))
            {
                playersCurrentAction = Types.PlayerAction.Dragging;
            }

            //Left Click Up
            if (Input.GetMouseButtonUp(0))
            {
                if (playersCurrentAction == Types.PlayerAction.Dragging)
                {
                    if (Input.GetMouseButtonUp(0) && selectedHex != null)
                    {
                        GridObject newSelectedHex = PlayerBoard.GetGridObject(Mouse3D.GetMouseWorldPosition());

                        if (newSelectedHex == null) return;

                        if (newSelectedHex != selectedHex && newSelectedHex.Mana.manaEnergy > 0)
                        {
                            swappedHex = newSelectedHex;
                            SwapHexMana();
                            
                            void SwapHexMana()
                            {
                                if (selectedHex != null && swappedHex != null)
                                {
                                    Types.ManaColors selectedManaColor = selectedHex.Mana.manaColor;
                                    selectedHex.Mana.SetManaColor(swappedHex.Mana.manaColor);
                                    swappedHex.Mana.SetManaColor(selectedManaColor);
                
                                    FindMatches(PlayerBoard, swappedHex.GridPosition, swappedHex.Mana.manaColor);

                                    if (consecutiveColors.Count >= distanceToCheckForMatches)
                                    {
                                        swappedHex.Mana.DisableMana();
                                        foreach (GridObject gridObject in consecutiveColors)
                                        {
                                            gridObject.Mana.DisableMana();
                                        }
                                        swappedHex.Unit.CreateUnit(Types.Units.SkeletonWarrior, false);
                                    }
                
                                    selectedHex.Deselect();
                                    swappedHex.Deselect();

                                    selectedHex = null;
                                    swappedHex = null;
                                }
                            }
                        }
                    }
                }

                //Reset Action to Idle
                playersCurrentAction = Types.PlayerAction.Idle;
            }
        }
        
        // Given a board and a starting position, get a hex on that board in a certain direction a certain distance away. 
        public GridObject GetHex(GridHexXZ<Game.GridObject> board, Vector2 startingHexPosition, Types.HexDirections direction, int distance)
        {
            switch (direction)
            {
                case Types.HexDirections.E:
                    return board.GetGridObject(Mathf.RoundToInt(startingHexPosition.x+distance), Mathf.RoundToInt(startingHexPosition.y));
                default:
                    return null;
            }
        }
        
        // Given a board get the hex at the given position 
        public GridObject GetHex(GridHexXZ<Game.GridObject> board, Vector2 startingHexPosition)
        {
            return board.GetGridObject(Mathf.RoundToInt(startingHexPosition.x), Mathf.RoundToInt(startingHexPosition.y));
        }

        //Check for matching mana on a given board from a starting position. The matches will be check given the distance in the games distanceToCheckForMatches variable
        public void FindMatches(GridHexXZ<Game.GridObject> board, Vector2 startPosition, Types.ManaColors colorToCheck)
        {
            consecutiveColors.Clear();
            //find consecutive
            if (distanceToCheckForMatches < 1)
            {
                return;
            }


            List<GridObject> consecutiveEast = new List<GridObject>();

            void CheckEast()
            {
                bool isConsecutive = true;
                for (int i = 1; i < distanceToCheckForMatches + 1; i++)
                {
                    if (!isConsecutive) break;
                    GridObject eGridObject = board.GetGridObject(Mathf.RoundToInt(startPosition.x+i), Mathf.RoundToInt(startPosition.y));

                    if (eGridObject != null)
                    {
                        if (eGridObject.Mana.manaColor == colorToCheck)
                        {
                            consecutiveEast.Add(eGridObject);
                        }
                        else
                        {
                            isConsecutive = false;
                        }
                    }
                }

                if (consecutiveEast.Count > consecutiveColors.Count)
                {
                    consecutiveColors = consecutiveEast;
                }
            }
            
            CheckEast();
            
            //find surrounding
        }

        //The GridObject class used by the game
        public class GridObject
        {
            public Transform HexObject;
            public Hex_Properties.Mana Mana;
            public Vector2 GridPosition;
            public DBS.HexProperties.Unit Unit;

            public void Select()
            {
                Instance.selectedHex = this;
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
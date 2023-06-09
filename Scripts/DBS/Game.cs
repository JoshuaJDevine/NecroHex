﻿/*
 * Notes:
 * Game starts. Extra mana poofs away. Characters in grid take posiitons... i.e skeleton footman go to front of grid. Attacks assmebled outside pushing into the grid
 * trying to defeat the players forces. Renew the energy. Leave the characters. You can also swap the characters to match three, trigger a death animation, and make them stronger
 * Also find a way to power the existing characters with extra mana
 * 
 * Start game with 2x2 grid for player
 * Finish implementing all match directions includinng all surrounding for bonus
 * Add Damage Numbers
 * Add Health Bars
 * Max moves players has at start is 9? Start with 9 and test to see what is needed
 * Skeletons can absorb certain mana around them to become slightly more powerful
 * Will need to fight three footmen with 1-2 skeletons
 * Can consume green mana slime blob to create a double green (implement this)
 *
 * Add SFX
 * Add music
 * Add title screen (made in a weekend productions)
 * 
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
using DBS.HexProperties;
using DBS.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
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
        public List<Unit> activeEnemyUnits = new List<Unit>();
        public List<Unit> activePlayerUnits = new List<Unit>();
        public Necromancer necromancer;
        public Cauldron.Cauldron cauldron;

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
                
                Debug.Log("Click on " + newSelectedHex.GridPosition);
                
                if (newSelectedHex.Mana.manaEnergy > 0 && necromancer.Mana > 0)
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

                        Debug.Log("SELECTED HEX: " + selectedHex.Mana.manaEnergy);
                        Debug.Log("SwapHEx: " + newSelectedHex.Mana.manaEnergy);
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
                                        swappedHex.Unit.gridObject = swappedHex;

                                        necromancer.objectsToCastTo.Enqueue(swappedHex);

                                        necromancer.PlayAnimation(
                                            Types.NecromancerAnimations.Cast, 
                                            () =>
                                            {
                                                Debug.Log("Animation ended");
                                            });
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
            
            
            if (Input.GetMouseButtonDown(1) && playersCurrentAction == Types.PlayerAction.Idle)
            {
                GridObject newSelectedHex = PlayerBoard.GetGridObject(Mouse3D.GetMouseWorldPosition());
                
                if (newSelectedHex == null) return;
                Debug.Log("Click on " + newSelectedHex.GridPosition);

                GridObject hexToCreateManaAt = GetEmptyHexClosestToZero(PlayerBoard, newSelectedHex);
                if (hexToCreateManaAt != null)
                {
                    cauldron.CreateMana(cauldron.currentAvailableColor, hexToCreateManaAt);
                }
                else
                {
                    Debug.Log("Error: hex was null");
                }
                
                Types.ManaColors newColor = (Types.ManaColors)Random.Range(0, System.Enum.GetValues(typeof(Types.ManaColors)).Length);
                cauldron.SetCauldronColor(newColor);
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

        public GridObject GetEmptyHexClosestToZero(GridHexXZ<Game.GridObject> board, GridObject startingHex)
        {
            if (startingHex.GridPosition.y == 0) return startingHex;

            GridObject result = null;
            
            for (int i = 0; i < board.GetHeight(); i++)
            {
                GridObject hexObject = GetHex(board, new Vector2(startingHex.GridPosition.x, i));
                if (hexObject.Mana.manaEnergy <= 0 && !hexObject.Unit.HasUnit)
                {
                    result = hexObject;
                    break;
                }
            }

            return result;
        }

        public GridObject GetRandomHex(GridHexXZ<Game.GridObject> board)
        {
            Game.GridObject gridObject = Game.Instance.GetHex(board, new Vector2(
                Random.Range(0,Game.Instance.PlayerBoard.GetWidth()), 
                Random.Range(0,Game.Instance.PlayerBoard.GetHeight())));

            return gridObject;
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
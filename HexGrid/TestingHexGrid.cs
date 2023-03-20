using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types = DBS.utils.Types;

public class TestingHexGrid : MonoBehaviour {


    [SerializeField] private Transform pfHex;

    private GridHexXZ<GridObject> gridHexXZ;
    private GridObject lastGridObject;

    private class GridObject {
        public Transform visualTransform;
        public DBS.Hex_Properties.Mana mana;
        public Vector2 gridPosition;

        public void Show() {
            visualTransform.Find("Selected").gameObject.SetActive(true);
            Debug.Log("SELECTED MANA COLOR: " + mana.manaColor);
        }

        public void Hide() {
            visualTransform.Find("Selected").gameObject.SetActive(false);
        }

    }

    private void Awake() {
        int width = 8;
        int height = 8;
        float cellSize = 1f;
        gridHexXZ = 
            new GridHexXZ<GridObject>(width, height, cellSize, Vector3.zero, (GridHexXZ<GridObject> g, int x, int y) => new GridObject());

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                Transform visualTransform = Instantiate(pfHex, gridHexXZ.GetWorldPosition(x, z), Quaternion.identity);
                GridObject newGridObj = gridHexXZ.GetGridObject(x, z);
                newGridObj.visualTransform = visualTransform;
                newGridObj.mana = newGridObj.visualTransform.gameObject.GetComponent<DBS.Hex_Properties.Mana>();
                newGridObj.gridPosition = new Vector2(x, z);
                newGridObj.Hide();
            }
        }

    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            if (lastGridObject != null) {
                lastGridObject.Hide();
            }

            lastGridObject = gridHexXZ.GetGridObject(Mouse3D.GetMouseWorldPosition());

            if (lastGridObject != null) {
                lastGridObject.Show();
            }
        }

        
    }
}
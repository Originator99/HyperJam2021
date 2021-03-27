using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    private readonly int rows, columns;
    private readonly float gridSize;

    public List<GridCell> gridCells { get; set; } = new List<GridCell>();
    public Grid(int rows, int columns, float gridSize) {
        if(rows == 0 || columns == 0) {
            Debug.LogError("Cannot generate grid, rows and columns are null");
        } else {
            this.rows = rows;
            this.columns = columns;
            this.gridSize = gridSize;
            GenerateGridCells();
        }
    }

    private void GenerateGridCells() {
        int offset = 1;
        int height = rows / 2;
        //Generating the top rows first
        for(int y = 0; y <= height; y++) {
            int breadth = columns / 2;
            //Generating left columns
            for(int x = 0; x <= breadth; x++) {
                PlaceGridCell(-x, y * offset);
            }
            breadth = columns - columns / 2;
            //Generating right columns
            for(int x = 1; x < breadth; x++) {
                PlaceGridCell(x, y * offset);
            }
        }
        offset = -1;
        height = rows - rows / 2;
        //Generating bottom rows
        for(int y = 1; y < height; y++) {
            int breadth = columns / 2;
            //Generating left columns
            for(int x = 0; x <= breadth; x++) {
                PlaceGridCell(-x, y * offset);
            }
            breadth = columns - columns / 2;
            //Generating right columns
            for(int x = 1; x < breadth; x++) {
                PlaceGridCell(x, y * offset);
            }
        }
    }

    private void PlaceGridCell(float xPos, float yPos) {
        GridCell cell = new GridCell(new Vector2(xPos,yPos) * gridSize);
        gridCells.Add(cell);
    }
}

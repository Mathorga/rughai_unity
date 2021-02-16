using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field<T> {
    private Vector2 position;
    private int width;
    private int height;
    private float cellWidth;
    private float cellHeight;

    private T[,] data;

    Field(int width, int height, float cellWidth, float cellHeight, Func<T> constructor) {
        this.width = width;
        this.height = height;

        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;

        this.data = new T[width, height];

        // Instantiate data.
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                this.data[i, j] = constructor();
            }
        }
    }

    public void SetElement(int x, int y, T value) {
        if (x >= 0  && y >= 0 && x < this.width && y < this.height) {
            this.data[x, y] = value;
        }
    }

    public void SetElement(Vector2 position, T value) {
        Vector2Int index = this.PositionToIndex(position);
        this.SetElement(index.x, index.y, value);
    }

    public T GetElement(int x, int y) {
        T result = default(T);
        if (x >= 0  && y >= 0 && x < this.width && y < this.height) {
            result = this.data[x, y];
        }
        return result;
    }

    public T GetElement(Vector2 position) {
        Vector2Int index = this.PositionToIndex(position);
        return this.GetElement(index.x, index.y);
    }

    private Vector2 IndexToPosition(int x, int y) {
        return this.position + new Vector2(x * this.cellWidth, y * this.cellHeight);
    }

    private Vector2Int PositionToIndex(Vector2 position) {
        // Compute cell position relative to field position.
        Vector2 relativePosition = position - this.position;

        return new Vector2Int(Mathf.FloorToInt(relativePosition.x / this.cellWidth), Mathf.FloorToInt(relativePosition.y / this.cellHeight));
    }
}

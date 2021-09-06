using System;
using UnityEngine;

public class Field<T> {
    public event EventHandler<OnFieldElementChangedEventArgs> OnFieldElementChanged;
    public class OnFieldElementChangedEventArgs {
        public int x;
        public int y;
    }

    public Vector2 position {
        get;
        private set;
    }
    public int width {
        get;
        private set;
    }
    public int height {
        get;
        private set;
    }
    public float cellWidth {
        get;
        private set;
    }
    public float cellHeight {
        get;
        private set;
    }

    public T[,] data;

    public Field(int width, int height, float cellWidth, float cellHeight, Vector2 position, Func<Field<T>, int, int, T> constructor) {
        // Set width and height.
        this.width = width;
        this.height = height;

        // Set cell size.
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;

        // Set position.
        this.position = position;

        // Initialize data.
        this.data = new T[width, height];

        // Instantiate data.
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                this.data[i, j] = constructor(this, i, j);
            }
        }
    }

    public void TriggerElementChanged(int x, int y) {
        // Trigger element change event.
        if (this.OnFieldElementChanged != null) {
            this.OnFieldElementChanged(this, new OnFieldElementChangedEventArgs{x = x, y = y});
        }
    }

    public void SetElement(int x, int y, T value) {
        if (x >= 0  && y >= 0 && x < this.width && y < this.height) {
            this.data[x, y] = value;

            // Trigger element change event.
            if (this.OnFieldElementChanged != null) {
                this.OnFieldElementChanged(this, new OnFieldElementChangedEventArgs{x = x, y = y});
            }
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

    public Vector2 IndexToPosition(int x, int y) {
        return this.position + new Vector2((x * this.cellWidth) + (this.cellWidth * 0.5f), (y * this.cellHeight) + (this.cellHeight * 0.5f));
    }

    public Vector2Int PositionToIndex(Vector2 position) {
        // Compute cell position relative to field position.
        Vector2 relativePosition = position - this.position;

        return new Vector2Int(Mathf.FloorToInt(relativePosition.x / this.cellWidth), Mathf.FloorToInt(relativePosition.y / this.cellHeight));
    }
}

﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;

    public Color defaultColor = Color.white;

    public Texture2D noiseSource;

    HexCell[] cells;
    Canvas gridCanvas;
    HexMesh hexMesh;

    void Awake() {
        HexMetrics.noiseSource = noiseSource;

        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for(int z = 0, i = 0; z < height; z++) {
            for(int x = 0; x < width; x++) {
                CreateCell(x, z, i++);
            }
        }
    }

    private void OnEnable() {
        HexMetrics.noiseSource = noiseSource;       
    }

    void Start() {
        hexMesh.Triangulate(cells);    
    }

    public HexCell GetCell(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return cells[index];
    }

    public void Refresh() {
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i) {
        Vector3 position;
        position.x = (x + z * 0.5f - (z/2)) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        // set position and coordinates
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        cell.color = defaultColor;

        // attach neighbors
        if (x > 0) { // set western neighbor if this cell is not on the western border
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0) { // set southern neighbors if this cell is not on the southern border
            if ((z & 1) == 0) { // deal with hex rows alternating back and forth
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0) {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            } else {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1) { // if this cell is not on the eastern border
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        // add label
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;
    }
}

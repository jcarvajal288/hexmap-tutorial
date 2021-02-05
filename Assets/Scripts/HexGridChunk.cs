﻿using UnityEngine;
using UnityEngine.UI;

public class HexGridChunk : MonoBehaviour
{
    HexCell[] cells;

    HexMesh hexMesh;
    Canvas gridCanvas;

    private void Awake() {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

    void Start() {
        hexMesh.Triangulate(cells);
    }

    public void AddCell(int index, HexCell cell) {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

    public void Refresh() {
        enabled = true; // tag this chunk as needing to be refreshed
    }

    private void LateUpdate() {
        hexMesh.Triangulate(cells);
        enabled = false;
    }
}

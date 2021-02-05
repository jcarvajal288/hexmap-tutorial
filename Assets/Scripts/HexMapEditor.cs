﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    private Color activeColor;
    private int activeElevation;

    private void Awake() {
        SelectColor(0);
    }

    public void SelectColor(int index) {
        activeColor = colors[index];
    }

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    private void Update() {
        if (Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput();
        }
    }

    private void HandleInput() {
        Vector3 mousePosition = Input.mousePosition;
        Ray inputRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            EditCell(hexGrid.GetCell(hit.point));
        }
    }

    void EditCell(HexCell cell) {
        cell.Color = activeColor;
        cell.Elevation = activeElevation;
    }
}

using UnityEngine;

public class HexCell : MonoBehaviour
{
    [SerializeField]
    internal HexCoordinates coordinates;
    internal Color color;

    [SerializeField]
    HexCell[] neighbors;

    public RectTransform uiRect;

    public int Elevation {
        get {
            return elevation;
        }
        set {
            elevation = value;
            // move the cell up to match its new elevation
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y +=
                (HexMetrics.SampleNoise(position).y * 2f - 1f) *
                HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            // move the UI label up as well to match
            Vector3 uiPosition = uiRect.localPosition;
            //uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;
        }
    }
    int elevation;

    public Vector3 Position {
        get {
            return transform.localPosition;
        }
    }

    public HexCell GetNeighbor(HexDirection direction) {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell) {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public HexEdgeType GetEdgeType(HexDirection direction) {
        return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
    }

    public HexEdgeType GetEdgeType(HexCell otherCell) {
        return HexMetrics.GetEdgeType(elevation, otherCell.elevation);
    }
}

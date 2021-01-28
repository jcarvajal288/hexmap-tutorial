using UnityEngine;
using UnityEditor;

// this is supposed to show the coordinates in the inspector for each HexCell.  
// It doesn't seem to work in this version of Unity

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        HexCoordinates coordinates = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
        );
        GUI.Label(position, coordinates.ToString());
        //base.OnGUI(position, property, label);
    }
}

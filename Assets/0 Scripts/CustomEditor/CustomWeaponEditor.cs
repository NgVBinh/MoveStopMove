#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeaponController))]
public class CustomWeaponEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Bắt đầu nhóm thuộc tính
        EditorGUI.BeginProperty(position, label, property);

        // Tính toán kích thước và vị trí của các thuộc tính
        Rect attackTypeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect rotateSpeedRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

        // Lấy các thuộc tính
        SerializedProperty weaponTypeProperty = property.FindPropertyRelative("weaponType");
        SerializedProperty rotateSpeedProperty = property.FindPropertyRelative("rotateSpeed");

        // Vẽ trường attackType
        EditorGUI.PropertyField(attackTypeRect, weaponTypeProperty);

        // Nếu attackType là Spin, vẽ trường rotateSpeed
        if ((WeaponType)weaponTypeProperty.enumValueIndex == WeaponType.SPIN)
        {
            EditorGUI.PropertyField(rotateSpeedRect, rotateSpeedProperty);
        }

        // Kết thúc nhóm thuộc tính
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty attackTypeProperty = property.FindPropertyRelative("weaponType");

        // Tính toán chiều cao của nhóm thuộc tính
        float height = EditorGUIUtility.singleLineHeight + 2; // Chiều cao của trường attackType

        // Nếu attackType là Spin, thêm chiều cao cho trường rotateSpeed
        if ((WeaponType)attackTypeProperty.enumValueIndex == WeaponType.SPIN)
        {
            height += EditorGUIUtility.singleLineHeight + 2;
        }

        return height;
    }
}

#endif

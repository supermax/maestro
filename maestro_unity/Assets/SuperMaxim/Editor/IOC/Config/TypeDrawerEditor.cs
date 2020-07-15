using System.Reflection;
using SuperMaxim.IOC.Attributes;
using UnityEditor;
using UnityEngine;

namespace SuperMaxim.Editor.IOC.Config
{
    [CustomPropertyDrawer(typeof(TypeDrawerAttribute))]
    public class TypeDrawerEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // var obj = fieldInfo.GetValue(property.serializedObject.targetObject) as TypeDelegator;
            //
            // var res = EditorGUI.ObjectField(position, obj, typeof(MonoScript), false) as MonoScript;
            // Debug.Log(res);
            //
            // var td = new System.Reflection.TypeDelegator(res.GetClass()); 
            //
            // fieldInfo.SetValue(obj, td);
        }
    }
}
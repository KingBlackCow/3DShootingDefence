﻿using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Invector
{
    [CustomPropertyDrawer(typeof(vEnumFlagAttribute))]
    public class vEnumFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            vEnumFlagAttribute flagSettings = (vEnumFlagAttribute)attribute;
            Enum targetEnum = GetBaseProperty<Enum>(property);

            string propName = flagSettings.enumName;
            if (string.IsNullOrEmpty(propName))
                propName = property.displayName;

            EditorGUI.BeginProperty(position, label, property);
#if UNITY_2017_1_OR_NEWER
            Enum enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);
#else
            Enum enumNew = EditorGUI.EnumMaskField(position, propName, targetEnum);
#endif
            property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            EditorGUI.EndProperty();
        }

        static T GetBaseProperty<T>(SerializedProperty prop)
        {
            // Separate the steps it takes to get to this property
            string[] separatedPaths = prop.propertyPath.Split('.');

            // Go down to the root of this serialized property
            System.Object reflectionTarget = prop.serializedObject.targetObject as object;
            // Walk down the path to get the target object
            foreach (var path in separatedPaths)
            {
                FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path);
                reflectionTarget = fieldInfo.GetValue(reflectionTarget);
            }
            return (T)reflectionTarget;
        }
    }
}
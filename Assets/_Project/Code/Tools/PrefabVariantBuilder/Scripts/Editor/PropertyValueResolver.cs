using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace MAEngine.PrefabVariantBuilder
{
    internal static class PropertyValueResolver
    {
        internal static PropertySnapshot Capture(SerializedProperty prop)
        {
            var snap = new PropertySnapshot
            {
                PropertyPath = prop.propertyPath,
                Type = prop.propertyType
            };

            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.LayerMask:
                    snap.StringValue = prop.intValue.ToString(CultureInfo.InvariantCulture);
                    break;
                case SerializedPropertyType.Boolean:
                    snap.BoolValue = prop.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    snap.FloatValue = prop.floatValue;
                    break;
                case SerializedPropertyType.String:
                    snap.StringValue = prop.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    var c = prop.colorValue;
                    snap.VectorValue = FormattableString.Invariant($"{c.r},{c.g},{c.b},{c.a}");
                    break;
                case SerializedPropertyType.ObjectReference:
                    snap.ObjectRefValue = prop.objectReferenceValue;
                    break;
                case SerializedPropertyType.Enum:
                    snap.StringValue = prop.enumValueIndex.ToString(CultureInfo.InvariantCulture);
                    break;
                case SerializedPropertyType.Vector2:
                    var v2 = prop.vector2Value;
                    snap.VectorValue = FormattableString.Invariant($"{v2.x},{v2.y}");
                    break;
                case SerializedPropertyType.Vector3:
                    var v3 = prop.vector3Value;
                    snap.VectorValue = FormattableString.Invariant($"{v3.x},{v3.y},{v3.z}");
                    break;
                case SerializedPropertyType.Vector4:
                    var v4 = prop.vector4Value;
                    snap.VectorValue = FormattableString.Invariant($"{v4.x},{v4.y},{v4.z},{v4.w}");
                    break;
                case SerializedPropertyType.Rect:
                    var r = prop.rectValue;
                    snap.VectorValue = FormattableString.Invariant($"{r.x},{r.y},{r.width},{r.height}");
                    break;
                case SerializedPropertyType.Bounds:
                    var b = prop.boundsValue;
                    snap.VectorValue = FormattableString.Invariant($"{b.center.x},{b.center.y},{b.center.z},{b.extents.x},{b.extents.y},{b.extents.z}");
                    break;
                case SerializedPropertyType.Quaternion:
                    var q = prop.quaternionValue;
                    snap.VectorValue = FormattableString.Invariant($"{q.x},{q.y},{q.z},{q.w}");
                    break;
                case SerializedPropertyType.Vector2Int:
                    var v2i = prop.vector2IntValue;
                    snap.VectorValue = FormattableString.Invariant($"{v2i.x},{v2i.y}");
                    break;
                case SerializedPropertyType.Vector3Int:
                    var v3i = prop.vector3IntValue;
                    snap.VectorValue = FormattableString.Invariant($"{v3i.x},{v3i.y},{v3i.z}");
                    break;
                case SerializedPropertyType.RectInt:
                    var ri = prop.rectIntValue;
                    snap.VectorValue = FormattableString.Invariant($"{ri.x},{ri.y},{ri.width},{ri.height}");
                    break;
                case SerializedPropertyType.BoundsInt:
                    var bi = prop.boundsIntValue;
                    snap.VectorValue = FormattableString.Invariant($"{bi.position.x},{bi.position.y},{bi.position.z},{bi.size.x},{bi.size.y},{bi.size.z}");
                    break;
                // AnimationCurve, Gradient, Generic, ManagedReference: skip (treat as equal)
            }

            return snap;
        }

        internal static bool AreEqual(PropertySnapshot a, PropertySnapshot b)
        {
            if (a.Type != b.Type) return false;

            switch (a.Type)
            {
                case SerializedPropertyType.Float:
                    return Mathf.Approximately(a.FloatValue, b.FloatValue);

                case SerializedPropertyType.Boolean:
                    return a.BoolValue == b.BoolValue;

                case SerializedPropertyType.Integer:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.String:
                    return string.Equals(a.StringValue, b.StringValue, StringComparison.Ordinal);

                case SerializedPropertyType.Color:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.BoundsInt:
                    return VectorsApproximatelyEqual(a.VectorValue, b.VectorValue);

                case SerializedPropertyType.ObjectReference:
                    return ObjectRefsEqual(a.ObjectRefValue, b.ObjectRefValue);

                default:
                    // AnimationCurve, Generic, ManagedReference etc.: treat as equal to avoid false overrides
                    return true;
            }
        }

        internal static void Apply(SerializedProperty prop, PropertySnapshot snap)
        {
            switch (snap.Type)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.LayerMask:
                    if (int.TryParse(snap.StringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var iv))
                        prop.intValue = iv;
                    break;
                case SerializedPropertyType.Boolean:
                    prop.boolValue = snap.BoolValue;
                    break;
                case SerializedPropertyType.Float:
                    prop.floatValue = snap.FloatValue;
                    break;
                case SerializedPropertyType.String:
                    prop.stringValue = snap.StringValue;
                    break;
                case SerializedPropertyType.Color:
                    prop.colorValue = ParseColor(snap.VectorValue);
                    break;
                case SerializedPropertyType.ObjectReference:
                    prop.objectReferenceValue = snap.ObjectRefValue;
                    break;
                case SerializedPropertyType.Enum:
                    if (int.TryParse(snap.StringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ei))
                        prop.enumValueIndex = ei;
                    break;
                case SerializedPropertyType.Vector2:
                    prop.vector2Value = ParseVector2(snap.VectorValue);
                    break;
                case SerializedPropertyType.Vector3:
                    prop.vector3Value = ParseVector3(snap.VectorValue);
                    break;
                case SerializedPropertyType.Vector4:
                    prop.vector4Value = ParseVector4(snap.VectorValue);
                    break;
                case SerializedPropertyType.Rect:
                    prop.rectValue = ParseRect(snap.VectorValue);
                    break;
                case SerializedPropertyType.Quaternion:
                    prop.quaternionValue = ParseQuaternion(snap.VectorValue);
                    break;
                case SerializedPropertyType.Vector2Int:
                    prop.vector2IntValue = ParseVector2Int(snap.VectorValue);
                    break;
                case SerializedPropertyType.Vector3Int:
                    prop.vector3IntValue = ParseVector3Int(snap.VectorValue);
                    break;
                case SerializedPropertyType.Bounds:
                    prop.boundsValue = ParseBounds(snap.VectorValue);
                    break;
                case SerializedPropertyType.BoundsInt:
                    prop.boundsIntValue = ParseBoundsInt(snap.VectorValue);
                    break;
                case SerializedPropertyType.RectInt:
                    prop.rectIntValue = ParseRectInt(snap.VectorValue);
                    break;
            }
        }

        private static bool VectorsApproximatelyEqual(string a, string b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            var partsA = a.Split(',');
            var partsB = b.Split(',');
            if (partsA.Length != partsB.Length) return false;
            for (var i = 0; i < partsA.Length; i++)
            {
                if (!float.TryParse(partsA[i], NumberStyles.Float, CultureInfo.InvariantCulture, out var fa)) return false;
                if (!float.TryParse(partsB[i], NumberStyles.Float, CultureInfo.InvariantCulture, out var fb)) return false;
                if (!Mathf.Approximately(fa, fb)) return false;
            }
            return true;
        }

        private static bool ObjectRefsEqual(UnityEngine.Object a, UnityEngine.Object b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            var pathA = AssetDatabase.GetAssetPath(a);
            var pathB = AssetDatabase.GetAssetPath(b);
            if (!string.IsNullOrEmpty(pathA) && !string.IsNullOrEmpty(pathB))
                return string.Equals(pathA, pathB, StringComparison.OrdinalIgnoreCase);
            return a.GetInstanceID() == b.GetInstanceID();
        }

        private static float[] ParseFloats(string s)
        {
            var parts = s.Split(',');
            var result = new float[parts.Length];
            for (var i = 0; i < parts.Length; i++)
                float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out result[i]);
            return result;
        }

        private static Color ParseColor(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 4 ? new Color(f[0], f[1], f[2], f[3]) : Color.white;
        }

        private static Vector2 ParseVector2(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 2 ? new Vector2(f[0], f[1]) : Vector2.zero;
        }

        private static Vector3 ParseVector3(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 3 ? new Vector3(f[0], f[1], f[2]) : Vector3.zero;
        }

        private static Vector4 ParseVector4(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 4 ? new Vector4(f[0], f[1], f[2], f[3]) : Vector4.zero;
        }

        private static Rect ParseRect(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 4 ? new Rect(f[0], f[1], f[2], f[3]) : Rect.zero;
        }

        private static Quaternion ParseQuaternion(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 4 ? new Quaternion(f[0], f[1], f[2], f[3]) : Quaternion.identity;
        }

        private static Vector2Int ParseVector2Int(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 2 ? new Vector2Int((int)f[0], (int)f[1]) : Vector2Int.zero;
        }

        private static Vector3Int ParseVector3Int(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 3 ? new Vector3Int((int)f[0], (int)f[1], (int)f[2]) : Vector3Int.zero;
        }

        // Capture format: "cx,cy,cz,ex,ey,ez" (center + extents).
        // Bounds ctor takes (center, size) where size = extents * 2.
        private static Bounds ParseBounds(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 6
                ? new Bounds(new Vector3(f[0], f[1], f[2]), new Vector3(f[3], f[4], f[5]) * 2f)
                : new Bounds();
        }

        // Capture format: "px,py,pz,sx,sy,sz" (position + size).
        private static BoundsInt ParseBoundsInt(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 6
                ? new BoundsInt(
                    new Vector3Int((int)f[0], (int)f[1], (int)f[2]),
                    new Vector3Int((int)f[3], (int)f[4], (int)f[5]))
                : new BoundsInt();
        }

        // Capture format: "x,y,width,height".
        private static RectInt ParseRectInt(string s)
        {
            var f = ParseFloats(s);
            return f.Length >= 4
                ? new RectInt((int)f[0], (int)f[1], (int)f[2], (int)f[3])
                : new RectInt();
        }
    }
}

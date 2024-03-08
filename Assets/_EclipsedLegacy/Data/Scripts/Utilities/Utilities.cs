using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace in3d.Utilities
{
    public static class Utilities
    {
        static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDict = new(100, new FloatComparer());


        /// <summary>
        /// Copies the values of the fields from one object to another.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="Base">The object from which to copy the values.</param>
        /// <param name="Copy">The object to which the values will be copied.</param>
        public static void CopyValues<T>(T Base, T Copy)
        {
            Type type = Base.GetType();
            foreach (FieldInfo field in type.GetFields())
            {
                field.SetValue(Copy, field.GetValue(Base));
            }
        }

        /// <summary>
        /// Returns a WaitForSeconds object for the specified duration. </summary>
        /// <param name="seconds">The duration in seconds to wait.</param>
        /// <returns>A WaitForSeconds object.</returns>
        public static WaitForSeconds GetWaitForSeconds(float seconds) {
            if (seconds < 1f / Application.targetFrameRate) return null;

            if (WaitForSecondsDict.TryGetValue(seconds, out var forSeconds)) return forSeconds;

            var waitForSeconds = new WaitForSeconds(seconds);
            WaitForSecondsDict[seconds] = waitForSeconds;
            return waitForSeconds;
        }

        class FloatComparer : IEqualityComparer<float> {
            public bool Equals(float x, float y) => Mathf.Abs(x - y) <= Mathf.Epsilon;
            public int GetHashCode(float obj) => obj.GetHashCode();
        }
    }
}

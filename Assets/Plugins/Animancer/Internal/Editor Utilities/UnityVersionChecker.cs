// Animancer // Copyright 2020 Kybernetik //

#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Lite-Only]
    /// Validates that the Animancer.Lite.dll is the correct one for this version of Unity.
    /// </summary>
    [InitializeOnLoad]
    internal static class UnityVersionChecker
    {
        /************************************************************************************************************************/

#if UNITY_2019_3_OR_NEWER
        const string ExpectedAssemblyTarget = "2019.3";
#elif UNITY_2019_1_OR_NEWER
        const string ExpectedAssemblyTarget = "2019.1";
#elif UNITY_2018_3_OR_NEWER
        const string ExpectedAssemblyTarget = "2018.3";
#elif UNITY_2018_1_OR_NEWER
        const string ExpectedAssemblyTarget = "2018.1";
#elif UNITY_2017_3_OR_NEWER
        const string ExpectedAssemblyTarget = "2017.3";
#else
        const string ExpectedAssemblyTarget = "2017.1";
#endif

        /************************************************************************************************************************/

        static UnityVersionChecker()
        {
            const string SessionStateKey = "Animancer.HasCheckedUnityVersion";

            if (SessionState.GetBool(SessionStateKey, false))
                return;

            SessionState.SetBool(SessionStateKey, true);

            var assembly = typeof(AnimancerEditorUtilities).Assembly;
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length != 1)
            {
                Debug.LogWarning(assembly.FullName + " does not have exactly one [AssemblyDescription] attribute.");
                return;
            }

            var attribute = (AssemblyDescriptionAttribute)attributes[0];
            if (attribute.Description.EndsWith("Unity " + ExpectedAssemblyTarget + "+."))
                return;

            var actualAssemblyTarget = attribute.Description.Substring(attribute.Description.Length - 14, 13);
            if (!actualAssemblyTarget.StartsWith("Unity "))
                actualAssemblyTarget = "[Unknown]";

            Debug.LogWarning("Animancer.Lite.dll was compiled for " + actualAssemblyTarget +
                " but the correct target for this version of Unity would be " + ExpectedAssemblyTarget +
                "+.\nYou should download the appropriate version of Animancer from " +
                "https://kybernetik.itch.io/animancer-lite" +
                "\nOr you could ignore this warning and delete UnityVersionChecker.cs to disable it," +
                " but then some features might not work correctly.");
        }

        /************************************************************************************************************************/
    }
}

#endif


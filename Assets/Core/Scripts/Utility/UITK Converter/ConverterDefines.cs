using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace PrSuperSoldier.UI.Converters
{
    [InitializeOnLoad]
    public static class ConverterDefines
    {
        static ConverterDefines()
        {
            RegisterConverterGroup();
            RegisterBooleanNegativeConverter();
        }

        private static void RegisterConverterGroup()
        {
            var group = new ConverterGroup("TimeSpanToMMSSFF");
            group.AddConverter((ref TimeSpan element) => element.ToString(@"mm\:ss\.ff"));
            ConverterGroups.RegisterConverterGroup(group);
        }

        private static void RegisterBooleanNegativeConverter()
        {
            var group = new ConverterGroup("BooleanNegative");
            group.AddConverter((ref bool element) => !element);
            ConverterGroups.RegisterConverterGroup(group);
        }
    }
}

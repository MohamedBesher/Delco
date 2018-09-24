using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Data.Extentions
{
    public static class EnumExtentions
    {

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }


        public static string GetIconValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            IconValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(IconValueAttribute), false) as IconValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].IconValue : null;
        }


        public static string GetColorValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            ColorValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(ColorValueAttribute), false) as ColorValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].ColorValue : null;
        }



    }
}

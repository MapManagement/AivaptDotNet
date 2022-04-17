using System;

using Discord;

namespace AivaptDotNet.Helpers.General
{
    public static class SimpleConverter
    {
        public static Color HexToColor(string hexString)
        {
            if(hexString.Length != 7)
            {
                throw new ArgumentException("HexString has to be valid color string.");
            }

            string redString = hexString.Substring(1, 2);
            string greenString = hexString.Substring(3, 2);
            string blueString = hexString.Substring(5, 2);

            int red = Convert.ToInt32(redString, 16);
            int green = Convert.ToInt32(greenString, 16);
            int blue = Convert.ToInt32(blueString, 16);

            Color color = new Color(red, green, blue);
            return color;

        }

    }
}
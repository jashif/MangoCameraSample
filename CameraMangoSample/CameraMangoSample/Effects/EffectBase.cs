// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System.IO;
using Microsoft.Devices;

namespace PhotoFun.Effects
{
    /// <summary>
    /// This class is a base class for all effect classes
    /// It contains basic functionality that all effect
    /// classes uses
    /// </summary>
    public abstract class EffectBase : IEffect
    {
        /// <summary>
        /// Extracts the Alpha, Red, Green, Blue from the source color 
        /// </summary>
        /// <param name="color">Source color</param>
        /// <param name="a">Alpha component value</param>
        /// <param name="r">Red component value</param>
        /// <param name="g">Green component value</param>
        /// <param name="b">Blue component value</param>
        protected void GetARGB(int color, out int a, out int r, out int g, out int b)
        {
            a = color >> 24;
            r = (color & 0x00ff0000) >> 16;
            g = (color & 0x0000ff00) >> 8;
            b = (color & 0x000000ff);
        }

        /// <summary>
        /// Assemble the ARGB values to one color value 
        /// </summary>
        /// <param name="a">Alpha component value</param>
        /// <param name="r">Red component value</param>
        /// <param name="g">Green component value</param>
        /// <param name="b">Blue component value</param>
        /// <returns>One color value from the given ARGB</returns>
        protected int GetColorFromArgb(int a, int r, int g, int b)
        {
            int result = ((a & 0xFF) << 24) | ((r & 0xFF) << 16) | ((g & 0xFF) << 8) | (b & 0xFF);
            return result;
        }

        /// <summary>
        /// When implemented in a derived class,
        /// takes the raw image as array of pixels and 
        /// returns a processed one
        /// </summary>
        /// <param name="source">Raw image content as array of pixels</param>
        /// <returns>Processed image as array of pixels</returns>
        public abstract int[] ProcessImage(int[] source);
    }
}

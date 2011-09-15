﻿// ----------------------------------------------------------------------------------
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

namespace PhotoFun.Effects
{
    public class SepiaEffect : EffectBase, IEffect
    {
        /// <summary>
        /// Sepia effect for the given pixel by the given depth
        /// </summary>
        /// <param name="color">Pixel color value</param>
        /// <param name="depth">Sepia depth</param>
        /// <returns>Pixel color value after sepia</returns>
        private int Sepia(int color, int depth)
        {
            int a, r, g, b;
            GetARGB(color, out a, out r, out g, out b);

            r = (int)(0.299 * r + 0.587 * g + 0.114 * b);
            g = b = r;

            r += depth * 2;
            if (r > 255)
                r = 255;
            g += depth;
            if (g > 255)
                g = 255;

            int result = ((a & 0xFF) << 24) | ((r & 0xFF) << 16) | ((g & 0xFF) << 8) | (b & 0xFF);

            return result;
        }

        /// <summary>
        /// Returns the Sepia-values of the source image
        /// </summary>
        /// <param name="source">Source image as array of pixels</param>
        /// <returns>the Sepia-values of the source image</returns>
        public override int[] ProcessImage(int[] source)
        {
            int[] target = new int[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                target[i] = Sepia(source[i], 50);
            }

            return target;
        }
    }
}

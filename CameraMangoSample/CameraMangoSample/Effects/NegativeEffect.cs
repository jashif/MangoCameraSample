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

namespace PhotoFun.Effects
{
    public class NegativeEffect : EffectBase, IEffect
    {

        /// <summary>
        /// Returns the negative color of the source color
        /// </summary>
        /// <param name="color">Source color</param>
        /// <returns>Negative color</returns>
        private int Negate(int color)
        {
            int a, r, g, b;
            GetARGB(color, out a, out r, out g, out b);

            r = 255 - r;
            g = 255 - g;
            b = 255 - b;

            int result = GetColorFromArgb(a, r, g, b);

            return result;
        }

        /// <summary>
        ///  Returns the negative pixel values of the source array of pixels
        /// </summary>
        /// <param name="source">Source image as array of pixels</param>
        /// <returns>The negative pixel values of the source array of pixels</returns>
        public override int[] ProcessImage(int[] source)
        {
            int[] target = new int[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                target[i] = Negate(source[i]);
            }

            return target;
        }
    }
}

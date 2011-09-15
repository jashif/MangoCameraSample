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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PhotoFun.Effects
{
    /// <summary>
    /// The blending mode.
    /// </summary>
    public enum BlendMode
    {
        /// <summary>
        /// Alpha blending uses the alpha channel to combine the source and destination. 
        /// </summary>
        Alpha,

        /// <summary>
        /// Additive blending adds the colors of the source and the destination.
        /// </summary>
        Additive,

        /// <summary>
        /// Subtractive blending subtracts the source color from the destination.
        /// </summary>
        Subtractive,

        /// <summary>
        /// Uses the source color as a mask.
        /// </summary>
        Mask,

        /// <summary>
        /// Multiplies the source color with the destination color.
        /// </summary>
        Multiply,

        /// <summary>
        /// Ignores the specified Color
        /// </summary>
        ColorKeying,

        /// <summary>
        /// No blending just copies the pixels from the source.
        /// </summary>
        None
    }
}

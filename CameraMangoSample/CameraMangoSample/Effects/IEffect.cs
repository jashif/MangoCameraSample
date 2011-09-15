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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Devices;

namespace PhotoFun.Effects
{
    /// <summary>
    /// This interface is for image processing effect 
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// When implemented, takes the source array of pixels and returns
        /// the array after processing
        /// </summary>
        /// <param name="source">Source image as array of pixeld</param>
        /// <returns>>Processed image as array of pixels</returns>
        int[] ProcessImage(int[] source);
    }

  
}

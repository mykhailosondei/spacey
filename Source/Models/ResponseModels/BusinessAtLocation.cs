﻿/*
 * Copyright(c) 2018 Microsoft Corporation. All rights reserved. 
 * 
 * This code is licensed under the MIT License (MIT). 
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do 
 * so, subject to the following conditions: 
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE. 
*/

using System;
using System.Runtime.Serialization;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Local Business Resoruce, used by Location Recognition
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class BusinessAtLocation
    {
        /// <summary>
        /// Busisness Address `Address` Resource
        /// </summary>
        [DataMember(Name = "businessAddress", EmitDefaultValue = false)]
        public Address BusinessAddress { get; set; }

        /// <summary>
        /// Resource just for Location Recog, `BusinessInfoEntity`
        /// </summary>
        [DataMember(Name = "businessInfo", EmitDefaultValue = false)]
        public BusinessInfoEntity BusinessInfo { get; set; }

        /// <summary>
        /// Type of business entity representing the primary nature of business of the entity. Refer to the Business Entity Types table below for a full list of supported types
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// List of types which represent the secondary nature of business of the entity
        /// </summary>
        [DataMember(Name = "otherTypes", EmitDefaultValue = false)]
        public string[] OtherTypes { get; set; }
    }
}

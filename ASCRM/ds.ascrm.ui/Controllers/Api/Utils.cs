/*
 * © 2017 LogMeIn, Inc. All Rights Reserved.
 * All rights reserved.
 * 
 * This software is distributed under the terms and conditions of the
 * LogMeIn SDK License Agreement. Please see file LICENSE for details.
 * 
 * Auto-generated file.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMeIn.GoToWebinar.Api.Common
{
    static class Utils
    {
        public static string Stringify(this object obj)
        {
            if (obj == null)
                return string.Empty;

            if (obj is DateTime)
                return ((DateTime)obj).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss%K");

            if (obj is bool)
                return ((bool)obj).ToString().ToLower();

            IEnumerable items = obj as IList ?? (IEnumerable)(obj as IDictionary);
            if (items != null)
                return "[\n" + string.Join(",", from object item in items select Stringify(item)) + "]";

            return obj.ToString();
        }
    }
}
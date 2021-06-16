﻿/* ****************************************************************************
 * Copyright 2021 51 Degrees Mobile Experts Limited (51degrees.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 * ***************************************************************************/

using System.Collections.Generic;

namespace Swan.Client.Model
{
    /// <summary>
    /// Base is the base structure for all actions. It includes the scheme for 
    /// the SWAN Operator URLs, the Operator domain and the access key needed 
    /// by the SWAN Operator.
    /// </summary>
    public class Base
    {
        internal readonly IConnection Connection;

        internal Base()
        { }

        internal Base(IConnection connection)
        {
            Connection = connection;
        }
    }
}

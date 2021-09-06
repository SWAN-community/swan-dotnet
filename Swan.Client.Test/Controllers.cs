/* ****************************************************************************
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swan.Client.Controllers;
using Swan.Client.Model;
using System;
using System.Threading.Tasks;
using Owid.Client;

namespace Swan.Client.Test
{
    [TestClass]
    public class Controllers : Base
    {
        [TestMethod]
        public async Task Fetch_ReturnUrl()
        {
            var controller = new FetchController(_connection)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = Base._httpContext
                }
            };
            var result = await controller.GetUrl(Base.ReturnUrl);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Fetch_UpdateUrl()
        {
            var controller = new UpdateController(_connection)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = Base._httpContext
                }
            };
            var update = new Update(_creator)
            {
                ReturnUrl = Base.ReturnUrl,
            };
            var result = await controller.GetUrl(update);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Decrypt()
        {
            var controller = new DecryptController(_connection)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = Base._httpContext
                }
            };
            var uri = await GetEncryptedFetchData();
            var result = await controller.Get(uri.ToString());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Pair[]));
        }


        [TestMethod]
        public async Task DecryptRaw()
        {
            var controller = new DecryptRawController(_connection)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = Base._httpContext
                }
            };
            var uri = await GetEncryptedFetchData();
            var result = await controller.Get(uri.ToString());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Update));
            var update = result.Value as Update;
            Assert.IsTrue(await update.SwidAsOwid.VerifyAsync());
        }

        private async Task<Uri> GetEncryptedFetchData()
        {
            var fetch = _connection.Fetch(
                _httpContext.Request,
                ReturnUrl);
            var redirectUrl = await fetch.GetURL();
            Assert.IsNotNull(redirectUrl);
            Uri uri;
            Assert.IsTrue(Uri.TryCreate(
                redirectUrl,
                UriKind.Absolute,
                out uri));
            return await uri.MockBrowserRedirect();
        }
    }
}
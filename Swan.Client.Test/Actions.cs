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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owid.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swan.Client.Test
{
    [TestClass]
    public class Actions : Base
    {
        [TestMethod]
        public async Task TestCreateSwid()
        {
            var swid = await _connection.CreateSwid();
            Assert.IsNotNull(swid);
            Assert.AreEqual(swid.Domain, Base.AccessNode);
            Assert.IsTrue(await swid.VerifyAsync());
        }

        [TestMethod]
        public async Task TestHomeNode()
        {
            var homeNode = await _connection.NewClient(
                Base._httpContext.Request).HomeNode();
            Assert.IsNotNull(homeNode);
        }

        [TestMethod]
        public async Task TestStop()
        {
            var first = await _connection.Stop(
                _httpContext.Request,
                ReturnUrl,
                StopDomain).GetURL();
            Assert.IsNotNull(first);
        }

        [TestMethod]
        public async Task TestFetch()
        {
            var first = await _connection.Fetch(
                _httpContext.Request, 
                ReturnUrl).GetURL();
            Assert.IsNotNull(first);
            var second = await _connection.Fetch(
                _httpContext.Request,
                ReturnUrl).GetURL();
            Assert.IsNotNull(second);
            Assert.AreNotEqual(first, second);
        }

        [TestMethod]
        public void TestFetchNoReturnUrl()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _connection.Fetch(
                _httpContext.Request,
                "").GetURL().Result);
        }

        [TestMethod]
        public void TestFetchNoRequest()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _connection.Fetch(
                null,
                ReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public void TestFetchBadReturnUrl()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _connection.Fetch(
                _httpContext.Request,
                BadReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public void TestFetchBadAccessNode()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _badAccessNodeConnection.Fetch(
                _httpContext.Request,
                ReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public void TestFetchBadAccessKey()
        {
            Assert.ThrowsException<AggregateException>(() =>
                _badAccessKeyConnection.Fetch(
                _httpContext.Request,
                ReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            var url = await _connection.Update(
                _httpContext.Request,
                ReturnUrl).GetURL();
            Assert.IsNotNull(url);
        }

        [TestMethod]
        public async Task TestUpdateDecrypt()
        {
            var pairs = await _connection.Decrypt(
                await GetEncryptedUpdateData());
            var swid = new Owid.Client.Model.Owid(
                pairs.Single(i => "swid".Equals(i.Key)).Value);
            var sid = new Owid.Client.Model.Owid(
                pairs.Single(i => "sid".Equals(i.Key)).Value);
            var pref = new Owid.Client.Model.Owid(
                pairs.Single(i => "pref".Equals(i.Key)).Value);
            var val = pairs.Single(i => "val".Equals(i.Key));
            Assert.IsTrue(await swid.VerifyAsync());
            Assert.IsTrue(await sid.VerifyAsync());
            Assert.IsTrue(await pref.VerifyAsync(_creator.Crypto));
            DateTime valDate = DateTime.MinValue;
            Assert.IsTrue(DateTime.TryParse(val.Value, out valDate));
            Assert.AreNotEqual(DateTime.MinValue, valDate);
        }

        [TestMethod]
        public async Task TestUpdateDecryptRaw()
        {
            var pairs = await _connection.DecryptRaw(
                await GetEncryptedUpdateData());
            Assert.AreEqual(Email, pairs.Email);
            Assert.AreEqual(Preference, pairs.Pref);
            Assert.AreEqual(Salt, pairs.Salt);
            Assert.IsTrue(await pairs.SwidAsOwid.VerifyAsync());
        }

        [TestMethod]
        public async Task TestDecrypt()
        {
            var pairs = await _connection.Decrypt(
                await GetEncryptedFetchData());
            Assert.IsNotNull(pairs);
        }

        [TestMethod]
        public async Task TestDecryptRaw()
        {
            var update = await _connection.DecryptRaw(
                await GetEncryptedFetchData());
            Assert.IsNotNull(update);
            Assert.IsNotNull(update.Swid);
        }

        [TestMethod]
        public async Task TestSwid()
        {
            var update = await _connection.DecryptRaw(
                await GetEncryptedFetchData());
            var success = await update.SwidAsOwid.VerifyAsync();
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void TestUpdateEmail()
        {
            var update = _connection.Update(
                _httpContext.Request,
                ReturnUrl);
            update.Email = Base.Email;
            Assert.IsNotNull(update.EmailAsOwid);
        }

        [TestMethod]
        public void TestUpdateEmailBadConnection()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                _badRsaConnection.Update(
                    _httpContext.Request,
                    ReturnUrl));
        }

        /// <summary>
        /// Completes a SWAN fetch storage operation without using a web 
        /// browser to return the values passed into the fetch method. Returns
        /// the encrypted result for use in decrypt operations.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetEncryptedFetchData()
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
            return (await uri.MockBrowserRedirect()).GetEncrypted(
                fetch.ReturnUrl);
        }

        private async Task<string> GetEncryptedUpdateData()
        {
            var update = _connection.Update(
                _httpContext.Request,
                ReturnUrl);
            update.Email = Email;
            update.Salt = Salt;
            update.Pref = Preference;
            var redirectUrl = await update.GetURL();
            Assert.IsNotNull(redirectUrl);
            Uri uri;
            Assert.IsTrue(Uri.TryCreate(
                redirectUrl,
                UriKind.Absolute,
                out uri));
            return (await uri.MockBrowserRedirect()).GetEncrypted(
                update.ReturnUrl);
        }
    }
}

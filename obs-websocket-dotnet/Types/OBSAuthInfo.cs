/*
    The MIT License (MIT)

    Copyright (c) 2017 Stéphane Lepin

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Newtonsoft.Json;

namespace OBSWebsocketDotNet.Types
{

    /// <summary>
    /// Data required by authentication
    /// </summary>
    public struct OBSAuthInfo
    {
        /// <summary>
        /// True if authentication is required, false otherwise
        /// </summary>
        public bool AuthRequired { get; }

        /// <summary>
        /// Authentication challenge
        /// </summary>
        public string Challenge { get; }

        /// <summary>
        /// Password salt
        /// </summary>
        public string PasswordSalt { get; }

        /// <summary>
        /// Builds the object from JSON response body
        /// </summary>

        [JsonConstructor]
        public OBSAuthInfo(bool authRequired, string challenge, string salt)
        {
            AuthRequired = authRequired;
            Challenge = challenge;
            PasswordSalt = salt;
        }
    }
}

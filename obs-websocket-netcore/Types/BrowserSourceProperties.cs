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
using Newtonsoft.Json.Linq;

namespace OBSWebsocketDotNet.Types
{
    /// <summary>
    /// BrowserSource source properties
    /// </summary>
    public struct BrowserSourceProperties
    {
        /// <summary>
        /// URL to load in the embedded browser
        /// </summary>
        public string URL { get; }

        /// <summary>
        /// true if the URL points to a local file, false otherwise.
        /// </summary>
        public bool IsLocalFile { get; }

        /// <summary>
        /// Additional CSS to apply to the page
        /// </summary>
        public string CustomCSS { get; }

        /// <summary>
        /// Embedded browser render (viewport) width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Embedded browser render (viewport) height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Embedded browser render frames per second
        /// </summary>
        public int FPS { get; }

        /// <summary>
        /// true if source should be disabled (inactive) when not visible, false otherwise
        /// </summary>
        public bool ShutdownWhenNotVisible { get; }

        /// <summary>
        /// true if source should be visible, false otherwise
        /// </summary>
        public bool Visible { get; }

        /// <summary>
        /// Construct the object from JSON response data
        /// </summary>

        [JsonConstructor]
        public BrowserSourceProperties(string url, bool is_local_file, string css, int width, int height, int fps, bool shutdown, bool render)
        {
            URL = url;
            IsLocalFile = is_local_file;
            CustomCSS = css;
            Width = width;
            Height = height;
            FPS = fps;
            ShutdownWhenNotVisible = shutdown;
            Visible = render;
        }

        /// <summary>
        /// Convert the object back to JSON
        /// </summary>
        /// <returns></returns>
        public JObject ToJSON()
        {
            return new JObject
            {
                { "url", URL },
                { "is_local_file", IsLocalFile },
                { "css", CustomCSS },
                { "width", Width },
                { "height", Height },
                { "fps", FPS },
                { "shutdown", ShutdownWhenNotVisible },
                { "render", Visible }
            };
        }
    }
}

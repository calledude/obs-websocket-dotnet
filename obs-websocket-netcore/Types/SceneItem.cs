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
    /// Describes a scene item in an OBS scene
    /// </summary>
    public struct SceneItem
    {
        /// <summary>
        /// Source name
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// Source internal type
        /// </summary>
        public string InternalType { get; }

        /// <summary>
        /// Source audio volume
        /// </summary>
        public float AudioVolume { get; }

        /// <summary>
        /// Scene item horizontal position/offset
        /// </summary>
        public float XPos { get; }

        /// <summary>
        /// Scene item vertical position/offset
        /// </summary>
        public float YPos { get; }

        /// <summary>
        /// Item source width, without scaling and transforms applied
        /// </summary>
        public int SourceWidth { get; }

        /// <summary>
        /// Item source height, without scaling and transforms applied
        /// </summary>
        public int SourceHeight { get; }

        /// <summary>
        /// Item width
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Item height
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Builds the object from the JSON scene description
        /// </summary>

        [JsonConstructor]
        public SceneItem(
            string name,
            string type,
            float volume,
            float x,
            float y,
            int source_cx,
            int source_cy,
            float cx,
            float cy)
        {
            SourceName = name;
            InternalType = type;
            AudioVolume = volume;
            XPos = x;
            YPos = y;
            SourceWidth = source_cx;
            SourceHeight = source_cy;
            Width = cx;
            Height = cy;
        }
    }
}

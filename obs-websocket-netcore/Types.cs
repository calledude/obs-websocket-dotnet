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

using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet.Enum;
using System;

namespace OBSWebsocketDotNet
{
    /// <summary>
    /// Called by <see cref="OBSWebsocket.SceneChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="newSceneName">Name of the new current scene</param>
    public delegate void SceneChangeCallback(OBSWebsocket sender, string newSceneName);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.SourceOrderChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="sceneName">Name of the scene where items where reordered</param>
    public delegate void SourceOrderChangeCallback(OBSWebsocket sender, string sceneName);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.SceneItemVisibilityChanged"/>, <see cref="OBSWebsocket.SceneItemAdded"/>
    /// or <see cref="OBSWebsocket.SceneItemRemoved"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="sceneName">Name of the scene where the item is</param>
    /// <param name="itemName">Name of the concerned item</param>
    public delegate void SceneItemUpdateCallback(OBSWebsocket sender, string sceneName, string itemName);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.TransitionChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="newTransitionName">Name of the new selected transition</param>
    public delegate void TransitionChangeCallback(OBSWebsocket sender, string newTransitionName);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.TransitionDurationChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="newDuration">Name of the new transition duration (in milliseconds)</param>
    public delegate void TransitionDurationChangeCallback(OBSWebsocket sender, int newDuration);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.StreamingStateChanged"/>, <see cref="OBSWebsocket.RecordingStateChanged"/>
    /// or <see cref="OBSWebsocket.ReplayBufferStateChanged"/> 
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="type">New output state</param>
    public delegate void OutputStateCallback(OBSWebsocket sender, OutputState type);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.StreamStatus"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="status">Stream status data</param>
    public delegate void StreamStatusCallback(OBSWebsocket sender, StreamStatus status);

    /// <summary>
    /// Called by <see cref="OBSWebsocket.StudioModeSwitched"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebsocket"/> instance</param>
    /// <param name="enabled">New Studio Mode status</param>
    public delegate void StudioModeChangeCallback(OBSWebsocket sender, bool enabled);

    /// <summary>
    /// Data of a stream status update
    /// </summary>
    public struct StreamStatus
    {
        /// <summary>
        /// True if streaming is started and running, false otherwise
        /// </summary>
        public readonly bool Streaming;

        /// <summary>
        /// True if recording is started and running, false otherwise
        /// </summary>
        public readonly bool Recording;

        /// <summary>
        /// Stream bitrate in bytes per second
        /// </summary>
        public readonly int BytesPerSec;

        /// <summary>
        /// Stream bitrate in kilobits per second
        /// </summary>
        public readonly int KbitsPerSec;

        /// <summary>
        /// RTMP output strain
        /// </summary>
        public readonly float Strain;

        /// <summary>
        /// Total time since streaming start
        /// </summary>
        public readonly TimeSpan TotalStreamTime;

        /// <summary>
        /// Number of frames sent since streaming start
        /// </summary>
        public readonly int TotalFrames;

        /// <summary>
        /// Overall number of frames dropped since streaming start
        /// </summary>
        public readonly int DroppedFrames;

        /// <summary>
        /// Current framerate in Frames Per Second
        /// </summary>
        public readonly float FPS;

        /// <summary>
        /// Builds the object from the JSON event body
        /// </summary>
        /// <param name="data">JSON event body as a <see cref="JObject"/></param>
        public StreamStatus(JObject data)
        {
            Streaming = (bool)data["streaming"];
            Recording = (bool)data["recording"];

            BytesPerSec = (int)data["bytes-per-sec"];
            KbitsPerSec = (int)data["kbits-per-sec"];
            Strain = (float)data["strain"];
            TotalStreamTime = (TimeSpan)data["stream-timecode"];

            TotalFrames = (int)data["num-total-frames"];
            DroppedFrames = (int)data["num-dropped-frames"];
            FPS = (float)data["fps"];
        }
    }

    /// <summary>
    /// Streaming settings
    /// </summary>
    public struct StreamingService
    {
        /// <summary>
        /// Type of streaming service
        /// </summary>
        public string Type;

        /// <summary>
        /// Streaming service settings (JSON data)
        /// </summary>
        public JObject Settings;
    }

    /// <summary>
    /// Common RTMP settings (predefined streaming services list)
    /// </summary>
    public struct CommonRTMPStreamingService
    {
        /// <summary>
        /// Streaming provider name
        /// </summary>
        public string ServiceName;

        /// <summary>
        /// Streaming server URL;
        /// </summary>
        public string ServerUrl;

        /// <summary>
        /// Stream key
        /// </summary>
        public string StreamKey;

        /// <summary>
        /// Construct object from data provided by <see cref="StreamingService.Settings"/>
        /// </summary>
        /// <param name="settings"></param>
        public CommonRTMPStreamingService(JObject settings)
        {
            ServiceName = (string)settings["service"];
            ServerUrl = (string)settings["server"];
            StreamKey = (string)settings["key"];
        }

        /// <summary>
        /// Convert to JSON object
        /// </summary>
        /// <returns></returns>
        public JObject ToJSON()
        {
            return new JObject
            {
                { "service", ServiceName },
                { "server", ServerUrl },
                { "key", StreamKey }
            };
        }
    }

    /// <summary>
    /// Custom RTMP settings (fully customizable RTMP credentials)
    /// </summary>
    public struct CustomRTMPStreamingService
    {
        /// <summary>
        /// RTMP server URL
        /// </summary>
        public string ServerAddress;

        /// <summary>
        /// RTMP stream key (URL suffix)
        /// </summary>
        public string StreamKey;

        /// <summary>
        /// Tell OBS' RTMP client to authenticate to the server
        /// </summary>
        public bool UseAuthentication;

        /// <summary>
        /// Username used if authentication is enabled
        /// </summary>
        public string AuthUsername;

        /// <summary>
        /// Password used if authentication is enabled
        /// </summary>
        public string AuthPassword;

        /// <summary>
        /// Construct object from data provided by <see cref="StreamingService.Settings"/>
        /// </summary>
        /// <param name="settings"></param>
        public CustomRTMPStreamingService(JObject settings)
        {
            ServerAddress = (string)settings["server"];
            StreamKey = (string)settings["key"];
            UseAuthentication = (bool)settings["use_auth"];
            AuthUsername = (string)settings["username"];
            AuthPassword = (string)settings["password"];
        }

        /// <summary>
        /// Convert to JSON object
        /// </summary>
        /// <returns></returns>
        public JObject ToJSON()
        {
            return new JObject
            {
                { "server", ServerAddress },
                { "key", StreamKey },
                { "use_auth", UseAuthentication },
                { "username", AuthUsername },
                { "password", AuthPassword }
            };
        }
    }

    /// <summary>
    /// Crop coordinates for a scene item
    /// </summary>
    public struct SceneItemCropInfo
    {
        /// <summary>
        /// Top crop (in pixels)
        /// </summary>
        public int Top;

        /// <summary>
        /// Bottom crop (in pixels)
        /// </summary>
        public int Bottom;

        /// <summary>
        /// Left crop (in pixels)
        /// </summary>
        public int Left;

        /// <summary>
        /// Right crop (in pixels)
        /// </summary>
        public int Right;
    }

    /// <summary>
    /// Thrown if authentication fails
    /// </summary>
    public class AuthFailureException : Exception
    {
    }

    /// <summary>
    /// Thrown when the server responds with an error
    /// </summary>
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public ErrorResponseException(string message) : base(message)
        {
        }
    }
}

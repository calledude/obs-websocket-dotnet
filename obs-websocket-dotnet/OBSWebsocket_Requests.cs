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
using OBSWebsocketDotNet.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBSWebsocketDotNet
{
    /// <summary>
    /// Instance of a connection with an obs-websocket server
    /// </summary>
    public partial class OBSWebsocket
    {
        /// <summary>
        /// Get the current scene info along with its items
        /// </summary>
        /// <returns>An <see cref="OBSScene"/> object describing the current scene</returns>
        public async Task<OBSScene> GetCurrentScene()
        {
            var response = await SendRequest("GetCurrentScene");
            return response.ToObject<OBSScene>();
        }

        /// <summary>
        /// Set the current scene to the specified one
        /// </summary>
        /// <param name="sceneName">The desired scene name</param>
        public async Task SetCurrentScene(string sceneName)
        {
            var requestFields = new JObject
            {
                { "scene-name", sceneName }
            };

            await SendRequest("SetCurrentScene", requestFields);
        }

        /// <summary>
        /// List every available scene
        /// </summary>
        /// <returns>A <see cref="List{OBSScene}" /> of <see cref="OBSScene"/> objects describing each scene</returns>
        public async Task<List<OBSScene>> ListScenes()
        {
            var response = await SendRequest("GetSceneList");
            return response
                .SelectTokens("$.scenes")
                .SelectMany(x => x.ToObject<OBSScene[]>())
                .ToList();
        }

        /// <summary>
        /// Change the visibility of the specified scene item
        /// </summary>
        /// <param name="itemName">Scene item which visiblity will be changed</param>
        /// <param name="visible">Desired visiblity</param>
        /// <param name="sceneName">Scene name of the specified item</param>
        public async Task SetSourceRender(string itemName, bool visible, string sceneName = null)
        {
            var requestFields = new JObject
            {
                { "source", itemName },
                { "render", visible }
            };

            if (sceneName != null)
                requestFields.Add("scene-name", sceneName);

            await SendRequest("SetSourceRender", requestFields);
        }

        /// <summary>
        /// Start/Stop the streaming output
        /// </summary>
        public async Task ToggleStreaming() => await SendRequest("StartStopStreaming");

        /// <summary>
        /// Start/Stop the recording output
        /// </summary>
        public async Task ToggleRecording() => await SendRequest("StartStopRecording");

        /// <summary>
        /// Get the current status of the streaming and recording outputs
        /// </summary>
        /// <returns>An <see cref="OutputStatus"/> object describing the current outputs states</returns>
        public async Task<OutputStatus> GetStreamingStatus()
        {
            var response = await SendRequest("GetStreamingStatus");
            return response.ToObject<OutputStatus>();
        }

        /// <summary>
        /// List all transitions
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of all transition names</returns>
        public async Task<List<string>> ListTransitions()
        {
            var response = await SendRequest("GetTransitionList");

            return response
                .SelectTokens("$.transitions[*].name")
                .Select(x => x.ToObject<string>())
                .ToList();
        }

        /// <summary>
        /// Get the current transition name and duration
        /// </summary>
        /// <returns>An <see cref="TransitionSettings"/> object with the current transition name and duration</returns>
        public async Task<TransitionSettings> GetCurrentTransition()
        {
            var respBody = await SendRequest("GetCurrentTransition");

            return respBody.ToObject<TransitionSettings>();
        }

        /// <summary>
        /// Set the current transition to the specified one
        /// </summary>
        /// <param name="transitionName">Desired transition name</param>
        public async Task SetCurrentTransition(string transitionName)
        {
            var requestFields = new JObject
            {
                { "transition-name", transitionName }
            };

            await SendRequest("SetCurrentTransition", requestFields);
        }

        /// <summary>
        /// Change the transition's duration
        /// </summary>
        /// <param name="duration">Desired transition duration (in milliseconds)</param>
        public async Task SetTransitionDuration(int duration)
        {
            var requestFields = new JObject
            {
                { "duration", duration }
            };

            await SendRequest("SetTransitionDuration", requestFields);
        }

        /// <summary>
        /// Change the volume of the specified source
        /// </summary>
        /// <param name="sourceName">Name of the source which volume will be changed</param>
        /// <param name="volume">Desired volume in linear scale (0.0 to 1.0)</param>
        public async Task SetVolume(string sourceName, float volume)
        {
            var requestFields = new JObject
            {
                { "source", sourceName },
                { "volume", volume }
            };

            await SendRequest("SetVolume", requestFields);
        }

        /// <summary>
        /// Get the volume of the specified source
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <returns>An <see cref="VolumeInfo"/> object containing the volume and mute state of the specified source</returns>
        public async Task<VolumeInfo> GetVolume(string sourceName)
        {
            var requestFields = new JObject
            {
                { "source", sourceName }
            };

            var response = await SendRequest("GetVolume", requestFields);
            return response.ToObject<VolumeInfo>();
        }

        /// <summary>
        /// Set the mute state of the specified source
        /// </summary>
        /// <param name="sourceName">Name of the source which mute state will be changed</param>
        /// <param name="mute">Desired mute state</param>
        public async Task SetMute(string sourceName, bool mute)
        {
            var requestFields = new JObject
            {
                { "source", sourceName },
                { "mute", mute }
            };

            await SendRequest("SetMute", requestFields);
        }

        /// <summary>
        /// Toggle the mute state of the specified source
        /// </summary>
        /// <param name="sourceName">Name of the source which mute state will be toggled</param>
        public async Task ToggleMute(string sourceName)
        {
            var requestFields = new JObject
            {
                { "source", sourceName }
            };

            await SendRequest("ToggleMute", requestFields);
        }

        /// <summary>
        /// Set the position of the specified scene item
        /// </summary>
        /// <param name="itemName">Name of the scene item which position will be changed</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="sceneName">(optional) name of the scene the item belongs to</param>
        public async Task SetSceneItemPosition(string itemName, float x, float y, string sceneName = null)
        {
            var requestFields = new JObject
            {
                { "item", itemName },
                { "x", x },
                { "y", y }
            };

            if (sceneName != null)
                requestFields.Add("scene-name", sceneName);

            await SendRequest("SetSceneItemPosition", requestFields);
        }

        /// <summary>
        /// Set the scale and rotation of the specified scene item
        /// </summary>
        /// <param name="itemName">Name of the scene item which transform will be changed</param>
        /// <param name="rotation">Rotation in Degrees</param>
        /// <param name="xScale">Horizontal scale factor</param>
        /// <param name="yScale">Vertical scale factor</param>
        /// <param name="sceneName">(optional) name of the scene the item belongs to</param>
        public async Task SetSceneItemTransform(string itemName, float rotation = 0, float xScale = 1, float yScale = 1, string sceneName = null)
        {
            var requestFields = new JObject
            {
                { "item", itemName },
                { "x-scale", xScale },
                { "y-scale", yScale },
                { "rotation", rotation }
            };

            if (sceneName != null)
                requestFields.Add("scene-name", sceneName);

            await SendRequest("SetSceneItemTransform", requestFields);
        }

        /// <summary>
        /// Set the current scene collection to the specified one
        /// </summary>
        /// <param name="scName">Desired scene collection name</param>
        public async Task SetCurrentSceneCollection(string scName)
        {
            var requestFields = new JObject
            {
                { "sc-name", scName }
            };

            await SendRequest("SetCurrentSceneCollection", requestFields);
        }

        /// <summary>
        /// Get the name of the current scene collection
        /// </summary>
        /// <returns>Name of the current scene collection</returns>
        public async Task<string> GetCurrentSceneCollection()
        {
            var response = await SendRequest("GetCurrentSceneCollection");
            return response
                .SelectToken("$.sc-name")
                .ToObject<string>();
        }

        /// <summary>
        /// List all scene collections
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of the names of all scene collections</returns>
        public async Task<List<string>> ListSceneCollections()
        {
            var response = await SendRequest("ListSceneCollections");

            return response
                .SelectTokens("$.scene-collections[*].sc-name")
                .Select(x => x.ToObject<string>())
                .ToList();
        }

        /// <summary>
        /// Set the current profile to the specified one
        /// </summary>
        /// <param name="profileName">Name of the desired profile</param>
        public async Task SetCurrentProfile(string profileName)
        {
            var requestFields = new JObject
            {
                { "profile-name", profileName }
            };

            await SendRequest("SetCurrentProfile", requestFields);
        }

        /// <summary>
        /// Get the name of the current profile
        /// </summary>
        /// <returns>Name of the current profile</returns>
        public async Task<string> GetCurrentProfile()
        {
            var response = await SendRequest("GetCurrentProfile");
            return response
                .SelectToken("$.profile-name")
                .ToObject<string>();
        }

        /// <summary>
        /// List all profiles
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of the names of all profiles</returns>
        public async Task<List<string>> ListProfiles()
        {
            var response = await SendRequest("ListProfiles");

            return response
                .SelectTokens("$.profiles[*].profile-name")
                .Select(x => x.ToObject<string>())
                .ToList();
        }

        // TODO: needs updating
        /// <summary>
        /// Start streaming. Will trigger an error if streaming is already active
        /// </summary>
        public async Task StartStreaming() => await SendRequest("StartStreaming");

        /// <summary>
        /// Stop streaming. Will trigger an error if streaming is not active.
        /// </summary>
        public async Task StopStreaming() => await SendRequest("StopStreaming");

        /// <summary>
        /// Start recording. Will trigger an error if recording is already active.
        /// </summary>
        public async Task StartRecording() => await SendRequest("StartRecording");

        /// <summary>
        /// Stop recording. Will trigger an error if recording is not active.
        /// </summary>
        public async Task StopRecording() => await SendRequest("StopRecording");

        /// <summary>
        /// Change the current recording folder
        /// </summary>
        /// <param name="recFolder">Recording folder path</param>
        public async Task SetRecordingFolder(string recFolder)
        {
            var requestFields = new JObject
            {
                { "rec-folder", recFolder }
            };
            await SendRequest("SetRecordingFolder", requestFields);
        }

        /// <summary>
        /// Get the path of the current recording folder
        /// </summary>
        /// <returns>Current recording folder path</returns>
        public async Task<string> GetRecordingFolder()
        {
            var response = await SendRequest("GetRecordingFolder");

            return response
                .SelectToken("$.rec-folder")
                .ToObject<string>();
        }

        /// <summary>
        /// Get duration of the currently selected transition (if supported)
        /// </summary>
        /// <returns>Current transition duration (in milliseconds)</returns>
        public async Task<int> GetTransitionDuration()
        {
            var response = await SendRequest("GetTransitionDuration");
            return (int)response["transition-duration"];
        }

        /// <summary>
        /// Get status of Studio Mode
        /// </summary>
        /// <returns>Studio Mode status (on/off)</returns>
        public async Task<bool> StudioModeEnabled()
        {
            var response = await SendRequest("GetStudioModeStatus");
            return (bool)response["studio-mode"];
        }

        /// <summary>
        /// Enable/disable Studio Mode
        /// </summary>
        /// <param name="enable">Desired Studio Mode status</param>
        public async Task SetStudioMode(bool enable)
        {
            if (enable)
                await SendRequest("EnableStudioMode");
            else
                await SendRequest("DisableStudioMode");
        }

        /// <summary>
        /// Toggle Studio Mode status (on to off or off to on)
        /// </summary>
        public async Task ToggleStudioMode() => await SendRequest("ToggleStudioMode");

        /// <summary>
        /// Get the currently selected preview scene. Triggers an error
        /// if Studio Mode is disabled
        /// </summary>
        /// <returns>Preview scene object</returns>
        public async Task<OBSScene> GetPreviewScene()
        {
            var response = await SendRequest("GetPreviewScene");
            return response.ToObject<OBSScene>();
        }

        /// <summary>
        /// Change the currently active preview scene to the one specified.
        /// Triggers an error if Studio Mode is disabled
        /// </summary>
        /// <param name="previewScene">Preview scene name</param>
        public async Task SetPreviewScene(string previewScene)
        {
            var requestFields = new JObject
            {
                { "scene-name", previewScene }
            };
            await SendRequest("SetPreviewScene", requestFields);
        }

        /// <summary>
        /// Change the currently active preview scene to the one specified.
        /// Triggers an error if Studio Mode is disabled.
        /// </summary>
        /// <param name="previewScene">Preview scene object</param>
        public async Task SetPreviewScene(OBSScene previewScene) => await SetPreviewScene(previewScene.Name);

        /// <summary>
        /// Triggers a Studio Mode transition (preview scene to program)
        /// </summary>
        /// <param name="transitionDuration">(optional) Transition duration</param>
        /// <param name="transitionName">(optional) Name of transition to use</param>
        public async Task TransitionToProgram(int transitionDuration = -1, string transitionName = null)
        {
            var requestFields = new JObject();

            if (transitionDuration > -1 || transitionName != null)
            {
                var withTransition = new JObject();

                if (transitionDuration > -1)
                    withTransition.Add("duration");

                if (transitionName != null)
                    withTransition.Add("name", transitionName);

                requestFields.Add("with-transition", withTransition);
            }

            await SendRequest("TransitionToProgram", requestFields);
        }

        /// <summary>
        /// Get if the specified source is muted
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <returns>Source mute status (on/off)</returns>
        public async Task<bool> GetMute(string sourceName)
        {
            var requestFields = new JObject
            {
                { "source", sourceName }
            };

            var response = await SendRequest("GetMute", requestFields);
            return response.SelectToken("$.muted").ToObject<bool>();
        }

        /// <summary>
        /// Toggle the Replay Buffer on/off
        /// </summary>
        public async Task ToggleReplayBuffer() => await SendRequest("StartStopReplayBuffer");

        /// <summary>
        /// Start recording into the Replay Buffer. Triggers an error
        /// if the Replay Buffer is already active, or if the "Save Replay Buffer"
        /// hotkey is not set in OBS' settings
        /// </summary>
        public async Task StartReplayBuffer() => await SendRequest("StartReplayBuffer");

        /// <summary>
        /// Stop recording into the Replay Buffer. Triggers an error if the
        /// Replay Buffer is not active.
        /// </summary>
        public async Task StopReplayBuffer() => await SendRequest("StopReplayBuffer");

        /// <summary>
        /// Save and flush the contents of the Replay Buffer to disk. Basically
        /// the same as triggering the "Save Replay Buffer" hotkey in OBS.
        /// Triggers an error if Replay Buffer is not active.
        /// </summary>
        public async Task SaveReplayBuffer() => await SendRequest("SaveReplayBuffer");

        /// <summary>
        /// Set the audio sync offset of the specified source
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <param name="syncOffset">Audio offset (in nanoseconds) for the specified source</param>
        public async Task SetSyncOffset(string sourceName, int syncOffset)
        {
            var requestFields = new JObject
            {
                { "source", sourceName },
                { "offset", syncOffset }
            };
            await SendRequest("SetSyncOffset", requestFields);
        }

        /// <summary>
        /// Get the audio sync offset of the specified source
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <returns>Audio offset (in nanoseconds) of the specified source</returns>
        public async Task<int> GetSyncOffset(string sourceName)
        {
            var requestFields = new JObject
            {
                { "source", sourceName }
            };

            var response = await SendRequest("GetSyncOffset", requestFields);
            return response
                .SelectToken("$.offset")
                .ToObject<int>();
        }

        /// <summary>
        /// Set the relative crop coordinates of the specified source item
        /// </summary>
        /// <param name="sceneItemName">Name of the scene item</param>
        /// <param name="cropInfo">Crop coordinates</param>
        /// <param name="sceneName">(optional) parent scene name of the specified source</param>
        public async Task SetSceneItemCrop(string sceneItemName,
            SceneItemCropInfo cropInfo, string sceneName = null)
        {
            var requestFields = new JObject();

            if (sceneName != null)
                requestFields.Add("scene-name");

            requestFields.Add("item", sceneItemName);
            requestFields.Add("top", cropInfo.Top);
            requestFields.Add("bottom", cropInfo.Bottom);
            requestFields.Add("left", cropInfo.Left);
            requestFields.Add("right", cropInfo.Right);

            await SendRequest("SetSceneItemCrop", requestFields);
        }

        /// <summary>
        /// Set the relative crop coordinates of the specified source item
        /// </summary>
        /// <param name="sceneItem">Scene item object</param>
        /// <param name="cropInfo">Crop coordinates</param>
        /// <param name="scene">Parent scene of scene item</param>
        public async Task SetSceneItemCrop(SceneItem sceneItem,
            SceneItemCropInfo cropInfo, OBSScene scene) => await SetSceneItemCrop(sceneItem.SourceName, cropInfo, scene.Name);

        /// <summary>
        /// Get names of configured special sources (like Desktop Audio
        /// and Mic sources)
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetSpecialSources()
        {
            var response = await SendRequest("GetSpecialSources");
            var sources = response.ToObject<Dictionary<string, string>>();
            sources.Remove("request-type");
            sources.Remove("message-id");

            return sources;
        }

        /// <summary>
        /// Set current streaming settings
        /// </summary>
        /// <param name="service"></param>
        /// <param name="save"></param>
        public async Task SetStreamingSettings(StreamingService service, bool save)
        {
            var requestFields = new JObject
            {
                { "type", service.Type },
                { "settings", service.Settings },
                { "save", save }
            };
            await SendRequest("SetStreamSettings", requestFields);
        }

        /// <summary>
        /// Get current streaming settings
        /// </summary>
        /// <returns></returns>
        public async Task<StreamingService> GetStreamSettings()
        {
            var response = await SendRequest("GetStreamSettings");

            var service = new StreamingService
            {
                Type = (string)response["type"],
                Settings = (JObject)response["settings"]
            };

            return service;
        }

        /// <summary>
        /// Save current Streaming settings to disk
        /// </summary>
        public async Task SaveStreamSettings() => await SendRequest("SaveStreamSettings");

        /// <summary>
        /// Get settings of the specified BrowserSource
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <param name="sceneName">Optional name of a scene where the specified source can be found</param>
        /// <returns>BrowserSource properties</returns>
        public async Task<BrowserSourceProperties> GetBrowserSourceProperties(string sourceName, string sceneName = null)
        {
            var request = new JObject
            {
                { "source", sourceName }
            };
            if (sceneName != null)
                request.Add("scene-name", sceneName);

            var response = await SendRequest("GetBrowserSourceProperties", request);

            return response.ToObject<BrowserSourceProperties>();
        }

        /// <summary>
        /// Set settings of the specified BrowserSource
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <param name="props">BrowserSource properties</param>
        /// <param name="sceneName">Optional name of a scene where the specified source can be found</param>
        public async Task SetBrowserSourceProperties(string sourceName, BrowserSourceProperties props, string sceneName = null)
        {
            var request = new JObject
            {
                { "source", sourceName }
            };
            if (sceneName != null)
                request.Add("scene-name", sourceName);

            request.Merge(props.ToJSON(), new JsonMergeSettings()
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });

            await SendRequest("SetBrowserSourceProperties", request);
        }
    }
}

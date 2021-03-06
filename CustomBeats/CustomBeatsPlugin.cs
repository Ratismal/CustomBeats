using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using FMOD;
using FMODUnity;
using HarmonyLib;
using JetBrains.Annotations;
using Rhythm;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace CustomBeats
{
    [BepInPlugin("me.stupidcat.plugins.custombeats", "Custom Beats", "1.0.0")]
    public class CustomBeatsPlugin : BaseUnityPlugin
    {
        public static CustomBeatsPlugin INSTANCE;
        public static ManualLogSource LOGGER;

        public bool dirty = false;
        public List<BeatmapIndex.Song> songs;

        private Harmony harmony;

        private ConfigEntry<KeyboardShortcut> Refresh { get; set; }

        public CustomBeatsPlugin()
        {
            Refresh = Config.Bind("Hotkeys", "Refresh", new KeyboardShortcut(KeyCode.F5));
        }

        void Awake()
        {
            CustomBeatsPlugin.INSTANCE = this;
            CustomBeatsPlugin.LOGGER = this.Logger;

            Logger.LogInfo("Helloooooooo, world!");
            Logger.LogInfo(Application.streamingAssetsPath);

            harmony = Harmony.CreateAndPatchAll(typeof(Patches));

            LoadSongs();
        }

        void LoadSongs()
        {
            songs = new List<BeatmapIndex.Song>();
            List<string> difficulties = new List<string>
            {
                "Beginner",
                "Easy",
                "Normal",
                "Regular",
                "Hard",
                "UNBEATABLE",
                "Trailer",
                "Tutorial"
            };

            string customSongDir = $"{Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'))}/CustomBeats/Songs/";
            if (!Directory.Exists(customSongDir)) Directory.CreateDirectory(customSongDir);
            Logger.LogInfo($"CustomBeats Song Directory: {customSongDir}");

            string[] songDirs = Directory.GetDirectories(customSongDir);
            foreach (var dir in songDirs)
            {
                string songName = Path.GetFileNameWithoutExtension(dir);
                Logger.LogInfo("Loading song " + songName);
                bool hasMaps = false;

                string stageScene = "";
                List<BeatmapInfo> beatmaps = new List<BeatmapInfo>();

                string[] osuFiles = Directory.GetFiles(dir, "*.osu");
                foreach (var osuFile in osuFiles)
                {
                    string beatmapName = Path.GetFileNameWithoutExtension(osuFile);
                    string content = File.ReadAllText(osuFile);
                    TextAsset asset = new TextAsset(content);
                    asset.name = beatmapName;
                    var difficultyMatch = Regex.Match(content, "Version: *(.+?)\r?\n");
                    string difficulty = difficultyMatch.Groups[1].Value;

                    var titleMatch = Regex.Match(content, "Title: *(.+?)\r?\n");
                    string title = titleMatch.Groups[1].Value;
                    var clipMatch = Regex.Match(content, "AudioFilename: *(.+?)\r?\n");
                    var clip = clipMatch.Groups[1].Value;

                    Logger.LogInfo(" - Loading beatmap " + beatmapName + " (" + difficulty + "): " + clip);

                    CustomBeatmapInfo beatmap = new CustomBeatmapInfo(asset, songName, difficulty, clip);

                    if (!difficulties.Contains(difficulty))
                    {
                        Logger.LogInfo(" - Adding new difficulty " + difficulty);
                        difficulties.Add(difficulty);
                    }

                    beatmaps.Add(beatmap);
                }

                if (beatmaps.Count > 0)
                {
                    beatmaps.Sort((BeatmapInfo x, BeatmapInfo y) => difficulties.IndexOf(x.difficulty) - difficulties.IndexOf(y.difficulty));

                    var song = new CustomBeatsSong(songName);
                    song.SetBeatmaps(beatmaps);


                    songs.Add(song);
                }
            }

            dirty = true;
        }

        void Update()
        {
            if (Refresh.Value.IsDown())
            {
                Logger.LogInfo("Refreshing!");
                LoadSongs();
            }
        }

        void OnDestroy()
        {
            Logger.LogInfo("Goodbye, world!");
            harmony.UnpatchSelf();
        }
    }
}

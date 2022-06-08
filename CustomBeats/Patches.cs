using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using HarmonyLib;
using Rhythm;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CustomBeats
{
    class Patches
    {
        public static List<BeatmapIndex.Song> originalSongs;

        private static List<string> originalMenuSongs;

        [HarmonyPatch(typeof(BeatmapIndex), "OnAfterDeserialize")]
        [HarmonyPrefix]
        static void BeatmapIndexUpdateCache(BeatmapIndex __instance, ref List<BeatmapIndex.Song> ___songs)
        {
            CustomBeatsPlugin.LOGGER.LogInfo("Loading songs in OnAfterDeserialize");

            if (originalSongs == null)
            {
                originalSongs = new List<BeatmapIndex.Song>();
                foreach (var song in ___songs)
                {
                    if (CustomBeatsPlugin.INSTANCE.songs.Find(s => s.name == song.name) == null)
                    {
                        originalSongs.Add(song);
                    }
                }
            }
            
            List<BeatmapIndex.Song> newSongs = new List<BeatmapIndex.Song>();

            CustomBeatsPlugin.LOGGER.LogInfo("Original Songs: " + originalSongs.Count + " | New Songs: " + CustomBeatsPlugin.INSTANCE.songs.Count);

            foreach (var song in originalSongs)
            {
                newSongs.Add(song);
            }

            foreach (var song in CustomBeatsPlugin.INSTANCE.songs)
            {
                newSongs.Add(song);
            }

            CustomBeatsPlugin.LOGGER.LogInfo("We now have " + newSongs.Count + " songs.");

            ___songs = newSongs;
        }

        [HarmonyPatch(typeof(WhiteLabelMainMenu), "Start")]
        [HarmonyPostfix]
        static void WhiteLabelMainMenuStart(ref WrapCounter ___songsInc, ref List<string> ___songs)
        {
            if (originalMenuSongs == null)
            {
                originalMenuSongs = ___songs;
            }

            ___songs = originalMenuSongs;

            foreach (var song in CustomBeatsPlugin.INSTANCE.songs)
            {
                ___songs.Add(song.name);
            }

            CustomBeatsPlugin.LOGGER.LogInfo("[START] Song count: " + ___songs.Count);

            ___songsInc = new WrapCounter(___songs.Count);
        }

        [HarmonyPatch(typeof(WhiteLabelMainMenu), "LevelSelect")]
        [HarmonyPrefix]
        static void WhiteLabelMainMenuLevelSelect(ref WrapCounter ___songsInc, ref List<string> ___songs, BeatmapIndex ___beatmapIndex)
        {
            if (CustomBeatsPlugin.INSTANCE.dirty)
            {
                foreach (var song in CustomBeatsPlugin.INSTANCE.songs)
                {
                    if (!___songs.Contains(song.name))
                    {
                        ___songs.Add(song.name);
                    }
                }
                CustomBeatsPlugin.LOGGER.LogInfo("[LevelSelect] Song count: " + ___songs.Count);

                ___songsInc = new WrapCounter(___songs.Count);
                CustomBeatsPlugin.INSTANCE.dirty = false;

                ___beatmapIndex.OnAfterDeserialize();
            }
        }

        
        [HarmonyPatch(typeof(BeatmapParser), "ParseBeatmap", new Type[] {} )]
        [HarmonyPostfix]
        static void WhiteLabelMainMenDASDSADSADt(BeatmapParser __instance)
        {
            BeatmapInfo beatmapInfo = __instance.beatmapIndex.FindByPath(__instance.beatmapPath);

            if (beatmapInfo is CustomBeatmapInfo)
            {
                __instance.audioKey = ((CustomBeatmapInfo) beatmapInfo)._audioKey;
            }
        }

        [HarmonyPatch(typeof(RhythmTracker), "PreloadFromTable", new Type[] { typeof(string) })]
        [HarmonyPostfix]
        static void PreloadPatch(RhythmTracker __instance, string key, ref EventInstance ___instance)
        {
            if (key.StartsWith("CustomBeats"))
            {
                if (___instance.isValid())
                {
                    // Needs to be ALLOWFADEOUT or it just doesn't work for some reason?
                    // Thus, cannot use __instance.StopAndRelease
                    // I spent 2 hours figuring this out >.>
                    ___instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ___instance.release();
                }
                
                __instance.Preload(PlaySource.FromFile, key);
            }
        }

        [HarmonyPatch(typeof(WhiteLabelMainMenu), "PlaySongPreview", new Type[] { typeof(string) })]
        [HarmonyPostfix]
        static void PlaySongPreviewPatch(RhythmTracker __instance, string audioPath, ref EventInstance ___songPreviewInstance, ref EventReference ___songPreviewEvent)
        {
            var song = CustomBeatsPlugin.INSTANCE.songs.Find(s => s.name == audioPath);

            if (song != null)
            {
                var info = song.Beatmaps.First().Value;
                var path = (info as CustomBeatmapInfo)._audioKey;

                if (___songPreviewInstance.isValid())
                {
                    ___songPreviewInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ___songPreviewInstance.release();
                }
                ___songPreviewInstance = RuntimeManager.CreateInstance(___songPreviewEvent);
                RhythmTracker.PrepareInstance(___songPreviewInstance, PlaySource.FromFile, path);
                ___songPreviewInstance.setPitch(JeffBezosController.songSpeed);
                ___songPreviewInstance.start();
            }
        }
    }
}

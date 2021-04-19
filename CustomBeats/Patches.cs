using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FMOD;
using FMOD.Studio;
using HarmonyLib;
using Rhythm;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CustomBeats
{
    class Patches
    {
        private static string[] originalSongs;
        private static BeatmapInfo[] originalBeatmapInfos;
        private static string[] originalDifficulties;

        private static List<string> originalMenuSongs;

        [HarmonyPatch(typeof(BeatmapIndex), "UpdateCache")]
        [HarmonyPrefix]
        static void BeatmapIndexUpdateCache(BeatmapIndex __instance, Dictionary<string, BeatmapInfo> ___cachedByName, Dictionary<string, List<BeatmapInfo>> ___cachedBySong, Dictionary<string, BeatmapInfo> ___cachedByPath)
        {
            if (originalSongs == null)
            {
                originalSongs = __instance.songNames;
                originalBeatmapInfos = __instance.beatmaps;
                originalDifficulties = __instance.difficulties;
            }

            __instance.songNames = originalSongs.Concat(CustomBeatsPlugin.INSTANCE.songs).ToArray();
            __instance.beatmaps = originalBeatmapInfos.Concat(CustomBeatsPlugin.INSTANCE.beatmaps).ToArray();
            __instance.difficulties = originalDifficulties.Concat(CustomBeatsPlugin.INSTANCE.difficulties).ToArray();

            foreach (var beatmap in __instance.beatmaps)
            {
                if (!___cachedByName.ContainsKey(beatmap.name))
                {
                    ___cachedByName.Add(beatmap.name, beatmap);
                }
                if (!___cachedBySong.ContainsKey(beatmap.songName))
                {
                    ___cachedBySong.Add(beatmap.songName, new List<BeatmapInfo>() { beatmap });
                }
                string path = beatmap.songName + "/" + beatmap.difficulty;
                if (!___cachedByPath.ContainsKey(path))
                {
                    ___cachedByPath.Add(path, beatmap);
                }
            }
        }

        [HarmonyPatch(typeof(BeatmapInfo), "audioKey", MethodType.Getter)]
        [HarmonyPrefix]
        static bool BeatmapInfoAudioKey(BeatmapInfo __instance, ref string __result)
        {
            if (__instance is CustomBeatmapInfo)
            {
                __result = ((CustomBeatmapInfo) __instance)._audioKey;
                return false;
            }
            return true;
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
                ___songs.Add(song);
            }

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
                    if (!___songs.Contains(song))
                    {
                        ___songs.Add(song);
                    }
                }

                ___songsInc = new WrapCounter(___songs.Count);
                CustomBeatsPlugin.INSTANCE.dirty = false;

                ___beatmapIndex.UpdateCache();
            }
        }

        [HarmonyPatch(typeof(BeatmapParser), "ParseBeatmap")]
        [HarmonyPostfix]
        static void WhiteLabelMainMenDASDSADSADt(BeatmapParser __instance)
        {
            // I don't know why, but if this patch doesn't exist it doesn't detect beatmapInfo as a CustomBeatmapInfo class.
            // So I guess I'm just leaving it here?
            BeatmapInfo beatmapInfo = __instance.beatmapIndex.FindByPath(__instance.beatmapPath);
            // CustomBeatsPlugin.LOGGER.LogInfo(beatmapInfo.name);

            if (beatmapInfo is CustomBeatmapInfo)
            {
                // CustomBeatsPlugin.LOGGER.LogInfo("I am losing my mind.");
            }
            else
            {
                // CustomBeatsPlugin.LOGGER.LogInfo("I am losing my mind even more.");
            }
        }
        
    }
}

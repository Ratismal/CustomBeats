﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhythm;
using UnityEngine;

namespace CustomBeats
{
    class CustomBeatmapInfo : BeatmapInfo
    {
        public string _audioKey;
        public CustomBeatmapInfo(TextAsset textAsset, string songName, string difficulty, string clipName) : base(textAsset, songName,
            difficulty)
        {
            _audioKey = "../../CustomBeats/Songs/" + songName + "/" + clipName;
        }
    }
}

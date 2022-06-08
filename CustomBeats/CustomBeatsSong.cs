using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Rhythm;

namespace CustomBeats
{
    public class CustomBeatsSong : BeatmapIndex.Song
    {
        public CustomBeatsSong(string name) : base(name)
        {
            stageScene = "TrainStationRhythm";
        }

        public void SetBeatmaps(List<BeatmapInfo> beatmaps)
        {
            var t = new Traverse(this);

            t.Field("beatmaps").SetValue(beatmaps);

            this.OnAfterDeserialize();
        }
        
    }
}

# Creating a Custom Beatmap

UNBEATABLE uses the osu file format v14. To create a beatmap, it's recommended to use osu client's built-in beatmap editor. 

osu! has guides for how to create beatmaps, so I won't get into detail about that here. Rather, I'll describe the specific details to keep in mind for how UNBEATABLE processes beatmaps.

## Basic Settings

Your beatmap should be on the `osu!mania` mode.

Under difficulty, set "Key Count" to 6.

The standard difficulties that UNBEATABLE has are:
- Beginner
- Easy
- Normal
- Hard
- UNBEATABLE

Where possible, you should comply to these standards. However, CustomBeats will load additional difficulties that you may provide.

You do not need to provide every single difficulty for your beatmap, but you do need to provide at least one (otherwise, you'll have no map!).

## Types of Beats

### osu!mania

You will have a piano roll consisting of 6 different columns. Each column has a specific meaning. For reference, I will refer to the columns as:

| Column: |  1  |  2  |  3  |  4  |  5  |  6  |
| :-----: | :-: | :-: | :-: | :-: | :-: | :-: |
| Type: | Command | Command | Top | Bottom | Flip | Extra |

Columns 3 and 4 are also considered to be "Normal", and I'll be refering to them as such.

I am unclear on what "Command" columns are for.

There are two kinds of beats that you can place in osu!mania:
- Instant
- Hold

And three kinds of modifiers you can assign:
- Whistle
- Finish
- Clap

### UNBEATABLE

On UNBEATABLE's side, we currently have the following types of beats.

| Beat Name | Column | Type | Modifier | Description | 
| --------- | ------ | ---- | -------- | ----------- |
| Default   | Normal | Instant | None/Clap| This is the standard beat that's on the top or bottom row.<br>If Clap is used, will hide before reaching the player.|
| Dodge     | Normal | Instant | Whistle | This is a spike that the player has to avoid
| Setpiece | Normal | Instant | Finish | Not sure what this is. Possibly a leftover beat from testing or debugging? In any case, you can't hit it, can't dodge it, and passing it instantly fails the song. Do not use. |
| Hold | Normal | Hold | None | This is the standard held beat that's on the top or bottom row. |
| Double | Normal | Hold | Whistle | This is a beat that you hit on the current row, and then hit again on the other row once it reaches the end of the held beat. <br>It acts weirdly if you have other Default beats at the same time, so use caution.<br>If Clap is used, will hide before reaching the player. |
| Flip | Flip | Instant | None | This beat will cause the current view to flip from Left to Right, or vice versa. |
| Zoom | Flip | Instant | Whistle | This beat will cause the current view to zoom in or out. |
| Freestyle | Extra | Instant | None | This is the beat that appears in the middle of the track, and can be hit with either top or bottom attacks. |
| Spam | Extra | Hold | Finish | This is the beat that takes up the entire height of the track, that you just keymash to complete. |

## Tips and Tricks

Here are some tips and tricks for when you're making your beatmaps.

### Hot Reloading

To test your track, it's best to run it in UNBEATABLE. When you do an update you want to test, you can 
1. Pause the track in UNBEATABLE (if currently running)
1. copy the updated .osu file into your CustomBeats/Songs folder
1. press `F5` (by default) to reload CustomBeats
1. restart your track.

Your changes will be automatically applied!

### Restart Button

Due to leftovers from [WHITE LABEL], the "RESTART" button will only appear in the pause menu after the track has started, and before the last note of the track. Otherwise, the "RESTART" button will be replaced with a "SAVE" button that does nothing.

To get around this, you can place a note at the very end of your beatmap which will always give you access to the "RESTART" button! Make sure to remove it once you're done.

## Example .osu file

```
osu file format v14

[General]
AudioFilename: audio.mp3
AudioLeadIn: 0
PreviewTime: -1
Countdown: 0
SampleSet: Soft
StackLeniency: 0.7
Mode: 3
LetterboxInBreaks: 0
SpecialStyle: 0
WidescreenStoryboard: 1

[Editor]
DistanceSpacing: 0.8
BeatDivisor: 4
GridSize: 4
TimelineZoom: 1

[Metadata]
Title:My Cool Song
TitleUnicode:My Cool Song
Artist:Artist's Name
ArtistUnicode:Artist's Name
Creator:Your Name
Version:Beginner
Source:
Tags:
BeatmapID:0
BeatmapSetID:-1

[Difficulty]
HPDrainRate:5
CircleSize:6
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1.4
SliderTickRate:1

[Events]
//Background and Video events
//Break Periods
//Storyboard Layer 0 (Background)
//Storyboard Layer 1 (Fail)
//Storyboard Layer 2 (Pass)
//Storyboard Layer 3 (Foreground)
//Storyboard Layer 4 (Overlay)
//Storyboard Sound Samples

[TimingPoints]
0,500,4,2,0,100,1,0


[HitObjects]
```
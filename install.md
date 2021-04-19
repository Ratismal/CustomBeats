# Installing CustomBeats

CustomBeats relies on [BepInEx](https://github.com/BepInEx/BepInEx), a Unity and XNA modloader.

## Installing BepInEx

You can look at BepInEx's [official install guide](https://bepinex.github.io/bepinex_docs/master/articles/user_guide/installation/index.html?tabs=tabid-win), or check out the abridged version below.

1. Download the latest BepInEx release [here](https://github.com/BepInEx/BepInEx/releases)
  - `BepInEx_x64_X.X.X.X.zip`
2. Extract the contents of `BepInEx_x64_X.X.X.X.zip` in your UNBEATABLE game directory, so that the `BepInEx` folder is in the same place as `UNBEATABLE.exe`
3. Run `UNBEATABLE.exe`. This will allow BepInEx to generate the files and folders it needs. To confirm
4. (optional) Open the `BepInEx/config/BepInEx.cfg` file, and enable `[Logging.Console]`. This will open a console containing runtime logs, which can be useful if you run into issues with CustomBeats.

## Installing CustomBeats

1. Download the latest Custombeats release [here](https://github.com/ratismal/CustomBeats/releases)
2. Extract the contents in your UNBEATABLE game directory, so that `BepinEx/plugins/CustomBeats/CustomBeats.dll` exists.
3. Run `UNBEATABLE.exe`. CustomBeats will generate a `CustomBeats` folder next to `UNBEATABLE.exe`.

## Adding a Custom Beatmap

1. Navigate to `CustomBeats/Songs`
2. Copy over any custom beatmap folder that you desire.
3. Restart the game, or press `F5` (by default) to hot reload custom beatmaps.

A beatmap's folder should be in the following structure:
```
"Song Name"
  -> audio.mp3
  -> Artist - song name (mapper) [Difficulty].osu
```

Please be aware that custom beatmaps may contain copywritten content. CustomBeats is not responsible for the distribution of any copywritten content, and cannot be held accountable.

## Creating a Custom Beatmap

Please refer to the [creation guide](creation.md).
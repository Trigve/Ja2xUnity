# Ja2xUnity

This project is an attempt to rewrite the `Jagged Alliance 2` in Unity engine but doing it Unity way (that is using
scenes, prefabs, ...). It isn't 1:1 rewrite. What it means is that some pre-processing of the original game files is 
needed. You will therefor need the original files from the `Jagged Alliance 2` (which you can buy on the GOG or
the Steam).

# Dependencies

Because of original file pre-processing, you'll need the applications and libraries listed below:  
- __`Unity 6.0.x`__ - The current supported version could be found in `ProjectSettings/ProjectVersion.txt`. Try to use
only this version, as it is guratanteed to be working.
- __`Aspid.MVVM`__ - Get the package from https://github.com/VPDPersonal/Aspid.MVVM and import it to the project
(`Assets/Plugins`)
- __`ffmpeg`__ - For converting the `.smk` (SMACK) videos. Path to the executable is set in `Ja2DevSettings` (see
below)

# Asset workflow

The base game could be build in the Unity without any assets. So you don't need the original files if you just want
to build the player (without asset bundles). But to be able to play the game, you'll need tho build the asset bundles
from the original content. Next are the steps for setting up the enviroment and building the asset bundles.

1. Create the devel settings (`Ja2DevSettings.asset`). Right click on the `Assets/Settings` path, then
`Create` -> `JA2 Devel Settings`. In devel settings, set the various dirs:
   - `Input Dir` - The directory, where the original `.slf` files are located
   - `Data Dir` - Should be `Assets/Data`
   - `Slf extract dir` - Should be `Assets/Data/ja2`
   - `User dir` - Should be `Assets/StreamingAssets`
   - `Bin dir` - Directory, where the external binaries are located (i.e. ffmpeg, ...)
   - `Bundle export dir` - Directory, where the asset bundles will be exported. Default is `Assets/StreamingAssets/bundles`
   - `Use Asset Bundles` - If chcecked, even in the editor, the asset bundles will be used (asset bundles must be built
already to be able to use this option)
![](doc/img/Ja2DevSettings.png)

![](doc/img/Ja2DevSettingsPanel.png)

## Extracting assets

In the menu click the `JA2` -> `Extract SLF`. The Unity will extract the files from all the `.slf` in the specified
paths. After the extraction is done, you can review the assets if everything was extracted correctly. The extracted
asset are located in the `Slf extract dir` of the devel settings. You could change some importer attributes per-asset
with with `Ja2ImportSettings` asset. There for each directory, you could add files and specify some defaults.

![](doc/img/ImportSettings1.png)

### Fonts

After extracting the fonts, one need to run `JA2` -> `Post-process Fonts` from menu, to generate default font classes
(`AssetFontClass`). You could also do it manually if needed (using Asset creation menu).

## AssetBundle build

In the menu click the `JA2` -> `Build Asset Bundles`. The dialog will be shown to select which bundles should be
created and various other options. You must also set the player options in project settings to match the player
(for instance IL2Cpp VS Mono):
   - `Debug build` - Build the "debug" asset bundles with no compression

![](doc/img/ExtractBundles.png)

Copy the generated `*.bundle` files to the `StreamingAssets` dir of the player and game now should run with the
assets from the asset bundles. You could download the current version of the player from the releases.

Dev blog: [https://ja2-3d.blogspot.com/](https://ja2-3d.blogspot.com/)
# Shelley

A character creator for Synty Studio's `POLYGON - Modular Fantasy Hero Characters`. Save prefabs of the characters you make.
A config" option is also available, but requires scripting to load.

![demo gif](docs/demo.gif)

Tested on Unity 2019.3.10f1 Personal

## First-Time Setup
1. Clone the repo and open it in Unity
1. Install the required assets:
    * [POLYGON - Modular Fantasy Hero Characters](https://assetstore.unity.com/packages/3d/characters/humanoids/polygon-modular-fantasy-hero-characters-143468) (tested with version 1.22)
    * TextMesh Pro (via the package manager)
1. Upgrade the Synty shaders, following `PolygonFantasyHeroCharacters/RenderPipeline_ReadMe`
1. Remove the `PolygonFantasyHeroCharacters/Scripts` directory
1. Move `Assets/polygonFantasyHeroCharacters/Prefabs/Characters_ModularParts_Static` to `Assets/Resources/Characters_ModularParts_Static`
1. Open Shelley/Scenes/Designer. A window should open asking you to import Text Mesh Pro files. Exit play mode, and import the files.
1. The designer is now ready to use. Note that the UI is built around a 1080p resolution.

## TODO
* Rotate the character & parts
* Change the camera position
* Maybe obey head attachment & head element rules
* Smoother/automated first time setup
* Merge the output character into a single mesh

## License

Unless otherwise noted, all code is subject to the MIT license, as specified in the [LICENSE](LICENSE).

Code in the `UnityUIExtensions` directory is from the [UnitiyUIExtensions project](https://bitbucket.org/UnityUIExtensions/unity-ui-extensions) and is minimally modified by me at most.


## Questions?

DM me @mahercbeaucoup on the [Synty discord](https://discord.gg/7vV5dUK)

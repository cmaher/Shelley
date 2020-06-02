# Shelley

A character creator for Synty Studio's `POLYGON - Modular Fantasy Hero Characters`

## First-Time Setup
1. Clone the repo and open it in Unity
1. Install the required assets:
    * [POLYGON - Modular Fantasy Hero Characters](https://assetstore.unity.com/packages/3d/characters/humanoids/polygon-modular-fantasy-hero-characters-143468)
    * [Proedural UI Image](https://assetstore.unity.com/packages/tools/gui/procedural-ui-image-52200)
    * TextMesh Pro (via the package manager)
1. Upgrade the Synty shaders, following `PolygonFantasyHeroCharacters/RenderPipeline_ReadMe`
1. Remove the `PolygonFantasyHeroCharacters/Scripts` directory
1. Update the references in Shelley/Scripts/ShelleyStudio/ShelleyStudioAssembly.asmdef
    * ./Assets/ProceduralUIImage/ProceduralUIImageAssembly.asmdef (create this first)
    * Unity.TextMeshPro (should be available after installing TextMeshPro)

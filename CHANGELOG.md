## [1.1.1] - 2025-07-14
## MusicManager fix
- MusicManager: crossfade attribute in play method now overrides crossfade variable completely

## [1.1.0] - 2025-07-10
## Small updates and more control over cached emitters in the sound builder.
- Support for looping playlist in MusicManager
- Option to disable crossfade in MusicManager
- Added a cache emitter variable to the sound builder for more control over the last emitter started by the builder.
- Easier to control looping sounds. (use a specific builder for each loop sound and control using the cache emitter)
- Added option to create an emitter with a specific parent.
- Emitters now play the stop sound function if they are disabled.
- Added action event on Emitter stop.
- Option to not use don't destroy on managers.

## [1.0.0] - 2024-09-06
## First Release
- Music Library and Sound Library scriptable created to hold reference of all sounds that can be played in the game.
add them in the respective managers.
- Created sound entries and music entries scriptable to be added to the sound library and music library respectively 
for each sound
- Added an entry generator for both music and sound, so it's possible to create several entries at once.
- Static class to instantiate both managers before load if it's located in the /resources folder.
- Manager prefabs added to prefabs folder, libraries / entries generator added to scriptable folder.
- UI Documents (UI toolkit) for libraries and entries inspector added.
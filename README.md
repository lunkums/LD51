<p align="center">
  <a href="https://www.youtube.com/watch?v=9LwGNk8jlZw"><img src="./docs/images/title.png" alt="The words SHOTGUN DERBY in pixel art lettering"/></a>
</p>



## About

SHOTGUN DERBY is an arena shooter where you mow down zombies with a shotgun and lob grenades to explode them to bits.  Every 10 seconds, a new wave of zombies spawns as the horde grows progressively larger. I didn’t have enough time to make coins useful or to add a dynamic arena, but I was able to spend a lot of time polishing the core gameplay.

The game was made in 48 hours for the Ludum Dare 51 game jam using MonoGame. I hope you enjoy it!

<p align="center">
  <img src="./docs/images/coverplusplus.png" alt="The words SHOTGUN DERBY in pixel art lettering"/>
</p>

## Controls

- Move: WASD
- Shoot: Left Mouse Click
- Grenade: Right Mouse Click
- Select: Enter / Left Mouse Click
- Volume up/down: +/-
- Exit: Escape

## Deploy Instructions

Replace `TARGET_PLATFORM` with your target platform (i.e. win-x64, linux-x64, or osx-x64) then run the following command in the .NET command line at the root directory (where the .sln lives):

`dotnet publish -c Release -r TARGET_PLATFORM /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained`

See the [MonoGame documentation](https://docs.monogame.net/articles/packaging_games.html) for additional details on building an application bundle for OSX.

## TODO

- Make money useful
  - Make grenades purchasable (and give them a cooldown)
- ~~Prevent shooting and clicking when the game window is out of focus~~
- Create walls for the arena
- Add music
- Make the arena dynamic
- Add screen shake
- Add other weapons

## Credits

- JROB774, "Sharp Retro Font." opengameart.org, https://opengameart.org/content/sharp-retro-font.
- Julian Höltge, Debugging and localization. https://bytinggames.com/.
- dasBUTCHER84, "SXP_SHOTGUN_RACK_01." freesound, https://freesound.org/people/dasBUTCHER84/sounds/449614/.
- Paracetamol, "Grenade pin pull then throw - sound effect." YouTube, https://www.youtube.com/watch?v=GLPefnYsrdU.

### Extras

If you want to mess around with some of the settings, go to the settings folder and open “settings.json.”


10/15/2019
For the Dialogue I'm following a tutorial from here: https://www.youtube.com/watch?v=_nRzoTzeyxU
Animation tutorial here: https://www.youtube.com/watch?v=hkaysu1Z-N8&t=641s

11/2/2019
Sorry for the late entry, but for today I'm implementing the hazards. for hazards I'm using a looping invoke that calls an on off switch essentially.
to do this though, ill need to change the tag of the object so the player can pass through unimpeded by damage, etc.
I turn on and off the layer as a whole to make the images disappear, which is nice for letting the player know that it's safe

I ended up using a array sweep and checking if each tile has a non null sprite, if it has a sprite
it changes it. Trying to fix merge issues

11/12/2019
I changed up the hit boxes of the player for both physics and for the hitbox of hazards, it's a little bit smaller so the player can feel better traversing
the stage.

11/16/2019
Organized the scripts and added menubuttons for a start, how to, and pause menu
Using this tutorial here for hiding and making the pause menu appear: https://www.sitepoint.com/adding-pause-main-menu-and-game-over-screens-in-unity/

To make an object a hazard:
1. give it the Hazard Tag
2. Place it on the Hazard Layer
3. If you want a fancy hazard, please attach one of the Hazard Behavior scripts to it

To make a Timer Hazard
1. make normal hazard
2. add the environment_hazard_timer script
3. set timer delay

To make an object a collectable:
1. Give it the Collectable Tag
2. Place it on the Collectable Layer
3. Make sure it has a collider, box2D or tileMap work well
4. don't forget to add the Collectable Script to it.

To make a door object:
1. Give it the Door Tag
2. Give it the Door Layer
3. Attach the Door Behavior Script to it
4. Make sure it has a collider, box2D or tileMap work well

To make an NPC:
1. Make a sprite or other object
2. Give it the interact Tag
3. Place on Interact Layer
4. Drag and drop the Dialogue Trigger from Scripts to the sprite
5. Add dialogue to the Queue
6. Make sure it has a collider

To make a TileMap animate:
1. make your tilemap and add a base tile for where you want animated
2. add the AnimateTileMap Script
3. add a number of frames to the TileSet Array
4. click and drag your frames to each item
5. set an animation speed, this is measuring in seconds, so the lower the faster it'll go
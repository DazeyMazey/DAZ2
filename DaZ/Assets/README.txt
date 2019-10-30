
10/15/2019
For the Dialogue I'm following a tutorial from here: https://www.youtube.com/watch?v=_nRzoTzeyxU

To make an object a hazard:
1. give it the Hazard Tag
2. Place it on the Hazard Layer

To make an object a collectable:
1. Give it the Collectable Tag
2. Place it on the Collectable Layer

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
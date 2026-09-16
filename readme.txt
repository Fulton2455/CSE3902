readme
My program opens up a window that allows a player to move around within it.
The player has acceleration built in and is able to build velocity in 8 directions cardinals and their 45 degree angles until max speedos reached this should go from 0-100 in .2 seconds if my math is right.
Holding the leftmousebutton starts an attack animation and hitting k kills the character.
There is no current way to revive the character so the program is essentially over if k is pressed. You may hit escape to exit it. 
The character faces independently of the movement direction and is only concerned with the mouse, the sprite can only face left or right. T
hitting i will swap the terrain to ice hitting f will change it to swamp and hitting t will turn it to stone. these are for testing and will be swapped to something else when the game is finished.
rightclicking on a chest will add a random custom item to your player there are 2 currently.
these items can be activated by 1 and 2 keys one item will give a permenant speed/acceleration boost for your terrain the other lets you shoot fireballs every 3 seconds.
item effects are reset when swapping terrains.


Chatgpt was used heavily to help me debug things as well as helped  stop other animations from interrupting attack and death animations
it also slightly helped me with the movement logic (deltatime) though it only wrote one line that i later modified and expanded upon. 
I also used it to find useful monogame functions such as spriteeffects. 

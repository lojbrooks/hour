# hour

hour is a matching block puzzle game inspired by my own personal fear of time running out. Your goal is to score as many points as you can over the course of one hour. The game uses a combo system in which players will attempt to make matches of alternating coloured blocks, while 'special' blocks will also complicate matters by manipulating the board and game clock. When the clock hits zero, the game is over.

To start with the game will provide you with two groups of three blocks that you can drag into the grid (the game board will be hourglass shaped, although the actual playing area will just be the top half). At stage one these block groups will simply be comprised of Black or White blocks, but as you progress they will also include the special power blocks. After the player drops a block group into the board, the game will check for any 'three in a row' of the same colour. These blocks will be destroyed, the player will gain score, and the remaining blocks will drop again. This continues until there are no matches, and the player is free to select another block group.

COMBO SYSTEM.

hour utilises a simple combo system, where the player will improve a score multiplier by continually matching alternate colour blocks. The first match can be of any colour, but after this the player should look for a different match, Black, then White, then Black again. This is aided by the fact that the game will specifically look for the ideal colour match. For instance, if a group is dropped into a row where a Black match and a White match would be made, the game will destroy the required blocks first and continue your combo. Matching an incorrect colour group will lose your combo and reset your multiplier.

SPECIAL BLOCKS.

From the second level the game will start including special blocks amongst the Black and White blocks. These blocks cannot be matched with others, and are only removed from the grid if they touch the bottom of the playing area. Each special block has two powers, one passive power which is enabled while the block is in play in the grid, and one which is used when the block hits the base of the grid and is destroyed. Usually keeping the block in play is the better option, these will usually be 'good' powers such as increasing multiplier, destroying other blocks, helping to create matches etc. The other powers will be negative effects, such as speeding up the clock, knocking minutes from the timer, and other in-game effects that make playing more difficult.

This should ensure there is a complex play mechanic, should you attempt to sure up your special blocks and risk losing your combo? Is it worth dropping one to the bottom and losing time from the clock to keep your score high? I should also note here that piling blocks up to the upper limit of the screen causes no penalty to score or time, but will destroy all White and Black blocks in that column, potentially dropping your special blocks to the floor.

SCORING.

I haven't worked out the exact scoring system yet, but it should go something like this...

Level 1 - 0 points
Level 2 - 10 points
Level 3 - 100 points
Level 4 - 1000 points

etc etc

Match three in a row = 1 point
Match four in a row = 2 points
Match five in a row = 3 points

Multiplier will be something like an increase of 0.05 per combo match.

Match Score x Level x Combo multiplier (rounded down).

So lets say you're on level 4, and have done five correct combos in a row (Black, White, Black, White, Black)

So a match three would get you 1 x 4 x 1.25 =  5

A match four after that would get you 2 x 4 x 1.3 = 10.4 (10 rounded down)

A match five after that would get you 3 x 4 x 1.35 = 16.2 (16 rounded down)

The idea being that the combo multiplier would be more important the further you get in the game. Obviously this is something that would need extensive testing!


SO FAR:

As you can see, I haven't got that far! The dragging of blocks to the grid works OK, they fall as intended (although only from the left side!). I'm trying to fill an array to record the positions of blocks so I can check for matches, but I'm struggling to get this set up. I feel like once this is working the rest should be fairly straightforward, implementing scoring and the special blocks should be easy enough to do I think. Feel free to offer up any thoughts you have, I haven't shared any of this stuff with anyone so a second pair of eyes certainly wouldn't go amiss.

Cheers,

Beeve x

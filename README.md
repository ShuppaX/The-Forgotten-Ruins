# BananaGame

BananaGame is an adventure game from an isometric view.

## Installation

Use the link to download and extract the .zip to play the current version of the game.

[Download link](https://tuni-my.sharepoint.com/:u:/g/personal/lauri_kalliola_tuni_fi/EYCb0dgwe-VOhNAcfGWFfBIBxFtmW40NRl4ImT8APoYGSw?e=DDNlH1)

## Controls

```python
# Movement:
   WASD (Keyboard)
   Left Stick (Gamepad)

# Dash:
   Shift/Space (Keyboard)
   Button South (Gamepad)

# Interact:
   L (Keyboard)
   Button North (Gamepad)

# Change Ability:
   R (Keyboard)
   D-Pad Left (Gamepad)

# Ability:
   K (Keyboard)
   Button East (Gamepad)

# Attack
   J (Keyboard)
   Button West (Gamepad)

# Pause
   Escape (Keyboard)
   Start (Gamepad)

```

## Work in progress:

```python
# Deathscreen:
   The buttons don't currently do anything.

# Sword swing particle effect

# Ending
```

## Current known issues:

```python
# Sand/Spark throwable
   These get disabled immediately when any of the particles collide with anything.

# Collider on moving puzzle parts.
   The colliders are sometimes too big and block the players movement in an annoying manner.

# Ranged enemy
   The enemy can get stuck shooting in a wrong direction.
   The enemy's projectiles don't work as inteded.

# Melee enemy
   Does not always damage the player.

# Puzzles
   The puzzles can be softlocked sometimes.
   The player can get stuck on the last moving staircase in the level if they turn off one of the puzzles torches.

# Pick upable rock
   If the rock is too close to a wall / near a corner edge of the platform the player can get "stuck" trying to
   grab the rock. Interruptable with a interaction cancel.

# Player melee animation
   The melee animation gets stuck for a short while if the player has movementinput after a melee attack.

# Dash
   Dash stops if the player pauses during dash. (Known issue, probably won't have time to figure it out)

# Audio Settings
   The values for the slider valuedisplays don't update correctly at the start of the game.
```

## License

BSD 3-Clause License

Copyright (c) 2023, GA Spring 2023
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its
   contributors may be used to endorse or promote products derived from
   this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
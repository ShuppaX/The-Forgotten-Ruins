# BananaGame

BananaGame is an adventure game from an isometric view.

## Installation

Use the link to download and extract the .zip to play the current version of the game.

[Download link](https://tuni-my.sharepoint.com/:u:/g/personal/lauri_kalliola_tuni_fi/EYCb0dgwe-VOhNAcfGWFfBIBxFtmW40NRl4ImT8APoYGSw?e=DDNlH1)

## Controls

```python
# Movement:
   WASD (Mouse & Keyboard / Keyboard only)
   Left Stick (Gamepad)

# Dash:
   Shift (Mouse & Keyboard / Keyboard only)
   Space (Mouse & Keyboard / Keyboard only)
   Button South (Gamepad)

# Interact:
   E (Mouse & Keyboard)
   L (Keyboard only)
   Button North (Gamepad)

# Change Ability:
   R (Mouse & Keyboard / Keyboard only)
   D-Pad Left (Gamepad)

# Ability:
   Mouse Right (Mouse & Keyboard)
   K (Keyboard only)
   Button East (Gamepad)

# Attack
   Mouse Left (Mouse & Keyboard)
   J (Keyboard only)
   Button West (Gamepad)

```

## Current known issues:
```python
# Sand/Spark throwable
   These get disabled immediately when any of the particles collide with anything.

# Collider on moving puzzle parts.
   The colliders are sometimes too big and block the players movement in an annoying manner.

# Ranged enemy
   The enemy can get stuck shooting in a wrong direction.

# Melee enemy
   Does not always damage the player.

# Throwable
   Can throw an error if the game was reset through Debug Menu.

# Fennec animations
   The fennect is missing an animation when it is walking towards an interactable object.

# Interacting
   Spamming interact can cause the player to enter wrong animations.
   Picking up and dropping interactable objects pushes the player back.

# Puzzles
   The puzzles can be softlocked sometimes.
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
# <h1 align="center">Unity-2D-Player-Combat-System-Template</h1>
<h2 align="center" dir="auto"><strong><code>Unity Engine Game Project</code></strong></h2>


<div class="Header Image">
  <a align="center" draggable="false" href="https://novalen.itch.io/jump-masters"> <img align="center" width="800" height="450" alt="PlayerCombat Gif" src="https://github.com/user-attachments/assets/5a1442d6-d720-4979-9c7b-4385099f1e85" /></a>

</div>

<h2 align="center" dir="auto"> Overview </h2>
<h2 align="center" dir="auto"><strong>Genre: <code>2D Side-Scroller</code> <code>Action-Adventure</code> <code>Hack n Slash</code></h2>
<h2 align="center" dir="auto"><strong>Role: <code>Gameplay and UI Programming</code></h2>
<p dir="auto">A combat system template I developed to be a personal package asset to implement into future projects. Unique combo button system, Lock On System, and Enemy Stagger.</p>

<h2 align="center" dir="auto"> Player Combat </h2>
<p dir="auto">The player combat was designed to have it so the player can perform attacks while on the move or stationary. With the use of both the 'Left Mouse Button and Right Mouse Button' players can perform unique physical attacks and chain them together to continue the combo if the player makes contact with the enemy/dummy that is hit by the player hit box.</p>

<h3 align="center" dir="auto"> Combat Design </h3>
<p dir="auto">Player will always initially start out in exploration mode. This modes is typical for regular movement and so for when the player is progressing through levels or exploring in the 2D space while they are not in combat. When the attack button is pressed it sends a signal setting combat mode to `true`. This will activate the initial attack animation that allows the play to enter into combat mode revealing the players weapon or fighting stance showing they are now prepared to engage enemies. The most important design aspect behind this combat system I developed is the use of animation events placed in the animations of each individual attack and transitional animations. </p>

<img width="1178" height="264" alt="Screenshot 2026-08-06 112810" src="https://github.com/user-attachments/assets/045d4eee-22bc-4ecc-b310-8cb047d3d04c" />

<p dir="auto">The animation events with in the combat animations are signals to activate functions with specific purposes. Throughout my time developing player attacking it would clip through multiple animations making it look as though the player was glitching. With specific animation event functions I can pause the input so that the animation can play in full and prevent it from skipping to another attack without it fully playing the initial animation before it </p>

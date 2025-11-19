# CSS385-A-FinalProject

# Prototype (Checkpoint 2)
Game Pitch

Pilot a recovery rocket through drifting debris and mechanical ruins in orbit — collect lost ship components, manage your cargo weight, and return through a space-time portal before being destroyed.

Your ship’s thrusters weaken as cargo increases, forcing you to balance risk and reward: collect more parts for a higher score, or head home before debris tears you apart.

# Splash Screen 
/SplashScreen.png
# Game Synopsis

You control a small salvage rocket navigating a zero-gravity junkfield.
Your goal is to collect scattered mechanical components floating through space while avoiding or destroying dangerous obstacles.
The rocket’s movement speed is affected by the cargo it carries — the more components collected, the heavier and slower it becomes.
A portal appears periodically, serving as your return point to base and completing the mission.

# Objective

Collect components and return to base to upgrade your space ship.

Avoid or destroy incoming obstacles.

Reach the portal to finish the mission.

Survive long enough to bring back your cargo!

# Core Mechanics
Mechanic	Description
Movement	Control the rocket with WASD.
Shooting	Fire bullets using Spacebar to clear obstacles.
Shield	Press J to activate a temporary shield (cooldown-based).
Cargo Collection	Fly into gear components to collect them — cargo bar fills up.
Speed Reduction	Movement speed decreases up to 50% at full cargo.
Portal	Spawns every 60 seconds, drifts downward — entering it ends the mission.
Obstacles	Randomly drift across the screen; collisions cause game over.
# Controls
Key	Action
W, A, S, D	Move the rocket
Spacebar	Fire bullets
J	Activate shield
Esc	Pause / Resume
Mouse	(Optional future aiming)
# UI and Systems

Splash Screen / Pause Menu:
Includes Start, Continue, Restart, and Exit options.
Serves as both a splash screen and pause system.

Cargo Progress Bar:
Shows collected components (0–10).
Updates dynamically and displays Cargo: X / 10.

Visuals:
Rocket thruster animation (boosterFlame) activates during thrust.
Portal and gears visually rotate and drift to simulate space motion.

# Video Demonstration
https://youtu.be/VNxFe-jdDjQ
Due to the Upgrade Scene isnt created yet, so touching of portal will exit the game

# Current Features Implemented

Working player movement and shooting system

Shield mechanic with cooldown

Cargo system and progress bar

Weighted speed mechanic (slower with more cargo)

Pause/Resume functionality

Obstacle and component object pooling

Portal spawning every 60 seconds

Game-over and restart logic

UI built using Unity UI Toolkit

# Next Steps

Introduce enemy drones

Add background parallax

Sound effects and background music
 
Mission completion animation instead of instant

Spaceship upgrade system
 
Better UI(Start Scene and Base Scene)

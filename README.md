# CSS385 Program2

Done based on Program1 and Program 2, just adding save and load function
The save function will save the x, y position of palyer and the score. And also the existing Obstacle object's position and size.
The load function will convert json text to data, and update play's position and score and recreate obstacle.

Brief write-up on minimum of 3 storage mechanisms you considered, pros/cons, and why you chose the solution you did
I considered three ways to save and load data: PlayerPrefs, JSON files, and databases. PlayerPrefs is simple and great for small things like settings but can’t handle complex data. Databases are powerful and good for big projects or online leaderboards, but they’re too heavy for a small local game. I decided to use JSON serialization because it’s easy to read, portable, and perfect for saving structured data like the player’s position, score, and obstacles. It gives me flexibility without adding extra complexity, and Unity’s built-in tools make it straightforward to use.

Demo link:
https://youtu.be/yhGSfZPtqKU


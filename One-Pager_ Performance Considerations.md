One-Pager: Performance Considerations  
 Identify 3 sources of performance concerns for games similar to yours

* **Physics and collision detect**  
  With Unity checking every object each frame, physics updates and collision checks can quickly become expensive because of the increasing number of moving asteroids and scrap objects.  
* **Particle and Lighting Effects**  
  Effects from explosions, thrusters, and glowing debris may involve multiple real-time lights and particle systems that increase GPU load.  
* **Objects Instantiation and Garbage Collection**  
  Continuously spawning and destroying scrap, asteroids, or particle objects can cause memory fragmentation and frame stutters when Unity’s garbage collector kicks in.


Identify at least one common strategy to address each performance concerns

* **Physics and collision detect**  
  * Use Layers to ignore unnecessary collisions.  
  * Replace real physics on far-away objects with simple distance checks or bounding-box triggers.  
  * Use Update() wisely—only for physics, not every script.  
* **Particle and Lighting Effects**  
  * Replace dynamic lights with baked or sprite-based glow effects.  
  * Lower particle counts and lifetime  
* **Objects Instantiation and Garbage Collection**  
  * Implement an object pool for asteroids, scrap, and explosion effects to reuse existing instances instead of destroying and creating new ones.

Summarize how you might use one or more such strategies for your own game and why  
I’ll focus on object pooling first since the game continuously spawns scrap and asteroids. Reusing inactive objects will cut down on CPU and garbage-collection spikes. Then, I will simplify collision checks by using layers and disabling physics for far off objects. Finally, I will keep particle effects lightweight by creating one shared explosion prefab and baked glow sprites for light flashes. 

# game-science

<h2>Week 6</h2>
<p>Check in <b>Assets/Module1/Scenes/Week6/</b> -- there is a scene called Week6.unity. In the scene, there is:</p>
<ul>
  <li>A skeleton agent, ready for use with NodeCanvas' behavior trees</li>
  <li>Three spheres, all called different variations of 'PatrolPoint'</li>
  <li>A capsule named Target</li>
</ul>
<p>In the subfolder <b>BT</b>, there are two behavior trees ready for configuration.</p>
<ol>
  <li>3PointPatrol will have its agent patrol randomly between three points. <b>You must specify the three points in the one 'Patrol' child node (by using GameObject references) in the NC Editor.</b> I like to use the three PatrolPoints as the...patrol points.</li>
  <li>WanderTillFind will have its agent wander aimlessly around the map until they find a specified target. Once found, the agent will follow the target until it loses sight of it. When it does, it will continue wandering until it finds it again. <b>You must specify the target in the two bottom Condition nodes and the rightmost GoTo node</b> (We can actually use a blackboard variable to handle this, but it would no longer be a PBT. Actually, right now, I don't think it even still is a PBT). I like to use the Target object, then while the game is running I move it around in Scene view to test it out.</li>
</ol>

<p>To use the tree, select the skeleton agent, then in the Inspector under the Behaviour Tree Component select the BT you want it to use. If you specified everything properly for that tree, it should be good to go--just click Play and it should start up.</p>

<h3>To Do</h3>
<p>We should have something to show for the update next Monday, so I could use some good Behaviour Trees to slap on some agents. Once you have a few, <b>push it to the develop branch (NOT the master branch)</b> and let me know. I'm going to try to see if I can clean things up a bit with the agent--right now, it is using NodeCanvas' built in sight system instead of the simple one I had for the previous dev skill.</p>

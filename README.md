# GE2-Boid-Project
Name: James Slattery

Student Number: C19774569

Class Group: DT508

# Video
[Video](https://youtu.be/TL8-2KNXcfU)

# Description
An underwater predator-prey simualtion involving plants, herbivores and carnivores

# Observations
Ran into many issues dealing with instantiating objects and I learned a lot about them through solving these issues, in the end I'm proud of how well I got it to work. I feel my biggest achievement was to get the interactions between the herbivores and plants working as smoothly as it did

# How it works
## Sequence
The plants choose a random height to grow to after which they will release a seed every 10 seconds. After releasing three seeds the plant dies. The seeds drift away from the plant for a few seconds before landing onto the sand and sprouting a new plant.

The Herbivores swim around randomly until they get hungry enough at which point they will start looking for plants to eat. Once they have eaten enough plants they return to swimming. If a herbivore is fully fed it has a chance to become pregnant and give birth to a new herbivore after 30 seconds. If a herbivore is pregnant their head changes colour.

The carnivore swims around randomly until it gets hungry enough at which point it will begin hunting one of the herbivores. After eating a herbivore it returns to swimming. There is only one carnivore in the scene.

## AI
Seek/Hunter: Searches for the appropiate tag via physics sphere, calculates the closest target to the agent and chooses that target for the agent to approach

Boid: Handles the autonomous aspects of the agent and the pregnancy system and child instantiation

Obstacle Avoidance: Avoids all obstacles except for objects under the Food tag and the sand ground

Constrain: Keeps the multiple gameobjects making up one entity together under certain parameters

Noise Wander: Handles the agents independent movement

# Instructions
1. Open the build
2. Use the arrow keys to move the camera
3. Use the mouse to look around the scene

# List of classes/assets

| Class/asset | Source |
|-----------|-----------|
| FPS Controller | From [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Water Shader | From [here](https://assetstore.unity.com/packages/3d/characters/animals/simple-boids-flocks-of-birds-fish-and-insects-164188) |
| Plant Spawner | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Plant Script | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Seed Script | Written myself |  |
| Nematode School | From [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Nematode | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Spine Animator | From [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Hunter Script | Written myself |
| Boid | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Noise Wander | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Seek | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Obstacle Avoidance | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |
| Constrain | Modified from [here](https://github.com/skooter500/GE2-Test-2023-Starter) |

# Resources
All external assets come from [here](https://assetstore.unity.com/packages/3d/characters/animals/simple-boids-flocks-of-birds-fish-and-insects-164188)

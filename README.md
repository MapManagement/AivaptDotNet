# Aivapt .NET
This project tries to replace my old Discord bot called "Aivapt". On one hand
I really had to improve my old code so that the bot would run more stable. On
the other hand I really wanted use [Discord .NET](https://docs.stillu.cc/index.html)
this time instead of [discord.py](https://discordpy.readthedocs.io/en/stable/).
Therefore I'm using C# and not Python anymore.

## Modules
I try to divide all fucntionalities in different modules. Not only to keep my code
as clean as possible but also to ensure a better usability later on. For now there
are these modules:

### General
Basic commands and information about the bot can be found here:
- ``test`` - used for a basic repsonse
- ``info`` - returns an embed that contains general information about the bot

### Voice
Contains anything that is related to oprations within a voice channel:
- ``join`` - bot connects to the voice channel of the user that invoked the command
- ``leave`` - bot disconnects from the voice channel
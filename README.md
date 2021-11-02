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
- ``join`` - bot connects to current voice channel
- ``leave`` - bot disconnects from current voice channel
- ``play [songUrl]`` - bot uses the entered URL to play a song

### Simple Command
Users can also create their own commands which do only display a specific text given
by the user. I called them **Simple Commands**. Once you created a command you can
use it like a normal command:
- ``cmd create [commandName] [title] [text]`` - create a new command
- ``cmd edit [commandName] [newTitle] [newText]`` - edit an existing command
- ``cmd del [commandName]`` - delete an existing command
- ``cmd all`` - displays all available "Simple Commands"


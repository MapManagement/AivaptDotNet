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
- ``play [songUrl]`` - bot uses the entered URL to play audio
- ``skip`` - bot skips current audio
- ``stop`` - bot stops playing current audio
- ``continue`` - bot continues playing audio

### Development
These commands return information about current development stages:
- ``dev latest`` - returns the latest commit
- ``dev info`` - returns general information about the repository
- ``dev release`` - returns the latest release
- ``dev issue [issue_number]`` - returns the specific issue

### Simple Command
Users can also create their own commands which do only display a specific text given
by the user. I called them **Simple Commands**. Once you created a command you can
use it like a normal command:
- ``cmd create [commandName] [title] [text]`` - create a new command
- ``cmd edit [commandName] [newTitle] [newText]`` - edit an existing command
- ``cmd del [commandName]`` - delete an existing command
- ``cmd all`` - displays all available "Simple Commands"

## Setup
### Discord Bot User
First of all you need to create a bot Discord user. You can create your bot on
[this website](https://discord.com/login?redirect_to=%2Fdevelopers%2Fapplications).
You'll get a unique token which consists of 60 symbols. See [Credentials](#credentials)
for further information. However, you probably want to adjust some permissions. I tested
following selection and had now problems so far:
<!-- TODO: add permissions -->

### Database
A MariaDB (MySQL should work too) connection is needed. You can find everything you
need in the [Migrations folder](Migrations/). Follow the instructions in
[Credentials](#credentials) for authentication.

### Lavalink
The audio module and respectively the whole audio service is based on
[Lavalink](https://github.com/freyacodes/Lavalink). I'm using a wrapper for Discord .NET
called [Victoria](https://github.com/Yucked/Victoria) to play any kind of audio. The bot
connects to a running Lavalink server which needs to be configured via a simple file.
You can find more about the configuration of Lavalink servers
[here](https://github.com/freyacodes/Lavalink#server-configuration). Keep in mind that
you also need to change the corresponding password. More about it in 
[Credentials](#credentials).

### Credentials
At the moment I'm storing my credentials in simple .txt files. The exact files are:
| Service   | File Name                 | Format                                                             |
|-----------|---------------------------|--------------------------------------------------------------------|
| MariaDB   | sql_connection_string.txt | server=localhost;port=3306;database=name;userid=user;password=pass |
| Bot Token | token.txt                 | thisismytoken...                                                   |
| Lavalink  | lavalink_pass.txt         | mypassword...                                                      |



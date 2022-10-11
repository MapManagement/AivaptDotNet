<p align="center">
	<img src="src/Resources/Images/bot_icon.png" />
	</br>
	<a>
		<img src="https://github.com/MapManagement/AivaptDotNet/actions/workflows/dotnet-build.yml/badge.svg" />
	</a>
</p>

# Aivapt .NET

This project tries to replace my old Discord bot called "Aivapt". On one hand I really had to improve my old code so that
the bot would run more stable. On the other hand I really wanted use [Discord .NET](https://docs.stillu.cc/index.html)
this time instead of [discord.py](https://discordpy.readthedocs.io/en/stable/). Therefore I'm using C# and not Python
anymore.

## Commands

The bot offers a wide range of built-in commands. In addition to them you can also create your own commands. I call them
``SimpleCommands``. [COMMANDS.md](docs/COMMANDS.md) contains a small explanation for each command.

## Setup

### Discord Bot User

First of all you need to create a bot Discord user. You can create your bot on
[this website](https://discord.com/login?redirect_to=%2Fdevelopers%2Fapplications). You'll get a unique token which consists
of 60 symbols. See [credentials](#credentials) for further information. However, you probably want to adjust some
permissions. I'm using following permission integer for my bot:

```Permission Integer: 2217856064```

### Credentials

I'm using [user-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=linux)
to store my credentials for development purposes. You need following variables:
| Key                          | Value                               |
|:----------------------------:|:-----------------------------------:|
| AIVAPT__ADMIN_ID             | Discord admin-user ID               |
| AIVAPT__BOT_TOKEN            | Discord bot token                   |
| AIVAPT__DB_CONNECTION_STRING | credentials for database connection |
| AIVAPT__LAVALINK_PASSWORD    | Lavalink server password            |

### Manual

#### Database

A MariaDB (MySQL should work too) connection is needed. You can find everything you need in the
[migrations folder](Migrations/). Follow the instructions in [credentials](#credentials) for authentication.

#### Lavalink

The audio module and respectively the whole audio service is based on [Lavalink](https://github.com/freyacodes/Lavalink).
I'm using a wrapper for Discord .NET called [Victoria](https://github.com/Yucked/Victoria) to play any kind of audio. The
bot connects to a local Lavalink server which needs to be configured using a simple configuration file. You can find a more
detailed explanation [here](https://github.com/freyacodes/Lavalink#server-configuration). Keep in mind that you also
need to change the corresponding password. More about it in [credentials](#credentials). I'm already working on an own
library that simplifies any voice channel interaction. Once it's finished it'll probably remove Lavalink.

### Docker

To simplify the deployment process, I created a simple docker-compose file. It includes the Dockerfile of the bot, a MariaDB
instance and a Lavalink server. You only need to insert your credentials with an environment file. Then you can start
the compose file:

```sh
docker-compose --env-file your_env_file
```

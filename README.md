# Aivapt .NET

![example workflow](https://github.com/MapManagement/AivaptDotNet/actions/workflows/dotnet-build.yml/badge.svg)

This project tries to replace my old Discord bot called "Aivapt". On one hand
I really had to improve my old code so that the bot would run more stable. On
the other hand I really wanted use [Discord .NET](https://docs.stillu.cc/index.html)
this time instead of [discord.py](https://discordpy.readthedocs.io/en/stable/).
Therefore I'm using C# and not Python anymore.

## Commands

The bot offers a wide range of built-in commands. In addition to them you can
also create your own commands. I call them ``SimpleCommands``. A small guide
for ``SimpleCommands`` and all built-in commands as well, are listed in
[COMMANDS.md](docs/COMMANDS.md).

## Setup

### Discord Bot User

First of all you need to create a bot Discord user. You can create your bot on
[this website](https://discord.com/login?redirect_to=%2Fdevelopers%2Fapplications).
You'll get a unique token which consists of 60 symbols. See [Credentials](#credentials)
for further information. However, you probably want to adjust some permissions. I tested
following selection and had now problems so far:
```Permission Integer: 2217856064```

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

I'm using [user-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=linux)
to store my credentials for development purposes. Following are the key-value pairs:
| Key                          | Value                               |
|:----------------------------:|:-----------------------------------:|
| AIVAPT__ADMIN_ID             | Discord admin-user ID               |
| AIVAPT__BOT_TOKEN            | Discord bot token                   |
| AIVAPT__DB_CONNECTION_STRING | credentials for database connection |
| AIVAPT__LAVALINK_PASSWORD    | Lavalink server password            |

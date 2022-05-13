# Commands

## General

Basic commands and information about the bot can be found here:

- ``test`` - used for a basic repsonse
- ``info`` - returns an embed that contains general information about the bot

## Voice

Contains anything that is related to oprations within a voice channel:

- ``join`` - bot connects to current voice channel
- ``leave`` - bot disconnects from current voice channel
- ``play [songUrl]`` - bot uses the entered URL to play audio
- ``skip`` - bot skips current audio
- ``stop`` - bot stops current audio
- ``continue`` - bot continues current audio

## Development

These commands return information about current the development stage:

- ``dev latest`` - returns the latest commit
- ``dev info`` - returns general information about the repository
- ``dev release`` - returns the latest release
- ``dev issue [issue_number]`` - returns the specific issue

## Quotes

Create quotes of users on your Discord server:

- ``quote new [user] [quoteText]`` - creates a new quote of a user
- ``quote show [quoteId]`` - returns the quote with the given ID
- ``quote random`` - returns a random quote
- ``quote amount`` - returns the amount of quotes that exist

## Simple Command

Users can also create their own commands which display a specific text chosen by the user. I called them
**Simple Commands**. Once you created a **Simple Command** you can use it like a normal command:

- ``cmd create [commandName] [title] [text]`` - create a new command
- ``cmd edit [commandName] [newTitle] [newText]`` - edit an existing command
- ``cmd del [commandName]`` - delete an existing command
- ``cmd all`` - displays all available "Simple Commands"

namespace EmojiCharades.Shared

open System

module Constants =
    let socketPath = "/ws"

type Color =
    | Red
    | Orange
    | Yellow
    | Green
    | Blue
    | Purple
    | Brown
    | Black
    | White

type Avatar = { Color: Color }

type PlayingStatus = {
    Nickname: string
    Avatar: Avatar
    CurrentRoom: string
}

type Status =
    | WaitingInLobby
    | Playing of PlayingStatus

type Player = { Id: Guid; Status: Status }

/// Messages processed on the client
type Guess =
    | Correct
    | Incorrect of string

type ClientMessage =
    // Messages for before the game has started
    | RoomCreated of {| code: string |}
    | PlayerJoined of
        {|
            id: Guid
            nickname: string
            avatar: Avatar
        |} // TODO: I'm not sure if we should use the `Player` type for the client state.
    | PlayerDisconnected of {| id: Guid |}
    | UpdateAvatar of {| id: Guid; avatar: Avatar |} // TODO: Should this be "PlayerUpdated?"
    | SetStartTime of int option // TODO: ???
    // Messages for after the game has started
    | StartGame
    | GuessMade of
        {|
            playerNickname: string
            guess: Guess
        |}

/// Messages processed on the server
type ServerMessage =
    | CurrentlyWaiting

    // Messages before you're in a room
    | CreateRoom
    | JoinRoom of {| code: string |}

    // Messages once you're in a room
    | Disconnect
    | MakeGuess of {| guess: string |}

namespace EmojiCharades.Shared

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

type Avatar = { color: Color }

type Player =
    { nickname: string
      avatar: Avatar
      actor: bool }

/// Messages processed on the client
type ResetStage = { newActor: string }

type Guess =
    | Correct
    | Incorrect of string

type ClientMessage =
    // Messages for before the game has started
    | AddPlayer of Player
    | RemovePlayer of string
    | UpdateAvatar of string * Avatar
    | SetStartTime of int option
    // Messages for after the game has started
    | StartGame
    | ResetStage of ResetStage
    | GuessMade of string * Guess

/// Messages processed on the server
type ServerMessage =
    | Join
    | MakeGuess of string

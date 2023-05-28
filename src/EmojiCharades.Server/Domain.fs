module Domain

open System
open EmojiCharades.Shared

type PlayerRole =
    | Host
    | Player

type RoomPlayer = { Player: Player; Role: PlayerRole }

type LobbyState = { Players: RoomPlayer list }

type InGamePlayer = {
    Player: Player
    Role: PlayerRole
    Score: int
}

type CurrentTurn = { Player: Player; TurnIndex: int }

type InGameState = {
    Players: InGamePlayer list
    UsedWords: Set<string>
    CurrentWord: string
    CurrentTurn: CurrentTurn
}

type RoomStatus =
    | Lobby of LobbyState
    | InGame of InGameState

type Room = { Code: string; Status: RoomStatus }

// this has side-effects!
let createRandomCode () =
    let characters = "abcdefghhijklmnopqrstuvwxyz123456789"

    [ 1..6 ]
    |> Seq.map (fun _ -> characters[Random.Shared.Next(0, characters.Length)])
    |> String.Concat

// used for development purposes, will be a proper wordlist later on
let wordlist = [ "Snake"; "Fruit"; "Red"; "Square"; "Circle" ]

let findRoomByCode (code: string) (rooms: Room list) =
    rooms |> List.tryFind (fun room -> room.Code = code)

// this has side-effects!
let rec createUniqueRoomCode (existingRoomCodes: string list) =
    let roomCode = createRandomCode ()

    if existingRoomCodes |> List.contains roomCode then
        createUniqueRoomCode existingRoomCodes
    else
        roomCode

let createRoom (hostingPlayer: Player) (existingRoomCodes: string list) =
    let code = createUniqueRoomCode existingRoomCodes
    let players = [ { Player = hostingPlayer; Role = Host } ]
    let status = Lobby { Players = players }
    { Code = code; Status = status }

let rec pickNextWord (inGameState: InGameState) =
    let randomWord = wordlist[Random.Shared.Next(0, wordlist.Length)]

    match inGameState.UsedWords.Contains(randomWord) with
    | true -> pickNextWord inGameState
    | false -> {
        inGameState with
            CurrentWord = randomWord
            UsedWords = inGameState.UsedWords.Add(randomWord)
      }

let nextTurn (inGameState: InGameState) =
    let playerCount = List.length inGameState.Players

    let turnIndex =
        let index = inGameState.CurrentTurn.TurnIndex + 1
        if index < playerCount then index else 0

    inGameState.Players
    |> List.tryItem turnIndex
    |> Option.map (fun currentPlayerTurn -> {
        Player = currentPlayerTurn.Player
        TurnIndex = turnIndex
    })

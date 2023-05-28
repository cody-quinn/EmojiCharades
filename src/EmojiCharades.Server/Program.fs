open EmojiCharades.Shared
open Elmish

// TODO: I need to think about this stuff some more.
module Actor =
    open System
    open Akka.FSharp
    open Domain

    type ActorMsg =
        | CreateRoom of {| host: Player |}
        | JoinRoom of {| code: string; player: Player |}

    type ActorReply =
        | Success
        | RoomNotFound of {| code: string |}

    let systemConfig = Configuration.defaultConfig ()
    let system = System.create "actor-system" systemConfig

    let processor (mailbox: Actor<ActorMsg>) =
        let rec loop (rooms: Room list) =
            actor {
                let! msg = mailbox.Receive()

                match msg with
                | CreateRoom info ->
                    let existingRoomCodes = rooms |> List.map (fun room -> room.Code)
                    let room = createRoom info.host existingRoomCodes
                    return! loop (room :: rooms)
                | JoinRoom info ->
                    let sender = mailbox.Sender()
                    let room = rooms |> List.tryFind (fun room -> room.Code = info.code)

                    match room with
                    | None ->
                        // I'm not sure if this is the correct way to reply!
                        sender <! RoomNotFound {| code = info.code |}
                        return! loop rooms
                    | Some room -> // this seems terrible inneficient
                        let newRooms =
                            rooms
                            |> List.map (fun currentRoom ->
                                if currentRoom.Code = info.code then
                                    let players = { Player = info.player; IsHost = false } :: currentRoom.Players
                                    { currentRoom with Players = players }
                                else
                                    currentRoom)

                        sender <! Success
                        return! loop newRooms
            }

        loop []

    let actorRef = spawn system "my-actor" processor

open System

let init () =
    {
        Id = Guid.NewGuid()
        Status = WaitingInLobby
    },
    Cmd.none

// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

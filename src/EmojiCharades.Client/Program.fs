module App

open Sutil
open Sutil.CoreElements
open Sutil.DaisyUI
open Fable.Core.JsInterop
open EmojiCharades.Shared

let increment number = number + 1
let decrement number = number - 1

type GameState = { Players: Player list }

let init () = { Players = [] }, Cmd.none

let update msg model =
    let fm =
        match msg with
        | AddPlayer player ->
            let players = player :: model.Players
            { model with Players = players }
        | RemovePlayer name ->
            let players = model.Players |> List.filter (fun p -> p.Nickname <> name)
            { model with Players = players }
        | _ -> model

    fm, Cmd.none

let playerComp (player: Player) =
    Html.div [
        Attr.className [ "rounded"; "bg-red-600"; "text-white"; "w-fit"; "px-3.5"; "py-2" ]
        Attr.text player.Nickname
    ]

let view () =
    // let nextId = Helpers.makeIdGenerator()
    // let makeThing thing = ThingView (nextId()) thing

    let model, dispatch = Store.makeElmish init update ignore ()
    let players model = model.Players

    Html.div [
        disposeOnUnmount [ model ]

        Bind.eachi (model .> players, snd >> playerComp)

        Daisy.Button.button [
            Daisy.Button.extraSmall
            Daisy.Button.primary
            Attr.text "Increment"
            onClick
                (fun _ ->
                    dispatch (
                        AddPlayer {
                            Nickname = "Cody"
                            Avatar = { Color = Red }
                            Actor = false
                        }
                    ))
                []
        ]
    ]

importSideEffects "./styles.css"
Program.mount ("sutil-app", view ()) |> ignore

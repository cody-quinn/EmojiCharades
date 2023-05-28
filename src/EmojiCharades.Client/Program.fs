module App

open Sutil
open Sutil.CoreElements
open Sutil.DaisyUI
open Fable.Core.JsInterop
open EmojiCharades.Shared

type Model = { Nickname: string; RoomCode: string }

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
                            Nickname = Nickname "Cody"
                            Avatar = { Color = Red }
                        }
                    ))
                []
        ]
    ]

importSideEffects "./styles.css"
Program.mount ("sutil-app", view ()) |> ignore

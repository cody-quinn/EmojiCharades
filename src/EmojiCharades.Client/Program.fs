module App

open Sutil
open Sutil.CoreElements
open Sutil.DaisyUI
open Fable.Core.JsInterop
open EmojiCharades.Shared

let increment number = number + 1
let decrement number = number - 1

type GameState = { players: Player list }

let update msg model =
    let fm =
        match msg with
        | AddPlayer player ->
            let players = player :: model.players
            { model with players = players }
        | RemovePlayer name ->
            let players = model.players |> List.filter (fun p -> p.nickname <> name)
            { model with players = players }
        | _ -> model

    fm, Cmd.none

let playerComp (player: Player) =
    Html.div
        [ Attr.className [ "rounded"; "bg-red-600"; "text-white"; "w-fit"; "px-3.5"; "py-2" ]
          Attr.text player.nickname ]

let view () =
    let counter = Store.make 0
    let subscriber = counter |> Store.subscribe (fun i -> printfn "%d" i)

    Html.div
        [ disposeOnUnmount [ counter; subscriber ]

          Bind.el (counter, Html.p)

          playerComp
              { nickname = "Cody"
                avatar = { color = Red }
                actor = false }

          // Daisy.Button.button [
          //   Daisy.Button.extraSmall
          //   Daisy.Button.primary
          //   Attr.text "Increment"
          //   onClick (fun _ -> Store.modify increment counter) []
          // ]
          // Daisy.Button.button [
          //   Daisy.Button.extraSmall
          //   Daisy.Button.primary
          //   Attr.text "Decrement"
          //   onClick (fun _ -> Store.modify decrement counter) []
          // ]
          ]

importSideEffects "./styles.css"
Program.mount ("sutil-app", view ()) |> ignore

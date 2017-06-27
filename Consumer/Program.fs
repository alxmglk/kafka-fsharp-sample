open System
open Kafka

type ConsumerSettings = {
    KafkaUrl : string
    Topic : string
    ConsumeTimeout : TimeSpan
}

type DownloadCompleted = {
    DocumentUri : string
    Timestamp : DateTime
}

let rec loop action =
    async {
        action()

        do! Async.Sleep 1000

        return! loop action 
    }

[<EntryPoint>]
let main argv = 
    let settings = {
        KafkaUrl = "kafka:9092"
        Topic = "download-completed"
        ConsumeTimeout = TimeSpan.FromSeconds(3.)
    }

    let handleMessage m =
        printfn "[Consumer]: received a message from Kafka at %A, message is { DocumentUri : %s; Timestamp : %A }" DateTime.Now m.DocumentUri m.Timestamp
    
    consume settings.KafkaUrl settings.Topic settings.ConsumeTimeout handleMessage
    |> loop
    |> Async.RunSynchronously

    0
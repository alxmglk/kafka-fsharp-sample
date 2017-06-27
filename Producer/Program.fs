open System
open Kafka

type ProducerSettings = {
    Broker : string
    Topic : string
    Timeout : int
}

type DownloadCompleted = {
    DocumentUri : string
    Timestamp : DateTime
}

let rec loop timeout action =
    async {
        action()

        do! Async.Sleep timeout

        return! loop timeout action 
    }

[<EntryPoint>]
let main argv = 
    let settings = {
        Broker = "kafka:9092"
        Topic = "download-completed"
        Timeout = 5000
    }

    let publishMessage () =
        printfn "[Producer]: Sending a message to Kafka at %A" DateTime.Now

        { 
            DocumentUri = Guid.NewGuid() |> sprintf "http://mystorage.com/%A"
            Timestamp = DateTime.UtcNow
        } 
        |> publish settings.Broker settings.Topic |> ignore

    publishMessage
    |> loop settings.Timeout
    |> Async.RunSynchronously
    
    0
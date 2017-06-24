open System

type ProducerSettings = {
    KafkaUrl : string
    Topic : string
    Timeout : int
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
        KafkaUrl = "localhost:9092"
        Topic = "download-completed-v1"
        Timeout = 5000
    }

    fun () -> printfn "[Producer]: Sending a message to Kafka at %A" DateTime.Now
    |> loop settings.Timeout
    |> Async.RunSynchronously
    
    0
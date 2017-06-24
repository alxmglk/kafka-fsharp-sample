open System

type ConsumerSettings = {
    KafkaUrl : string
    Topic : string
}

let rec loop action =
    async {
        action()
        do! Async.Sleep 5000

        return! loop action 
    }

[<EntryPoint>]
let main argv = 
    let settings = {
        KafkaUrl = "localhost:9092"
        Topic = "download-completed-v1"
    }

    fun () -> printfn "[Consumer]: received a message from Kafka at %A" DateTime.Now
    |> loop
    |> Async.RunSynchronously


    0
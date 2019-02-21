namespace NGrains

open OrleansBasic;
open Orleans;
open Microsoft.Extensions.Logging;

module Say =
    open System.Threading.Tasks

    let hello name =
        printfn "Hello %s" name

    type NGrain =
        inherit Grain
        interface IHello with
            member this.SayHello(greeting) = 
                let recv greet = sprintf "\n SayHello message received, greeting = '%s'" greet
                let res = recv greeting
                this.logger.LogInformation res
                let client greet = sprintf "\n Client said: '%s', so NGrain says: Hello!" greet
                let res = client greeting
                Task.FromResult(res)

        val logger : ILogger<NGrain>

        new(log) = { logger = log}
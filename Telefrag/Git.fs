module Telefrag.Git

open System.Threading.Tasks

open Telefrag

let private runGitCommand _args = task {
    return failwith "TODO"
}

let resolve(Ref ref): Task<Commit> = task {
    let! out = runGitCommand [|"rev-parse", ref|]
    return Commit(Seq.exactlyOne out)
}

let getParents(Commit commit): Task<Commit seq> = task {
    let! out = runGitCommand [|"rev-parse", $"{commit}^@"|]
    return Seq.map Commit out
}

namespace Faucet.Contracts.Faucet.ContractDefinition

open System
open System.Threading.Tasks
open System.Collections.Generic
open System.Numerics
open Nethereum.Hex.HexTypes
open Nethereum.ABI.FunctionEncoding.Attributes
open Nethereum.Web3
open Nethereum.RPC.Eth.DTOs
open Nethereum.Contracts.CQS
open Nethereum.Contracts
open System.Threading

    
    
    type FaucetDeployment(byteCode: string) =
        inherit ContractDeploymentMessage(byteCode)
        
        static let BYTECODE = "608060405234801561001057600080fd5b5061019e806100206000396000f3fe6080604052600436106100435760003560e01c80630a93cf331461004f578063a23f8a3d146100a5578063a26759cb146100c3578063af7f2c9b146100cd57600080fd5b3661004a57005b600080fd5b34801561005b57600080fd5b5061008861006a366004610117565b60ff166000908152600160205260409020546001600160a01b031690565b6040516001600160a01b0390911681526020015b60405180910390f35b3480156100b157600080fd5b5060045b60405190815260200161009c565b6100cb6100e3565b005b3480156100d957600080fd5b506100b560005481565b6000805481806100f283610141565b90915550600090815260016020526040902080546001600160a01b0319163317905550565b60006020828403121561012957600080fd5b813560ff8116811461013a57600080fd5b9392505050565b60006001820161016157634e487b7160e01b600052601160045260246000fd5b506001019056fea26469706673582212202005cedcbbdfb7b128fad8a6aae04c5bc1a5515315830fc6aedcd255209bfb2164736f6c63430008110033"
        
        new() = FaucetDeployment(BYTECODE)
        

        
    
    [<Function("addFunds")>]
    type AddFundsFunction() = 
        inherit FunctionMessage()
    

        
    
    [<Function("getFunderAtIndex", "address")>]
    type GetFunderAtIndexFunction() = 
        inherit FunctionMessage()
    
            [<Parameter("uint8", "index", 1)>]
            member val public Index = Unchecked.defaultof<byte> with get, set
        
    
    [<Function("justTesting", "uint256")>]
    type JustTestingFunction() = 
        inherit FunctionMessage()
    

        
    
    [<Function("numOfFunders", "uint256")>]
    type NumOfFundersFunction() = 
        inherit FunctionMessage()
    

        
    
    
    
    [<FunctionOutput>]
    type GetFunderAtIndexOutputDTO() =
        inherit FunctionOutputDTO() 
            [<Parameter("address", "", 1)>]
            member val public ReturnValue1 = Unchecked.defaultof<string> with get, set
        
    
    [<FunctionOutput>]
    type JustTestingOutputDTO() =
        inherit FunctionOutputDTO() 
            [<Parameter("uint256", "", 1)>]
            member val public ReturnValue1 = Unchecked.defaultof<BigInteger> with get, set
        
    
    [<FunctionOutput>]
    type NumOfFundersOutputDTO() =
        inherit FunctionOutputDTO() 
            [<Parameter("uint256", "", 1)>]
            member val public ReturnValue1 = Unchecked.defaultof<BigInteger> with get, set
    


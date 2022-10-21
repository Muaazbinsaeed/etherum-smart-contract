namespace Faucet.Contracts.Faucet

open System
open System.Threading.Tasks
open System.Collections.Generic
open System.Numerics
open Nethereum.Hex.HexTypes
open Nethereum.ABI.FunctionEncoding.Attributes
open Nethereum.Web3
open Nethereum.RPC.Eth.DTOs
open Nethereum.Contracts.CQS
open Nethereum.Contracts.ContractHandlers
open Nethereum.Contracts
open System.Threading
open Faucet.Contracts.Faucet.ContractDefinition


    type FaucetService (web3: Web3, contractAddress: string) =
    
        member val Web3 = web3 with get
        member val ContractHandler = web3.Eth.GetContractHandler(contractAddress) with get
    
        static member DeployContractAndWaitForReceiptAsync(web3: Web3, faucetDeployment: FaucetDeployment, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> = 
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            web3.Eth.GetContractDeploymentHandler<FaucetDeployment>().SendRequestAndWaitForReceiptAsync(faucetDeployment, cancellationTokenSourceVal)
        
        static member DeployContractAsync(web3: Web3, faucetDeployment: FaucetDeployment): Task<string> =
            web3.Eth.GetContractDeploymentHandler<FaucetDeployment>().SendRequestAsync(faucetDeployment)
        
        static member DeployContractAndGetServiceAsync(web3: Web3, faucetDeployment: FaucetDeployment, ?cancellationTokenSource : CancellationTokenSource) = async {
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            let! receipt = FaucetService.DeployContractAndWaitForReceiptAsync(web3, faucetDeployment, cancellationTokenSourceVal) |> Async.AwaitTask
            return new FaucetService(web3, receipt.ContractAddress);
            }
    
        member this.AddFundsRequestAsync(addFundsFunction: AddFundsFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(addFundsFunction);
        
        member this.AddFundsRequestAndWaitForReceiptAsync(addFundsFunction: AddFundsFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(addFundsFunction, cancellationTokenSourceVal);
        
        member this.GetFunderAtIndexQueryAsync(getFunderAtIndexFunction: GetFunderAtIndexFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<GetFunderAtIndexFunction, string>(getFunderAtIndexFunction, blockParameterVal)
            
        member this.JustTestingQueryAsync(justTestingFunction: JustTestingFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<JustTestingFunction, BigInteger>(justTestingFunction, blockParameterVal)
            
        member this.NumOfFundersQueryAsync(numOfFundersFunction: NumOfFundersFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<NumOfFundersFunction, BigInteger>(numOfFundersFunction, blockParameterVal)
            
    


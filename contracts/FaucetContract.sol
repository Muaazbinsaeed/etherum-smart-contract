// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;
import "./Owned.sol";
import "./Logger.sol";
import "./IFaucet.sol";

contract Faucet is Owned, Logger, IFaucet {
    uint256 public numOfFunders;
    mapping(address => bool) private funders;
    mapping(uint256 => address) private lutFunders; //lut->lookupTable

    modifier limitWithdraw(uint256 withdrawAmount) {
        require(
            withdrawAmount <= 1000000000000000000,
            "Cannot withdraw more than 0.1 ether"
        );
        _;
    }

    //private-> can be accesible only within the smart contract

    //internal-> can be accesible within smart contact and also derived smart contract

    receive() external payable {}

    function emitLog() public pure override returns (bytes32) {
        return "Hello World";
    }

    function transferOwnership(address newOwner) external onlyOwner {
        owner = newOwner;
    }

    function addFunds() external payable override {
        address funder = msg.sender;
        if (!funders[funder]) {
            numOfFunders++;
            funders[funder] = true;
            lutFunders[numOfFunders] = funder;
        }
    }

    function test1() external onlyOwner {
        //some managing stuff that only admin has access
    }

    function test2() external onlyOwner {
        //some managing stuff that only admin has access
    }

    function justTesting() external pure returns (uint256) {
        return 2 + 2;
    }

    function withdraw(uint256 withdrawAmount)
        external
        override
        limitWithdraw(withdrawAmount)
    {
        payable(msg.sender).transfer(withdrawAmount);
    }

    function getAllFunders() external view returns (address[] memory) {
        address[] memory _funders = new address[](numOfFunders);
        for (uint256 index = 0; index < numOfFunders; index++) {
            _funders[index] = lutFunders[index];
        }
        return _funders;
    }

    function getFunderAtIndex(uint8 index) external view returns (address) {
        return lutFunders[index];
    }

    //pure,view-read-only-call,no gas fee
    //view-it indicates that the function will not alter the storage state in any way
    //pure -even more strict,indication that it wont event read the storage state
    //Transactions (can generate state changes) and require gas free
    //read-only call,no gas free
    //to talk to the node on network you can make JSON-RPC http calls
    //const instance= await Faucet.deployed();
    // instance.addFunds({value:"2000000000000000000",from:accounts[0]})
    // instance.withdraw("5000000000000000000",{from:"0x1DDEFfc049989Ef088D64fAc6bFBCC6b33D6e417"})
}

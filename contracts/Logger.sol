// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

//It's a way for designer to say that any child of the abstract contract has to implement the specified methods

abstract contract Logger {
    uint256 public testNum;

    constructor() {
        testNum = 1000;
    }

    function emitLog() public virtual returns (bytes32);

    function test3() external pure returns (uint256) {
        return 100;
    }
}

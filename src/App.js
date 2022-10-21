import { useCallback, useEffect, useState } from "react";
import "./App.css";
import Web3 from "web3";
import detectEthereumProvider from "@metamask/detect-provider";
import { loadContract } from "./utils/load-contract";

function App() {
  const [web3Api, setWeb3Api] = useState({
    provider: null,
    web3: null,
    contract: null,
  });
  const [balance, setBalance] = useState(null);
  const [account, setAccount] = useState(null);
  const [shouldReloadEffect, setShouldReloadEffect] = useState(false);

  const reloadEffect = useCallback(() => {
    setShouldReloadEffect(!shouldReloadEffect);
  }, [shouldReloadEffect]);
  const setAccountListener = (provider) => {
    provider.on("accountsChanged", (_) => window.location.reload());
    provider._jsonRpcConnection.events.on("notification", (payload) => {
      const { method } = payload;
      if (method === "metamask_unlockStateChanged") {
        setAccount(null);
      }
    });
  };
  useEffect(() => {
    const loadProvider = async () => {
      const provider = await detectEthereumProvider();
      const contract = await loadContract("Faucet", provider);
      // with metamask we have an access to window.etherum & to window/web3
      //metamask injects a global API into website
      //this API allows websites to request users,account,read data to blockchain
      //sign messages and transaction
      if (provider) {
        // await provider.request({ method: "eth_requestAccounts" });
        setAccountListener(provider);
        setWeb3Api({
          web3: new Web3(provider),
          provider,
          contract,
        });
      } else {
        console.error("Please install MetaMask!");
      }
      // if (window.ethereum) {
      //   provider = window.ethereum;
      //   try {
      //     await provider.request({ method: "eth_requestAccounts" });
      //   } catch (error) {
      //     console.log("User denied accounts access!");
      //   }
      // } else if (window.web3) {
      //   provider = window.web3.currentProvider;
      // } else if (!process.env.production) {
      //   provider = new Web3.providers.HttpProvider("http:localhost:7545");
      // }
      // setWeb3Api({
      //   web3: new Web3(provider),
      //   provider: provider,
      // });
    };
    loadProvider();
    return () => {
      setWeb3Api({
        provider: null,
        web3: null,
      });
    };
  }, []);

  useEffect(() => {
    let loadBalance = async () => {
      const { contract, web3 } = web3Api;
      const balance = await web3.eth.getBalance(contract.address);
      setBalance(web3.utils.fromWei(balance, "ether"));
    };
    web3Api.contract && loadBalance();
    return () => {};
  }, [web3Api, shouldReloadEffect]);

  useEffect(() => {
    let getAccount = async () => {
      const accounts = await web3Api.web3.eth.getAccounts();
      setAccount(accounts[0]);
    };
    web3Api.web3 && getAccount();
    return () => {
      setAccount(null);
    };
  }, [web3Api.web3]);

  let addFunds = useCallback(async () => {
    const { contract, web3 } = web3Api;
    await contract.addFunds({
      from: account,
      value: web3.utils.toWei("1", "ether"),
    });
    reloadEffect();
  }, [web3Api, account, reloadEffect]);
  let withDrawFunds = async () => {
    const { contract, web3 } = web3Api;
    let withdrawAmount = web3.utils.toWei("0.1", "ether");
    await contract.withdraw(withdrawAmount, {
      from: account,
    });
    reloadEffect();
  };
  console.log({ web3Api });
  return (
    <div className="faucet-wrapper">
      <div className="faucet">
        <div className="is-flex is-align-items-center">
          <span>
            <strong className="mr-2">Account:</strong>
          </span>
          {account ? (
            <div>{account}</div>
          ) : (
            <button
              onClick={() => {
                web3Api.provider.request({ method: "eth_requestAccounts" });
              }}
              className="button is-small"
            >
              Connect Wallet
            </button>
          )}
        </div>
        <div className="balance-view is-size-2 mb-4">
          Current Balance:<strong>{balance}</strong> ETH
        </div>
        {/* <button
          onClick={async () => {
            const accounts = await window.ethereum.request({
              method: "eth_requestAccounts",
            });
            console.log({ accounts });
          }}
          className="button mr-2"
        >
          Enable Ethereum
        </button> */}

        <button
          disabled={!account}
          onClick={addFunds}
          className="button is-link mr-2"
        >
          Donate 1 eth
        </button>
        <button
          disabled={!account}
          onClick={withDrawFunds}
          className="button is-primary "
        >
          Withdraw
        </button>
      </div>
    </div>
  );
}

export default App;

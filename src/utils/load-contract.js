import contract from "@truffle/contract";

export const loadContract = async (name, provider) => {
  try {
    let res = await fetch(`/contracts/${name}.json`);
    const Artificat = await res.json();
    const _contract = contract(Artificat);
    _contract.setProvider(provider);
    const deployedContract = await _contract.deployed();
    return deployedContract;
  } catch (error) {
    console.log({ error });
  }
};

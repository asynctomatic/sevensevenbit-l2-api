#!/bin/sh

export FOUNDRY_PROFILE=default && \
forge build && \
export FOUNDRY_PROFILE=modules && \
forge build && \

export FOUNDRY_PROFILE=default && \
forge script script/scalable-dex/DeployStarkEx.s.sol:DeployStarkExScript \
	--rpc-url http://blockchain:8545 \
	--private-key 0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80 \
	--broadcast && \
	
export FOUNDRY_PROFILE=modules && \
forge script script/modules/DeployMintableModule.s.sol:DeployMintableModuleScript \
	--rpc-url http://blockchain:8545 \
	--private-key 0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80 \
	--broadcast && \
	
export FOUNDRY_PROFILE=modules && \
forge script script/modules/DeployFaucetsModule.s.sol:DeployFaucetsModuleScript \
	--rpc-url http://blockchain:8545 \
	--private-key 0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80 \
	--broadcast && \

sleep 10


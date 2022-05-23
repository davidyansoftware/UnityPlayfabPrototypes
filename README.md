# UnityPlayfabPrototypes

A collection of proof-of-concepts for integrating Playfab and Unity.

# Simulated Battle/Leaderboard

Uses playfab leaderboard system to create battle system between players.

Potential opponents are selected based on relative powerlevel, player can choose one to battle.

Battles are handled in a server-authoritative manner, and return updated ratings for the player.

See https://github.com/davidyansoftware/AzureFunctionsPrototypes for how battles are executed via server-less calls

# Chat
Uses playfab multiplayer servers to host a lightweight custom chat server.

Server is built seperately from client and is deployed to Playfab. Official documentation: https://docs.microsoft.com/en-us/gaming/playfab/features/multiplayer/servers/

Clients can submit messages to the chat server, and the server pushes incoming messages to each other client.

Clients read incoming messages and print to screen.

# Time

Get authoritative server time for in-game functions to prevent cheating via local clock changes.

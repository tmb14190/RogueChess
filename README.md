This is a chess rogue like built in Monogame (XNA).
The code is structured in a way to work like normal chess, but with extremely modifiable pieces and rules.
With this in mind, the code is setup with a Rules class and a Buffs class, which can be expanded
to create a rogue like game within a chess environment. In this currently basic version of chess
unique piece abilities are treated as buffs, e.g. castling is a king buff, en passant and 
pawn upgrade are pawn buffs, and check is treated as a rule. This allows for  different chess 
variants to be implemented, and combining this with a rogue like deck building style, with rng and 
predicative strategy, to create what is in essence a sophisticated chess puzzle game.

# RevertFroggyNerf Changelog  

## 1.0.2  
 - Fixed mod for clients due to latest game update.  
	- Host no longer has authority over all client's ``maxAdditiveExtraAirJumps `` value. So clients need to set it themselves.  
	- This means the mod is now required by all clients.  
	- Also had to switch to a standard harmony patch rather than using the player spawn event from GameManager

## 1.0.1
 - Fixed mod not applying changes for clients (non-host players)

## 1.0.0
 - initial release
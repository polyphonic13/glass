# Conventions

## Definitions

* Scene: a Unity scene
* Level: a Scene that involves a Player / game state progression
* Section: a portion of a Level with grouped, related game objects and an optional PlayerPosition

## Naming Conventions

* <Name>Data: serializable class/struct
* <Name>Controller: management of mutiple instances of <Name> class/component
* <Name>Agent: component of a GameObject controlling <Name> functionality
* Trigger: anything related to OnTriggerEnter
* Switch: component whose Actuate method triggers some action
* Reaction: sibling component that calls Actuate via SendMessage


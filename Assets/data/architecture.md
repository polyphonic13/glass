==================================
Legend

* ^ 	Serializable
* =		Singleton
* ~ 	MonoBehaviour
* %		Prefab
+ #		Persistent
* -_n_	References 

==================================


- GameController%#
	- Game~=
		- GameData^
			-1
			-2
			-3
			-4
	- Inventory~=
		- Item~
			- ItemData^ -1
	
- SceneController*
	- SceneController~=
		- ScenePrefabController~
			- ScenePrefabData
				- Prefab
					-2
		- TaskController~
			- Task -3
		- SceneData -4
		
I. Game Start
		
II. Scene

1. Scene Load
2. [Game] checks GameData for current scene name key, assigning `sceneData`
3. [Game] calls SceneController.Init, passing `sceneData`
4. [SceneController] Init calls ScenePrefabController

III. Scene Change


IV. Load

V. 	Save
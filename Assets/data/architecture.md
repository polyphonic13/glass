==================================
__Legend__

* ^ 	Serializable
* =		Singleton
* ~ 	MonoBehaviour
* %		Prefab
+ #		Persistent
* -_n_	References 

==================================

__Structure__

- GameController%#
	- [Game][1]~=
		- GameData^
			-1
			-2
			-3
			-4
	- Inventory~=
		- Item~
			- ItemData^ -1
	
- SceneController*
	- [SceneController][2]~=
		- ScenePrefabController~
			- ScenePrefabData
				- Prefab
					-2
		- TaskController~
			- Task -3
		- SceneData -4
		
__I. Game Start__
		
__II. Scene__

1. Scene Load
2. [1]: [Game] checks GameData for current scene name key, assigning `sceneData`
3. [1]: [Game] calls SceneController.Init, passing `sceneData`
4. [2]: [SceneController] Init calls ScenePrefabController

__III. Scene Change__

__IV. Load__

__V. Save__
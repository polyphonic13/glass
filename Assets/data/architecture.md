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
	- [1]: Game~=
		- GameData^
			-1
			-2
			-3
			-4
	- Inventory~=
		- Item~
			- ItemData^ -1
	
- SceneController*
	- [2]: SceneController~=
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
2. [Game][1] checks GameData for current scene name key, assigning `sceneData`
3. [Game][1] calls SceneController.Init, passing `sceneData`
4. [SceneController][2] Init calls ScenePrefabController

__III. Scene Change__

__IV. Load__

__V. Save__
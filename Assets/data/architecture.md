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
2. [Game](#Game) checks GameData for current scene name key, assigning `sceneData`
3. [Game](#Game) calls SceneController.Init, passing `sceneData`
4. [SceneController][2] Init calls ScenePrefabController.Init passing `scenePrefabData`
5. 

__III. Scene Change__

__IV. Load__

__V. Save__
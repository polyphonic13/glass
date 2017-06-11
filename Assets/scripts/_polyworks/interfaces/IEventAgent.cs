namespace Polyworks {

	using System;

	public interface IEventAgent {
		void Enable();
		void Disable();
		void AddEventListeners();
		void RemoveEventListeners();
	}
}


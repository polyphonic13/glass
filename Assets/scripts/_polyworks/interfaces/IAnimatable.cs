using System; 

namespace Polyworks {
	public interface IAnimatable {
		void Play();
		void Pause();
		void Resume();
		bool GetIsActive();
	}
	
}
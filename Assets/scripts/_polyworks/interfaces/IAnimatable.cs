using System; 

namespace Polyworks {
	public interface IAnimatable {
		void Play(string clip);
		void Pause();
		void Resume();
		bool GetIsActive();
	}
	
}
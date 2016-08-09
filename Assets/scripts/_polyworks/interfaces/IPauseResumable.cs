using System; 

namespace Polyworks {
	public interface IPauseResumable {
		void Pause;
		void Resume;
		bool GetIsActive;
	}
	
}
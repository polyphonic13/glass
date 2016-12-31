namespace Polyworks {

	using System;

	public interface IInputControllable {
		void SetVertical(float vertical);
		void SetHorizontal(float horizontal);
		void SetInput(InputObject input);
	}
}


using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorInputForLook = true;

		public event Action OnPauseInput;

		private float _mouseSenstivity = 1.0f;

		#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (PauseState.IsPaused)
			{
				MoveInput(Vector2.zero);
				return;
			}

			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (PauseState.IsPaused)
			{
				LookInput(Vector2.zero);
				return;
			}

			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (PauseState.IsPaused) return;

			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnPause(InputValue value)
		{
			if (value.isPressed)
			{
				OnPauseInput?.Invoke();
			}
		}
		#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection * _mouseSenstivity;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void SetMouseSensitivity(float newSensitivity)
		{
			_mouseSenstivity = newSensitivity;
		}
	}
}

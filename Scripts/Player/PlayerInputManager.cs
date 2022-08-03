using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool AnalogMovement;

    [Header("Mouse Cursor Settings")]
    public bool CursorLocked = true;
    public bool CursorInputForLook = true;

    public Vector2 Move { get; set; }
    public Vector2 Look { get; set; }
    public bool Jump { get; set; }
    public bool Sprint { get; set; }
    public bool Interact { get; set; }
    public bool FireAction { get; set; }
	public bool BuildMenu { get; set; } = false;
	public bool MiddleMouse { get; set; }
	public float Scroll { get; set; }

	private int switchAction;

	public int GetSwitchAction()
    {
		int currentSwitchAction = switchAction;

		switchAction = 0;
		return currentSwitchAction;
    }

	// Action Triggers
	private void OnMove(InputValue value)
	{
		Move = value.Get<Vector2>();
	}

	private void OnLook(InputValue value)
	{
		Look = value.Get<Vector2>();
	}

	private void OnJump(InputValue value)
	{
		Jump = value.isPressed;
	}

	private void OnSprint(InputValue value)
	{
		Sprint = value.isPressed;
	}

	private void OnInteract(InputValue value)
	{
		if (CursorInputForLook)
		{
			Interact = value.isPressed;
		}
	}

	private void OnFireAction(InputValue value)
	{
		FireAction = value.isPressed;
	}

	private void OnBuildMenu()
    {
		BuildMenu = !BuildMenu;
		// TODO: Put this somewhere else
		GameManager.Instance.LogController.AddLog(LogEntryType.Info, "Build Mode " + (BuildMenu ? "Enabled" : "Disabled") + ".");
	}

	private void OnScroll(InputValue value)
    {
		Scroll = value.Get<float>();
    }

	private void OnSwitchObjectRight(InputValue value)
    {
		switchAction = 1;
    }

	private void OnSwitchObjectLeft(InputValue value)
    {
		switchAction = -1;
    }

	private void OnMiddleMouse(InputValue value)
    {
		MiddleMouse = value.isPressed;
    }

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(CursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}

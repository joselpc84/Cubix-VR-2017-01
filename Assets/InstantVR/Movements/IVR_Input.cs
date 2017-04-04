/* InstantVR Input
 * author: Pascal Serrarnes
 * email: support@passervr.com
 * version: 3.7.1
 * date: January 22, 2017
 * 
 * - Fixed PlayMaker issues
 */

using UnityEngine;

namespace IVR {

    public static class Controllers {
        public static ControllerInput[] controllers;

        public static void Update() {
            if (controllers != null) {
                for (int i = 0; i < controllers.Length; i++)
                    controllers[i].Update();
            }
        }

        public static ControllerInput GetController(int playerID) {
            if (controllers == null) {
                controllers = new ControllerInput[1];
                controllers[0] = new ControllerInput();
            }
            return controllers[0];
        }

        public static void Clear() {
            if (controllers != null) {
                for (int i = 0; i < controllers.Length; i++)
                    controllers[i].Clear();
            }
        }
    }

    public class ControllerInput {
        public enum Side {
            Left,
            Right
        }
        public enum Button {
            ButtonOne = 0,
            ButtonTwo = 1,
            ButtonThree = 2,
            ButtonFour = 3,
            Bumper = 10,
            BumperTouch = 11,
            Trigger = 12,
            TriggerTouch = 13,
            StickButton = 14,
            StickTouch = 15,
            Up = 20,
            Down = 21,
            Left = 22,
            Right = 23,
            Option = 30,
            None = 9999
        }

        public ControllerInputSide left;
        public ControllerInputSide right;

        public void Update() {
            left.Update();
            right.Update();
        }

        public ControllerInput() {
            left = new ControllerInputSide();
            right = new ControllerInputSide();
        }

        public void Clear() {
            Update();
            left.Clear();
            right.Clear();
        }

        public bool GetButton(Side side, Button buttonID) {
            switch (side) {
                case Side.Left:
                    return left.GetButton(buttonID);
                case Side.Right:
                    return right.GetButton(buttonID);
                default:
                    return false;

            }
        }
    }

    public class ControllerInputSide {
        public float stickHorizontal;
        public float stickVertical;
        public bool stickButton;
        public bool stickTouch;
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public bool[] buttons = new bool[4];

        public float bumper;
        public float trigger;

        public bool option;

        public event OnButtonDown OnButtonDownEvent;
        public event OnButtonUp OnButtonUpEvent;

        public delegate void OnButtonDown(ControllerInput.Button buttonNr);
        public delegate void OnButtonUp(ControllerInput.Button buttonNr);

        private bool[] lastButtons = new bool[4];
        private bool lastBumper;
        private bool lastTrigger;
        private bool lastStickButton;
        private bool lastOption;

        public void Update() {
            for (int i = 0; i < 4; i++) {
                if (buttons[i] && !lastButtons[i]) {
                    if (OnButtonDownEvent != null)
                        OnButtonDownEvent((ControllerInput.Button) i);

                } else if (!buttons[i] && lastButtons[i]) {
                    if (OnButtonUpEvent != null)
                        OnButtonUpEvent((ControllerInput.Button) i);
                }
                lastButtons[i] = buttons[i];
            }

            if (bumper > 0.9F && !lastBumper) {
                if (OnButtonDownEvent != null)
                    OnButtonDownEvent(ControllerInput.Button.Bumper);
                lastBumper = true;
            } else if (bumper < 0.1F && lastBumper) {
                if (OnButtonUpEvent != null)
                    OnButtonUpEvent(ControllerInput.Button.Bumper);
                lastBumper = false;
            }

            if (trigger > 0.9F && !lastTrigger) {
                if (OnButtonDownEvent != null)
                    OnButtonDownEvent(ControllerInput.Button.Trigger);
                lastTrigger = true;
            } else if (trigger < 0.1F && lastTrigger) {
                if (OnButtonUpEvent != null)
                    OnButtonUpEvent(ControllerInput.Button.Trigger);
                lastTrigger = false;
            }

            if (stickButton && !lastStickButton) {
                if (OnButtonDownEvent != null)
                    OnButtonDownEvent(ControllerInput.Button.StickButton);
            } else if (!stickButton && lastStickButton) {
                if (OnButtonUpEvent != null)
                    OnButtonUpEvent(ControllerInput.Button.StickButton);
            }
            lastStickButton = stickButton;

            if (option && !lastOption) {
                if (OnButtonDownEvent != null)
                    OnButtonDownEvent(ControllerInput.Button.Option);
            } else if (!option && lastOption) {
                if (OnButtonUpEvent != null)
                    OnButtonUpEvent(ControllerInput.Button.Option);
            }
            lastOption = option;
        }

        public void Clear() {
            stickHorizontal = 0;
            stickVertical = 0;
            stickButton = false;
            stickTouch = true;

            up = false;
            down = false;
            left = false;
            right = false;

            for (int i = 0; i < 4; i++)
                buttons[i] = false;

            bumper = 0;
            trigger = 0;

            option = false;
        }

        public bool GetButton(ControllerInput.Button buttonID) {
            switch (buttonID) {
                case ControllerInput.Button.ButtonOne:
                    return buttons[0];
                case ControllerInput.Button.ButtonTwo:
                    return buttons[1];
                case ControllerInput.Button.ButtonThree:
                    return buttons[2];
                case ControllerInput.Button.ButtonFour:
                    return buttons[3];
                case ControllerInput.Button.Bumper:
                    return bumper > 0.9F;
                case ControllerInput.Button.Trigger:
                    return trigger > 0.9F;
                case ControllerInput.Button.StickButton:
                    return stickButton;
                case ControllerInput.Button.StickTouch:
                    return stickTouch;
                case ControllerInput.Button.Option:
                    return option;
                case ControllerInput.Button.Up:
                    return up;
                case ControllerInput.Button.Down:
                    return down;
                case ControllerInput.Button.Left:
                    return left;
                case ControllerInput.Button.Right:
                    return right;
                default:
                    return false;
            }
        }
    }
}

#if PLAYMAKER
namespace HutongGames.PlayMaker.Actions {

    [ActionCategory("InstantVR")]
    [Tooltip("Controller input axis")]
    public class GetControllerAxis : FsmStateAction {

        [RequiredField]
        [Tooltip("Left or right (side) controller")]
        public BodySide controllerSide = BodySide.Left;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the direction vector.")]
        public FsmVector3 storeVector;

        private IVR.ControllerInput controller0;

        public override void Awake() {
            controller0 = IVR.Controllers.GetController(0);
        }

        public override void OnUpdate() {
            IVR.ControllerInputSide controller = (controllerSide == BodySide.Left) ? controller0.left : controller0.right;

            storeVector.Value = new Vector3(controller.stickHorizontal, 0, controller.stickVertical);
        }
    }

    [ActionCategory("InstantVR")]
    [Tooltip("Controller input button")]
    public class GetControllerButton : FsmStateAction {

        [RequiredField]
        [Tooltip("Left or right (side) controller")]
        public BodySide controllerSide = BodySide.Right;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Controller Button")]
        public IVR.ControllerInput.Button button;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store Result Bool")]
        public FsmBool storeBool;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store Result Float")]
        public FsmFloat storeFloat;

        [Tooltip("Event to send when the button is pressed.")]
        public FsmEvent buttonPressed;

        [Tooltip("Event to send when the button is released.")]
        public FsmEvent buttonReleased;

        private IVR.ControllerInput controller0;

        public override void Awake() {
            controller0 = IVR.Controllers.GetController(0);
        }

        public override void OnUpdate() {
            IVR.ControllerInputSide controller = (controllerSide == BodySide.Left) ? controller0.left : controller0.right;

            bool oldBool = storeBool.Value;

            switch (button) {
                case IVR.ControllerInput.Button.StickButton:
                    storeBool.Value = controller.stickButton;
                    storeFloat.Value = controller.stickButton ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.Up:
                    storeBool.Value = controller.up;
                    storeFloat.Value = controller.up ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.Down:
                    storeBool.Value = controller.down;
                    storeFloat.Value = controller.down ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.Left:
                    storeBool.Value = controller.left;
                    storeFloat.Value = controller.left ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.Right:
                    storeBool.Value = controller.right;
                    storeFloat.Value = controller.right ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.ButtonOne:
                    storeBool.Value = controller.buttons[0];
                    storeFloat.Value = controller.buttons[0] ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.ButtonTwo:
                    storeBool.Value = controller.buttons[1];
                    storeFloat.Value = controller.buttons[1] ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.ButtonThree:
                    storeBool.Value = controller.buttons[2];
                    storeFloat.Value = controller.buttons[2] ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.ButtonFour:
                    storeBool.Value = controller.buttons[3];
                    storeFloat.Value = controller.buttons[3] ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.Option:
                    storeBool.Value = controller.option;
                    storeFloat.Value = controller.option ? 1 : 0;
                    break;
                case IVR.ControllerInput.Button.Bumper:
                    storeBool.Value = controller.bumper > 0.9F;
                    storeFloat.Value = controller.bumper;
                    break;
                case IVR.ControllerInput.Button.Trigger:
                    storeBool.Value = controller.trigger > 0.9F;
                    storeFloat.Value = controller.trigger;
                    break;
            }

            if (storeBool.Value && !oldBool)
                Fsm.Event(buttonPressed);
            else if (!storeBool.Value && oldBool)
                Fsm.Event(buttonReleased);
        }
    }
}
#endif
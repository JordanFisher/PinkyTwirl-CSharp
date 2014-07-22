using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PinkyGame
{
    public class Manager
    {
        static PinkyGame game = null;

        public static void Start()
        {
            new Thread(() =>
            {
                game = new PinkyGame();
                game.Run();
            }).Start();
        }

        public static void End()
        {
            if (game != null)
            {
                game.Exit();
            }
        }

        public static event Action<GamePadState> OnLeftJoystickMove;
        internal static void DoLeftJoystickMove(GamePadState state) { if (OnLeftJoystickMove != null) OnLeftJoystickMove(state); }

        public static event Action<GamePadState> OnRightJoystickMove;
        internal static void DoRightJoystickMove(GamePadState state) { if (OnRightJoystickMove != null) OnRightJoystickMove(state); }

        public static event Action<GamePadState> OnButtonPress;
        internal static void DoButtonPress(GamePadState state) { if (OnButtonPress != null) OnButtonPress(state); }

        public static event Action<GamePadState> OnButtonRelease;
        internal static void DoButtonRelease(GamePadState state) { if (OnButtonRelease != null) OnButtonRelease(state); }
    }

    public static class ButtonExtension
    {
        public static bool Down(this ButtonState state)
        {
            return state == ButtonState.Pressed;
        }

        public static bool Pressed(this Buttons button)
        {
            return InputInfo.CurGamepad.IsButtonDown(button) && InputInfo.PrevGamepad.IsButtonUp(button);
        }

        public static bool Released(this Buttons button)
        {
            return InputInfo.CurGamepad.IsButtonUp(button) && InputInfo.PrevGamepad.IsButtonDown(button);
        }

        public static bool Down(this Buttons button)
        {
            return InputInfo.CurGamepad.IsButtonDown(button);
        }
    }

    public static class InputInfo
    {
        public static GamePadState CurGamepad, PrevGamepad;
    }

    public class PinkyGame : Game
    {
        System.Windows.Forms.Control form;
        public PinkyGame()
        {
            form = System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.VisibleChanged += gameForms_VisibleChanged;
        }

        void gameForms_VisibleChanged(object sender, EventArgs e)
        {
            if (form.Visible)
                form.Visible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            InputInfo.PrevGamepad = InputInfo.CurGamepad;
            InputInfo.CurGamepad = GamePad.GetState(PlayerIndex.One);

            InputInfo.CurGamepad = GamePad.GetState(PlayerIndex.One);

            if (InputInfo.CurGamepad.ThumbSticks.Left.Length() > .05f)
                Manager.DoLeftJoystickMove(InputInfo.CurGamepad);

            if (InputInfo.CurGamepad.ThumbSticks.Right.Length() > .05f)
                Manager.DoRightJoystickMove(InputInfo.CurGamepad);

            if (Buttons.A.Pressed() || Buttons.B.Pressed() || Buttons.X.Pressed() || Buttons.Y.Pressed() || Buttons.LeftShoulder.Pressed() || Buttons.RightShoulder.Pressed())
                Manager.DoButtonPress(InputInfo.CurGamepad);

            if (Buttons.A.Released() || Buttons.B.Released() || Buttons.X.Released() || Buttons.Y.Released() || Buttons.LeftShoulder.Released() || Buttons.RightShoulder.Released())
                Manager.DoButtonRelease(InputInfo.CurGamepad);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}

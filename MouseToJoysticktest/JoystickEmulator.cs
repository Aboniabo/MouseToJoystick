using System.ComponentModel;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Exceptions;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace MouseToJoysticktest;

public partial class JoystickEmulator : Form
{
    private ViGEmClient _vigemClient;
    private IXbox360Controller _controller;
    private bool _isControllerConnected = false; // Flag to track connection state

    private Point _screenCenter;
    private int _screenWidth;
    private int _screenHeight;

    // --- Constants ---
    private const short JOYSTICK_MIN = short.MinValue; // -32768
    private const short JOYSTICK_MAX = short.MaxValue; //  32767
    private const short JOYSTICK_CENTER = 0;

    public JoystickEmulator()
    {
        InitializeComponent();
    }

    private void JoystickEmulator_Load(object sender, EventArgs e)
    {
        Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
        _screenWidth = screenBounds.Width;
        _screenHeight = screenBounds.Height;
        _screenCenter = new Point(_screenWidth / 2, _screenHeight / 2);

        lblScreenCenter.Text = $"Center: ({_screenCenter.X}, {_screenCenter.Y})";
        lblStatus.Text = "Status: Ready. ViGEmBus driver must be installed.";
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        try
        {
            // --- Initialize ViGEm Client (only once) ---
            if (_vigemClient == null)
            {
                _vigemClient = new ViGEmClient();
            }

            // --- Create Controller (only once) ---
            if (_controller == null)
            {
                _controller = _vigemClient.CreateXbox360Controller();
            }

            // --- Connect Controller (if not already connected) ---
            if (!_isControllerConnected)
            {
                _controller.Connect(); // This can throw if connection fails
                _isControllerConnected = true; // Set flag AFTER successful connection
            }

            lblStatus.Text = "Status: Running";
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            timer1.Enabled = true;
        }
        catch (VigemBusNotFoundException)
        {
            MessageBox.Show("ViGEmBus driver is not installed. Please install it and restart the application.", "Driver Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Status: Error - ViGEmBus Driver Not Found";
            CleanupResources(); // Clean up if init fails
        }
        catch (Exception ex) // Catch other potential errors during init/connect
        {
            MessageBox.Show($"An error occurred during startup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = $"Status: Error - {ex.GetType().Name}";
            _isControllerConnected = false; // Ensure flag is false on error
            CleanupResources(); // Attempt cleanup on error
            btnStart.Enabled = true; // Allow retry
            btnStop.Enabled = false;
            timer1.Enabled = false;
        }
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        StopEmulation();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        // Check if controller exists and we think it's connected
        if (_controller == null || !_isControllerConnected)
        {
            // Should ideally not happen if StopEmulation logic is correct, but acts as a safeguard
            StopEmulation();
            lblStatus.Text = "Status: Stopped (Controller invalid state)";
            return;
        }

        try
        {
            // --- Get Mouse Position ---
            Point mousePos = Cursor.Position;
            lblMousePos.Text = $"Mouse: ({mousePos.X}, {mousePos.Y})";

            // --- Calculate Delta from Center ---
            double deltaX = mousePos.X - _screenCenter.X;
            double deltaY = mousePos.Y - _screenCenter.Y;
            double normY = -deltaY; // Invert Y-axis

            // --- Map Delta to Joystick Range ---
            double maxDeltaX = _screenWidth / 2.0;
            double maxDeltaY = _screenHeight / 2.0;
            double normalizedX = (maxDeltaX > 0) ? deltaX / maxDeltaX : 0;
            double normalizedY = (maxDeltaY > 0) ? normY / maxDeltaY : 0;
            normalizedX = Math.Max(-1.0, Math.Min(1.0, normalizedX));
            normalizedY = Math.Max(-1.0, Math.Min(1.0, normalizedY));

            // --- Scale to Short Range ---
            short joystickX = (short)(normalizedX * JOYSTICK_MAX);
            short joystickY = (short)(normalizedY * JOYSTICK_MAX);

            // --- Update Virtual Controller ---
            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, joystickX);
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, joystickY);
            // If other controls were added, you might need _controller.SubmitReport();

            lblJoystickPos.Text = $"Joystick: ({joystickX}, {joystickY})";
        }
        // Catch exceptions that indicate the connection is lost or device is invalid
        catch (VigemTargetNotPluggedInException ex)
        {
            Console.WriteLine($"Controller disconnected or invalid: {ex.Message}");
            StopEmulationWithError("Error - Controller Disconnected");
        }
        catch (Exception ex) // Catch other potential errors during update
        {
            Console.WriteLine($"Error updating controller: {ex.Message}");
            StopEmulationWithError($"Error updating - {ex.GetType().Name}");
        }
    }


    private void StopEmulation()
    {
        timer1.Enabled = false; // Stop the timer 

        // Reset joystick to center if controller exists and we think it's connected
        if (_controller != null && _isControllerConnected)
        {
            try
            {
                _controller.ResetReport();
            }
            catch (VigemTargetNotPluggedInException) { /* Expected if already disconnected */ }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting controller: {ex.Message}");
                // Continue stopping even if reset fails
            }
        }

        lblStatus.Text = "Status: Stopped";
        lblJoystickPos.Text = "Joystick: (0, 0)";
        btnStart.Enabled = true;
        btnStop.Enabled = false;

    }

    // Helper for stopping when an error occurs during Ticks
    private void StopEmulationWithError(string statusMessage)
    {
        StopEmulation(); // Perform standard stop actions
        _isControllerConnected = false; // Mark as disconnected due to error
        lblStatus.Text = $"Status: {statusMessage}";
    }
    
    private void CleanupResources()
    {
        // Disconnect controller only if it exists and we believe it's connected
        if (_controller != null && _isControllerConnected)
        {
            try
            {
                _controller.Disconnect();
            }
            catch (VigemTargetNotPluggedInException) {}
            catch (Exception ex)
            {
                Console.WriteLine($"Error disconnecting controller: {ex.Message}");
            }
            finally
            {
                _isControllerConnected = false;
            }
        }
        if (_vigemClient != null && !this.IsDisposed && this.Disposing)
        {
            try { _vigemClient.Dispose(); }
            catch (Exception ex) { Console.WriteLine($"Error disposing ViGEmClient: {ex.Message}"); }
            _vigemClient = null;
        }
    }

    private void JoystickEmulator_Closing(object? sender, CancelEventArgs cancelEventArgs)
    {
        StopEmulation();    // Stop the timer and reset axes
        CleanupResources(); // Disconnect controller and dispose client
    }
}
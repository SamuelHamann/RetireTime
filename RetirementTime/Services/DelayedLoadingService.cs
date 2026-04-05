namespace RetirementTime.Services;

/// <summary>
/// Service for managing delayed loading indicators that only show after a specified delay.
/// This prevents flickering loading states on fast page loads.
/// </summary>
public class DelayedLoadingService : IDisposable
{
    private CancellationTokenSource? _delayCts;
    private bool _isLoading;
    private bool _showLoading;
    private Action? _onStateChanged;

    /// <summary>
    /// Gets whether the loading indicator should be displayed.
    /// </summary>
    public bool ShowLoading => _showLoading;

    /// <summary>
    /// Gets whether loading is currently in progress.
    /// </summary>
    public bool IsLoading => _isLoading;

    /// <summary>
    /// Starts a loading operation with a delayed indicator.
    /// The loading indicator will only show if loading takes longer than the specified delay.
    /// </summary>
    /// <param name="onStateChanged">Callback to invoke when the loading state changes (for triggering UI updates)</param>
    /// <param name="delayMs">Delay in milliseconds before showing the loading indicator (default: 500ms)</param>
    public void StartLoading(Action onStateChanged, int delayMs = 500)
    {
        _onStateChanged = onStateChanged;
        _isLoading = true;
        _showLoading = false;

        // Cancel any existing delay timer (but don't dispose yet - the task might still be running)
        try
        {
            _delayCts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Already disposed
        }

        // Start new delay timer
        _delayCts = new CancellationTokenSource();
        _ = ShowLoadingAfterDelay(delayMs, _delayCts.Token);
    }

    /// <summary>
    /// Completes the loading operation and hides the loading indicator.
    /// </summary>
    public void StopLoading()
    {
        _isLoading = false;
        _showLoading = false;

        // Cancel the delay timer if still running
        try
        {
            _delayCts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // CTS may already be disposed
        }

        try
        {
            _onStateChanged?.Invoke();
        }
        catch
        {
            // Prevent callback exceptions from propagating
        }
    }

    private async Task ShowLoadingAfterDelay(int delayMs, CancellationToken cancellationToken)
    {
        try
        {
            // Use ConfigureAwait(false) for efficiency since we don't need the original context
            await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

            if (!cancellationToken.IsCancellationRequested && _isLoading)
            {
                _showLoading = true;

                try
                {
                    _onStateChanged?.Invoke();
                }
                catch
                {
                    // Prevent callback exceptions from propagating
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when loading completes quickly or is cancelled
        }
        catch (ObjectDisposedException)
        {
            // Expected when the service is disposed while delay is running
        }
        catch
        {
            // Catch any other exceptions to prevent UI breaking
        }
    }

    public void Dispose()
    {
        try
        {
            _delayCts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Already disposed
        }

        _delayCts?.Dispose();
        _onStateChanged = null;
    }
}

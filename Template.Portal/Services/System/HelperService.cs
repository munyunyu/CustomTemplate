using Template.Library.Extensions;

namespace Template.Portal.Services.System
{
    /// <summary>
    /// Global static services
    /// </summary>
    public class HelperService
    {
        #region events

        public event Action? OnChange;

        #endregion

        #region properties

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Success Message
        /// </summary>
        public string SuccessMessage { get; set; } = string.Empty;

        /// <summary>
        /// System busy communicating with the server
        /// </summary>
        public bool IsLoading { get; set; } = false;

        /// <summary>
        /// Display default dialog
        /// </summary>
        public bool ShowDialog { get; set; } = false;

        #endregion

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void SetErrorMessage(string input)
        {
            IsLoading = false;
            SuccessMessage = string.Empty;
            ErrorMessage = input;
            NotifyStateChanged();
        }

        public void SetErrorMessage(Exception input)
        {
            IsLoading = false;
            SuccessMessage = string.Empty;
            ErrorMessage = input.GetAllMessages();
            NotifyStateChanged();
        }

        public void SetSuccessMessage(string input)
        {
            IsLoading = false;
            ErrorMessage = string.Empty;
            SuccessMessage = input;
            NotifyStateChanged();
        }

        public void SetIsLoadingState(bool input)
        {
            IsLoading = input;
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            NotifyStateChanged();
        }

        public void ShowDefaultDialog()
        {
            ShowDialog = true;
            NotifyStateChanged();
        }

        public void HideDefaultDialog()
        {
            ShowDialog = false;
            NotifyStateChanged();
        }

        //public Task InvokeStateHasChangedAsync()
        //{
        //    InvokeAsync(() => StateHasChanged());
        //}

    }
}

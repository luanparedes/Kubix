using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Kubix.Services.Interfaces;
using Microsoft.Web.WebView2.Core;

namespace Kubix.ViewModel
{
    public class AIViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "StateChoiceApp";
        public const string STATE_CHAT_GPT = "StateChatGpt";
        public const string STATE_DEEPSEEK = "StateDeepseek";
        public const string STATE_COPILOT = "StateCopilot";
        public const string STATE_GEMINI = "StateGemini";
        public const string STATE_META = "StateMeta";

        public readonly string ChatGptURL = "https://chatgpt.com/";
        public readonly string DeepseekURL = "https://chat.deepseek.com/sign_in.com/";
        public readonly string CopilotURL = "https://copilot.microsoft.com/";
        public readonly string GeminiURL = "https://gemini.google.com/app";
        public readonly string MetaURL = "https://meta.ai";

        #endregion

        #region Fields and Properties

        private readonly ILogger _logger;
        private Control pageControl;
        AIApp ActualAIApp;

        private string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Constructor

        public AIViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("AIViewModel initialized.");
        }

        #endregion

        #region Event Handlers

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "ChatGptBtn":
                    ActualAIApp = AIApp.ChatGpt;
                    CurrentState = STATE_CHAT_GPT;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Switched to ChatGPT view.");
                    break;
                case "DeepseekBtn":
                    ActualAIApp = AIApp.Deepseek;
                    CurrentState = STATE_DEEPSEEK;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Switched to Deepseek view.");
                    break;
                case "CopilotBtn":
                    ActualAIApp = AIApp.Copilot;
                    CurrentState = STATE_COPILOT;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Switched to Copilot view.");
                    break;
                case "GeminiBtn":
                    ActualAIApp = AIApp.Gemini;
                    CurrentState = STATE_GEMINI;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Switched to Gemini view.");
                    break;
                case "MetaBtn":
                    ActualAIApp = AIApp.Meta;
                    CurrentState = STATE_META;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Switched to Meta view.");
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    _logger.InfoLog("Returned to AI app choice view.");
                    break;
            }
        }

        public void PageControl_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void AIAppWeb_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            CoreWebView2Settings settings = sender.CoreWebView2.Settings;

            settings.IsWebMessageEnabled = false;
            settings.AreDefaultScriptDialogsEnabled = false;
            settings.IsScriptEnabled = true;
            settings.AreHostObjectsAllowed = false;

            _logger.InfoLog("WebView2 AIView CoreWebView2 initialized with configuration:");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.IsWebMessageEnabled}");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.AreDefaultScriptDialogsEnabled}");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.IsScriptEnabled}");
            _logger.InfoLog($"WebView2 AIView initialized with custom settings:{settings.AreHostObjectsAllowed}");
        }

        public void AIAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }

        #endregion
    }

    public enum AIApp
    {
        ChatGpt,
        Deepseek,
        Copilot,
        Gemini,
        Meta
    }
}

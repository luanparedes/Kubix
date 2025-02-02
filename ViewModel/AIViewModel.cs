using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

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

        private Control pageControl;

        AIApp ActualAIApp;

        private string CurrentState { get; set; } = STATE_CHOICE_APP;

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "ChatGptBtn":
                    ActualAIApp = AIApp.ChatGpt;
                    CurrentState = STATE_CHAT_GPT;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "DeepseekBtn":
                    ActualAIApp = AIApp.Deepseek;
                    CurrentState = STATE_DEEPSEEK;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "CopilotBtn":
                    ActualAIApp = AIApp.Copilot;
                    CurrentState = STATE_COPILOT;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "GeminiBtn":
                    ActualAIApp = AIApp.Gemini;
                    CurrentState = STATE_GEMINI;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "MetaBtn":
                    ActualAIApp = AIApp.Meta;
                    CurrentState = STATE_META;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
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
            (sender as WebView2).CoreWebView2.Settings.IsWebMessageEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            (sender as WebView2).CoreWebView2.Settings.IsScriptEnabled = true;

            (sender as WebView2).CoreWebView2.Settings.AreHostObjectsAllowed = false;
        }

        public void AIAppWeb_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (!args.Uri.StartsWith("https://"))
                args.Cancel = true;
        }
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

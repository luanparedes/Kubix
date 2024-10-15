
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.UI.WebUI;
using System;

namespace KanBoard.ViewModel
{
    public class AIViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "StateChoiceApp";
        public const string STATE_CHAT_GPT = "StateChatGpt";
        public const string STATE_COPILOT = "StateCopilot";
        public const string STATE_GEMINI = "StateGemini";

        public readonly string ChatGptURL = "https://chatgpt.com/";
        public readonly string CopilotURL = "https://copilot.microsoft.com/";
        public readonly string GeminiURL = "https://gemini.google.com/app";
        public readonly string NullURL = "http://null.com";

        #endregion

        private Control pageControl;
        private WebView2 webView;

        AIApp ActualAIApp;

        private string CurrentState { get; set; } = STATE_CHOICE_APP;

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "ChatGptBtn":
                    webView.Source = new Uri(ChatGptURL);
                    ActualAIApp = AIApp.ChatGpt;
                    CurrentState = STATE_CHAT_GPT;
                    break;
                case "CopilotBtn":
                    webView.Source = new Uri(CopilotURL);
                    ActualAIApp = AIApp.Copilot;
                    CurrentState = STATE_COPILOT;
                    break;
                case "GeminiBtn":
                    webView.Source = new Uri(GeminiURL);
                    ActualAIApp = AIApp.Gemini;
                    CurrentState = STATE_GEMINI;
                    break;
                case "BackButton":
                    webView.Source = new Uri(NullURL);
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

        public void WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            webView = sender as WebView2;
        }

        public void StreamAppWeb_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }
    }

    public enum AIApp
    {
        ChatGpt,
        Copilot,
        Gemini
    }
}

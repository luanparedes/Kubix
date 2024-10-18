
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
    }

    public enum AIApp
    {
        ChatGpt,
        Copilot,
        Gemini
    }
}

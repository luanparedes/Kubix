using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KanBoard.Controls
{
    public class GoogleAuthentication
    {
        private const string ClientId = "XXX";
        private const string ClientSecret = "XXX";
        private const string TokenPath = "XXX"; // Onde salvamos o token localmente.
        private const string Scope = "XXX";

        public GoogleAuthentication()
        {
            Authenticate();
        }

        public async void Authenticate()
        {
            var credential = await CheckLoginStatusAsync();

            if (credential == null)
            {
                // Se não estiver logado, realiza o login.
                credential = await LoginWithGoogleAsync();

                if (credential != null)
                {
                    await SaveTokenAsync(credential.Token);
                    ShowMessage("Login realizado com sucesso!");
                }
                else
                {
                    ShowMessage("Erro ao realizar login.");
                }
            }
            else
            {
                ShowMessage($"Bem-vindo, token ativo: {credential.Token.AccessToken}");
            }
        }

        private async Task<UserCredential?> CheckLoginStatusAsync()
        {
            if (File.Exists(TokenPath))
            {
                try
                {
                    using var stream = new FileStream(TokenPath, FileMode.Open, FileAccess.Read);
                    var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets
                        {
                            ClientId = ClientId,
                            ClientSecret = ClientSecret
                        },
                        new[] { Scope },
                        "user",
                        CancellationToken.None,
                        new FileDataStore(TokenPath, true)
                    );

                    if (credential.Token.IsExpired(Google.Apis.Util.SystemClock.Default))
                    {
                        await credential.RefreshTokenAsync(CancellationToken.None);
                    }

                    return credential;
                }
                catch
                {
                    // Se houver algum problema com o token, retornamos null.
                    return null;
                }
            }

            return null;
        }

        private async Task<UserCredential?> LoginWithGoogleAsync()
        {
            try
            {
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = ClientId,
                        ClientSecret = ClientSecret
                    },
                    new[] { Scope },
                    "user",
                    CancellationToken.None
                );

                return credential;
            }
            catch (Exception ex)
            {
                ShowMessage($"Erro: {ex.Message}");
                return null;
            }
        }

        private async Task SaveTokenAsync(TokenResponse token)
        {
            using var writer = new StreamWriter(TokenPath);
            await writer.WriteAsync(System.Text.Json.JsonSerializer.Serialize(token));
        }

        private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Info",
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
    }
}

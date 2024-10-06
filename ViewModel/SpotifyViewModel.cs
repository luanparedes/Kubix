using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanBoard.ViewModel
{
    public class SpotifyViewModel
    {
        public readonly string SpotifyURL = "https://accounts.spotify.com/authorize?client_id=SEU_CLIENT_ID&response_type=token&redirect_uri=SEU_REDIRECT_URI&scope=user-library-read";
        public readonly string ClientID = "3d1fe76afb55445bb1d1b61ce16f75bf";
        public readonly string SecretID = "ad303a1b037a461297d2dc024cecba16";
        public readonly string RedirectURL = "http://localhost:PORT";

        public readonly string FullPathRedirect;

        public SpotifyViewModel() 
        {
            FullPathRedirect = $"https://accounts.spotify.com/authorize?client_id={ClientID}&response_type=token&redirect_uri={RedirectURL}&scope=user-library-read";
        }
    }
}

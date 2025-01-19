using System.Collections.Generic;

namespace DefaultNamespace
{
    public static class UriHolder
    {
        private const string FREE_TEXTURE_URI_1 =
            "https://drive.usercontent.google.com/u/0/uc?id=1AHBg_6WImXD89P-bIMthMdr9RwKcEduB&export=download";

        private const string FREE_TEXTURE_URI_2 =
            "https://drive.usercontent.google.com/u/0/uc?id=1AKm2K37Umwt9xvWSTirg2mTaKaAbAIuf&export=download";

        private const string FREE_TEXTURE_URI_3 =
            "https://drive.usercontent.google.com/u/0/uc?id=1AMVA-0J76wFJJKWvEiCYLhmMySbW_6BX&export=download";

        private const string FREE_TEXTURE_URI_4 =
            "https://drive.usercontent.google.com/u/0/uc?id=1AMEaF6aEbfyYn_Mh_0mzGm6RSn14KzNQ&export=download";

        public static List<string> UrisList = new List<string>
            { FREE_TEXTURE_URI_1, FREE_TEXTURE_URI_2, FREE_TEXTURE_URI_3, FREE_TEXTURE_URI_4 };
    }
}
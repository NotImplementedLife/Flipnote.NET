namespace FlipnoteDotNet.App.Storage.Codecs
{
    public static class V1
    {
        public static Project EncodeProject(AppState appState)
        {            
            var assets = from a in appState.AssetsService.Assets select a.ToDTO();

            return new ProjectV1
            {
                PaletteConfig = appState.PaletteConfig.ToDTO(),
                Assets = assets.ToArray(),
            };            
        }


        public static AppState DecodeProject(Project project, AppState baseState)
        {
            if (project is not ProjectV1 projv1)
                throw new InvalidOperationException("Format version mismatch");

            var assets = from a in projv1.Assets select a.ToAsset();
            var paletteConfig = projv1.PaletteConfig.ToPaletteConfig();

            var appState = new AppState(paletteConfig, baseState.UndoStack);
            foreach (var a in projv1.Assets)
                appState.AssetsService.Assets.Add(a.ToAsset());

            return appState;
        }

    }
}

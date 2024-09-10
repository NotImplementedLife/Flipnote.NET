using FlipnoteDotNet.Canvas;

namespace FlipnoteDotNet.App.Storage.Codecs
{
    public static class V1
    {
        public static Project EncodeProject(AppState appState)
        {            
            var assets = from a in appState.AssetsService.Assets select a.ToDTO();
            var frames = from f in appState.FramesManager.Frames select f.ToDTO();

            return new ProjectV1
            {
                PaletteConfig = appState.PaletteConfig.ToDTO(),
                Assets = assets.ToArray(),
                Frames = frames.ToArray()
            };
        }


        public static AppState DecodeProject(Project project, AppState baseState, CanvasModel canvasModel)
        {
            // not the best to use canvasModel here

            if (project is not ProjectV1 projv1)
                throw new InvalidOperationException("Format version mismatch");
            
            var paletteConfig = projv1.PaletteConfig.ToPaletteConfig();

            var appState = new AppState(paletteConfig, baseState.UndoStack);            

            foreach (var a in projv1.Assets)
                appState.AssetsService.Assets.Add(a.ToAsset());
            foreach (var f in projv1.Frames)            
                appState.FramesManager.AddFrame(f.ToFrame(canvasModel, paletteConfig));                                                     



            return appState;
        }

    }
}

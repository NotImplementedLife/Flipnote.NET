using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service;

namespace FlipnoteDotNet.App
{
    public class AppState
    {
        public readonly PaletteConfig PaletteConfig;
        public readonly UndoStack UndoStack = new UndoStack();
        public readonly FramesManager FramesManager;
        public readonly AssetsService AssetsService;
        public readonly FlipnoteEditorService FlipnoteEditorService;

        public AppState(PaletteConfig paletteConfig)
        {
            PaletteConfig = paletteConfig;
            FramesManager = new FramesManager(paletteConfig);
            AssetsService = new AssetsService(UndoStack);
            FlipnoteEditorService = new FlipnoteEditorService(FramesManager, UndoStack);
        }


        private static AppState _Instance = new AppState(PaletteConfigs.Flipnote3D);
        public static AppState Instance => _Instance;
    }
}

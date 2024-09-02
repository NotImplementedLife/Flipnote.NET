using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service;

namespace FlipnoteDotNet.App
{
    public class AppState
    {
        public readonly PaletteConfig PaletteConfig;
        public readonly UndoStack UndoStack;
        public readonly FramesManager FramesManager;
        public readonly AssetsService AssetsService;
        public readonly FlipnoteEditorService FlipnoteEditorService;

        private bool fChanged = false;
        public bool Changed => fChanged;

        public AppState(PaletteConfig paletteConfig, UndoStack undoStack = null)
        {
            undoStack?.Clear();
            UndoStack = undoStack ?? new UndoStack();
            PaletteConfig = paletteConfig;
            FramesManager = new FramesManager(paletteConfig);
            AssetsService = new AssetsService(UndoStack);
            FlipnoteEditorService = new FlipnoteEditorService(FramesManager, UndoStack);
        }        

        public static AppState CreateFlipnoteInstance(UndoStack undoStack = null) => new AppState(PaletteConfigs.Flipnote, undoStack);
        public static AppState CreateFlipnote3DInstance(UndoStack undoStack = null) => new AppState(PaletteConfigs.Flipnote3D, undoStack);
        public static AppState CreateVGA16Instance(UndoStack undoStack = null) => new AppState(PaletteConfigs.VGA16, undoStack);


    }
}

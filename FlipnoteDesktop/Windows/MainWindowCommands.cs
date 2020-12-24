using System.Windows.Input;

namespace FlipnoteDesktop.Windows
{
    /// <summary>
    /// Defines the routed commands used in the MainWindow
    /// </summary>
    static class MainWindowCommands
    {
        public static RoutedCommand ToggleGridVisibility = new RoutedCommand();
        public static RoutedCommand SwitchActiveLayer = new RoutedCommand();
        public static RoutedCommand ZoomInCanvas = new RoutedCommand();
        public static RoutedCommand ZoomOutCanvas = new RoutedCommand();
        public static RoutedCommand GetFlipnoteUserId = new RoutedCommand();
        public static RoutedCommand InvertLayerColors = new RoutedCommand();
        public static RoutedCommand OpenPluginManager = new RoutedCommand();        
    }
}

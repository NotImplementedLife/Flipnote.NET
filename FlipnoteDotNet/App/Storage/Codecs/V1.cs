namespace FlipnoteDotNet.App.Storage.Codecs
{
    public static class V1
    {
        public static Project EncodeProject(AppState appState)
        {
            var assets = from a in appState.AssetsService.Assets select a.ToDTO();
            var project = new ProjectV1(assets);
            return project;
        }        




    }
}

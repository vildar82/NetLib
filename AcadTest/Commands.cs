using Autodesk.AutoCAD.Runtime;
using JetBrains.Annotations;

namespace AcadTest
{
    [PublicAPI]
    public static class Commands
    {
        [CommandMethod(nameof(Test1Command))]
        public static void Test1Command()
        {
            var mainVM = new MainViewModel();
            var mainView = new MainView(mainVM);
            mainView.Show();
        }
    }
}
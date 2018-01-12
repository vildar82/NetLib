using AcadLib;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using NetLib.WPF;
using ReactiveUI;

namespace AcadTest
{
    public class MainViewModel : BaseViewModel
    {
        public ReactiveCommand TestProgress { get; set; }

        public MainViewModel()
        {
            TestProgress = CreateCommand(TestProgressExec);
        }

        private void TestProgressExec()
        {
            ShowProgressDialog(c =>
            {
                c.Minimum = 0;
                c.Maximum = 100000;
                var doc = AcadHelper.Doc;
                var db = doc.Database;
                using (doc.LockDocument())
                using (var t = db.TransactionManager.StartTransaction())
                {
                    var cs = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                    for (var i = 0; i < 100000; i++)
                    {
                        c.SetProgress(i);
                        if (i % 10000 == 0) DoEvents();
                        var line = new Line(Point3d.Origin,
                            new Point3d(MathExt.Rnd.NextDouble() * i, MathExt.Rnd.NextDouble() * i,
                                MathExt.Rnd.NextDouble() * i));
                        line.SetDatabaseDefaults();
                        line.Color = Color.FromRgb((byte)MathExt.Rnd.Next(0, 255), (byte)MathExt.Rnd.Next(0, 255),
                            (byte)MathExt.Rnd.Next(0, 255));
                        cs.AppendEntity(line);
                        t.AddNewlyCreatedDBObject(line, true);
                    }
                    t.Commit();
                }
            }, "Test", "thread to db");
        }
    }
}
using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Commons.Reflection;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Actions;
using FlipnoteDotNet.Model.Entities;
using PPMLib.Rendering;
using PPMLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using static PPMLib.Utils.FlipnoteVisualSourceResizer;

namespace ATest
{
    internal class Program
    {     
        static void Create()
        {                                    
            var ctx = new FlipnoteSharedActionContext();
            var mng = new DatabaseManager(ctx);            

            mng.WithDatabase((db) =>
            {
                var proj = db.Create<FlipnoteProject>();
                for (int i = 0; i < 1; i++) 
                    proj.Entity.Tracks.Add(db.Create<Track>());
                proj.Entity.Name = "My project";
                proj.Commit();
                ctx.Project = proj;
            });

            mng.DoAction(new AddSequenceAction(0, 2, 5));
            mng.DoAction(new AddSequenceAction(0, 6, 9));
            mng.DoAction(new ChangeTimestampAction(12));

            Console.WriteLine(ctx.Project.ToString() + "\n");
            mng.UndoLastAction();
            Console.WriteLine(ctx.Project.ToString() + "\n");
            //mng.RedoLastAction();
            //Console.WriteLine(ctx.Project.ToString() + "\n");

            mng.SaveToFile("result.bin");
        }

        static void Load()
        {
            var ctx = new FlipnoteSharedActionContext();
            var mng = DatabaseManager.LoadFromFile("result.bin");
            mng.ActionContext = ctx;

            mng.WithDatabase(db =>
            {
                ctx.Project = db.FindFirst<FlipnoteProject>();
            });

            Console.WriteLine(ctx.Project.ToString() + "\n");
        }


        static void Main(string[] args)
        {
            InitializerService.Register(typeof(PPMLib.Initializer));
            InitializerService.Register(typeof(FlipnoteDotNet.Model.Initializer));
            InitializerService.Register(typeof(BinarySerializer));
            InitializerService.Run();

            Create();
            Load();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }       
    }
}

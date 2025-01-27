using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using DataBox.LANGUAGE.Ribbon;
using DataBox.Properties;
using DataBox.Ribbon.Fun;
using ExcelDna.Integration.CustomUI;

namespace DataBox.Ribbon
{
    [ComVisible(true)]
    public class RibbonController : ExcelRibbon
    {
        private IRibbonUI RibbonUI;

        private static string GetResourceText(string ResourceName)
        {
            Assembly Asm = Assembly.GetExecutingAssembly();
            string[] ResourceNames = Asm.GetManifestResourceNames();
            for (int i = 0; i < ResourceNames.Length; ++i)
            {
                if (
                    string.Compare(
                        ResourceName,
                        ResourceNames[i],
                        StringComparison.OrdinalIgnoreCase
                    ) == 0
                )
                {
                    using (
                        StreamReader ResourceReader = new StreamReader(
                            Asm.GetManifestResourceStream(ResourceNames[i])
                        )
                    )
                    {
                        if (ResourceReader != null)
                        {
                            return ResourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        public override string GetCustomUI(string RibbonID)
        {
            return GetResourceText($"DataBox.Ribbon.RibbonUI.xml");
        }

        public void LoadRibbon(IRibbonUI RibbonUI)
        {
            this.RibbonUI = RibbonUI;
            RibbonUI.Invalidate();
        }

        public string Label(IRibbonControl Control)
        {
            switch (Control.Id)
            {
                case "DataBox":
                    return Basics.DataBox;
                case "DataQuickFind":
                    return Basics.DataQuickFind;
                case "BindingBtn":
                    return DataQuickFindCS.ISNone ? Basics.Binding : Basics.UnBinding;
                case "BatchExtraction":
                    return Basics.BatchExtraction;
                case "LaunchBtn":
                    return Basics.Launch;
                case "ConfigBtn":
                    return Basics.NewConfig;
                case "DataBoxInfo":
                    return Basics.DataBoxInfo;
                case "UpdatesBtn":
                    return Basics.Updates;
                case "BoxInfoBtn":
                    return Basics.BoxInfo;
                default:
                    return null;
            }
        }

        public Bitmap Image(IRibbonControl Control)
        {
            switch (Control.Id)
            {
                case "BindingBtn":
                    return DataQuickFindCS.ISNone ? Resources.绑定数据 : Resources.解绑数据;
                case "LaunchBtn":
                    return Resources.开始提取;
                case "ConfigBtn":
                    return Resources.新建配置;
                case "UpdatesBtn":
                    return Resources.检查更新;
                case "BoxInfoBtn":
                    return Resources.查看信息;
                default:
                    return null;
            }
        }

        public void Click(IRibbonControl Control)
        {
            switch (Control.Id)
            {
                case "BindingBtn":
                    DataQuickFindCS.Create();
                    RibbonUI.InvalidateControl("BindingBtn");
                    return;
                case "LaunchBtn":
                    BatchExtractionCS.Start();
                    return;
                case "ConfigBtn":
                    new NewConfig().Show();
                    return;
                case "UpdatesBtn":
                    return;
                case "BoxInfoBtn":
                    new DataBoxInfo().Show();
                    return;
                default:
                    return;
            }
        }
    }
}

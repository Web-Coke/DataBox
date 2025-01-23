using System;
using System.Reflection;
using System.Windows.Forms;
using DataBox.LANGUAGE.Ribbon;
using DataBox.RustFun.Ribbon;

namespace DataBox.Ribbon
{
    public partial class DataBoxInfo : Form
    {
        private static readonly Version AssemblyVersion = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version;
        private static readonly string _AddInVersion = string.Format(
            "{0}.{1}.{2}.{3}",
            AssemblyVersion?.Major,
            AssemblyVersion?.Minor,
            AssemblyVersion?.Build,
            AssemblyVersion?.Revision
        );
        private static readonly string _CoreVersion = DataBoxInfoRs.CoreVersion().ToString();

        public DataBoxInfo()
        {
            InitializeComponent();
            AliPay.Text = DataBoxInfoText.AliPay;
            WeChat.Text = DataBoxInfoText.WeChat;
            DonateAuthor.Text = DataBoxInfoText.DonateAuthor;
            CoreVersionLabel.Text = DataBoxInfoText.CoreVersion;
            CoreVersion.Text = _CoreVersion;
            AddInVersionLabel.Text = DataBoxInfoText.AddInVersion;
            AddInVersion.Text = _AddInVersion;
            Feedback.Text = DataBoxInfoText.Feedback;
            Feedback.LinkArea = new LinkArea(
                DataBoxInfoText.Feedback.Length - 21,
                DataBoxInfoText.Feedback.Length
            );
            Text = DataBoxInfoText.DataBoxInfo;
        }

        private void Feedback_LinkClicked(object Sender, LinkLabelLinkClickedEventArgs Even)
        {
            Feedback.LinkVisited = true;
            System.Diagnostics.Process.Start("mailto:web-chang@foxmail.com");
        }
    }
}

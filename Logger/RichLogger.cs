using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTemplate.Logger
{
    internal class RichLogger
    {
        public void Log(string message)
        {
            RichTextBox? t = Application.OpenForms["Form1"].Controls["richTextBox1"] as RichTextBox;
            t.Text += message + "\n";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jigglehelper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitJiggleboneOptions();

        }

        private void InitJiggleboneOptions()
        {
            // enable, option, value=
            smK_EditListView1.Items.AddRange(new ListViewItem[] {
                CreateListViewItemFrom("yaw_stiffness", 120, true),
                CreateListViewItemFrom("yaw_damping", 5, true),
                CreateListViewItemFrom("pitch_stiffness", 120, true),
                CreateListViewItemFrom("pitch_damping", 5, true),
                CreateListViewItemFrom("along_stiffness", 100, true),
                CreateListViewItemFrom("along_damping", 0, true),
                CreateListViewItemFrom("allow_length_flex", "", true),
                CreateListViewItemFrom("length", 20, true),
                CreateListViewItemFrom("tip_mass", 20, true),
                CreateListViewItemFrom("angle_constraint", "15", true),
            });
        }

        private ListViewItem CreateListViewItemFrom(string optionName, object defaultValue, bool _checked)
        {
            ListViewItem item = new ListViewItem(optionName);
            item.SubItems.Add(defaultValue.ToString());
            item.Checked = _checked;
            return item;
        }

        private void addAutoincrementBonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var questionform = new AutoIncrementDialogue();
            if (questionform.ShowDialog() == DialogResult.OK)
            {
                for (int i = (questionform.checkBox1.Checked ? 0 : 1); i < questionform.numericUpDown1.Value +1 ; i++)
                {
                    textBox1.AppendText(questionform.textBox1.Text.Replace("#", i.ToString()) + "\r\n");
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.SelectedText);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var text = Clipboard.GetText();
            textBox1.Paste(text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            foreach(var bone in textBox1.Text.Split('\r', '\n'))
            {
                if (string.IsNullOrEmpty(bone)) continue;
                builder.AppendLine("$jigglebone \""+bone+"\"\r\n{");
                builder.AppendLine("\tis_flexible");
                builder.AppendLine("\t{");
                foreach (ListViewItem item in smK_EditListView1.CheckedItems )
                {
                    builder.AppendLine(string.Format( "\t\t{0} {1}", item.SubItems[0].Text , item.SubItems[1].Text ));
                }
                builder.AppendLine("\t}");
                builder.AppendLine("}");
            }
            Clipboard.SetText(builder.ToString());
        }
    }
}

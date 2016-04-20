using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoundEditor
{
    public partial class RoundSettings : Form
    {
        RoundEditor roundEditor;
        public RoundSettings(RoundEditor editor)
        {
            InitializeComponent();
            roundEditor = editor;
            if (roundEditor.SpawnType == 0)
            {
                individualButton.Checked = true;
                if(roundEditor.ScreenEnemies != -1)
                {
                    numberButton.Checked = true;
                    numberBox.Text = roundEditor.ScreenEnemies.ToString();  
                }
                else
                {
                    numberButton.Checked = false;
                }
            }
            else
            {
                groupButton.Checked = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            roundEditor.Enabled = true;
            if (groupButton.Checked == true) roundEditor.SpawnType = 1;
            else roundEditor.SpawnType = 0;
            if (individualButton.Checked == true && numberButton.Checked == true)
            {
                int num = 0;
                int.TryParse(numberBox.Text, out num);
                if (num <= 0)
                {
                    // Give an error, a number was not entered
                    var dialog = MessageBox.Show("Invalid value for number of enemies, must an int greater than 0.","", MessageBoxButtons.OK);
                    e.Cancel = true;
                }
                else
                {
                    roundEditor.ScreenEnemies = num;
                }
            }
            else roundEditor.ScreenEnemies = -1;
            base.OnClosing(e);
        }

        private void groupButton_CheckedChanged(object sender, EventArgs e)
        {
            if (groupButton.Checked == true)
            {
                enemiesBox.Enabled = false;
            }
            else
            {
                enemiesBox.Enabled = true;
            }
        }

        private void numberButton_CheckedChanged(object sender, EventArgs e)
        {
            if (numberButton.Checked == true)
            {
                numberBox.Enabled = true;
            }
            else
            {
                numberBox.Enabled = false;
            }
        }
    }
}

// RoundEditor
// An external tool that allows us to generate a binary file of a round for our horde mode
// The binary file will contain the positions of all the enemies for the round as well as what type of enemies they are
// You can create new .dat files for a round, or load in an existing round to edit it through this tool
// Kiernan Brown

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RoundEditor
{
    public partial class RoundEditor : Form
    {
        // Create a list of Labels, used as a list for holding all the enemies in a round
        private List<Label> enemies = new List<Label>();
        private List<Label> group2Enemies = new List<Label>();
        private List<Label> group3Enemies = new List<Label>();
        private RoundSettings settingsForm;

        // Round Settings
        private int spawnType = 0; // 0 if individual, 1 if groups
        private int screenEnemies = -1;
        private int group = 1;

        public int SpawnType
        {
            get { return spawnType; }
            set { spawnType = value; }
        }

        public int ScreenEnemies
        {
            get { return screenEnemies; }
            set { screenEnemies = value; }
        }

        public RoundEditor()
        {
            InitializeComponent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog object and show the dialog to allow the user to pick where the file is saved
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Dat File |*.dat"; // Only allow .dat files to be saved, as these are the files we are using for rounds
            saveFileDialog1.Title = "Save Round";
            saveFileDialog1.ShowDialog();

            // If the file name is not blank, we will make a file
            if (saveFileDialog1.FileName != "")
            {
                // Create the binary file
                // Create a BinaryWriter that uses the FileName the user specified with the dialog
                BinaryWriter saver = new BinaryWriter(new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write));

                // Foreach loop to go through the array of enemies and save their information
                saver.Write(screenEnemies);

                foreach (Label enemy in enemies)
                {
                    saver.Write(enemy.Text);
                    saver.Write(enemy.Location.X);
                    saver.Write(enemy.Location.Y);
                }
                // Close the file as we are done with it
                saver.Close();
            }
        }

        private void clearRoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Foreach loop that goes through the enemies list and removes all enemies from the screen
            foreach (Label enemy in enemies) this.Controls.Remove(enemy);
            enemies.Clear(); // Clear the enemies list
            enemies.Capacity = 0; // Reset the enemies list capacity
        }

        private void loadRoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If the enemies list is not empty, the user will be warned that their current round will be cleared
            if (enemies.Capacity > 0 && enemies.Count > 0)
            {
                DialogResult confirmationDialog = MessageBox.Show("Loading a new round will clear the current round. Continue with loading new a round?", "Warning!", MessageBoxButtons.YesNo);

                // If the user selects no, this event will stop
                if (confirmationDialog == DialogResult.No)
                {
                    return;
                }
            }

            // The list was empty, or the user chose yes when warned. Load in a roundd
            // Create an OpenFileDialog object and show the dialog, so the user can select a round to load
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "dat files (*.dat)|*.dat"; // Filter for .dat files, as rounds are .dat files
            openFileDialog1.ShowDialog();

            // Make sure user selected a file. No exceptions when users choose cancel
            if (openFileDialog1.FileName == "") return;

            // Create a BinaryReader that will read the round file
            BinaryReader loader = new BinaryReader(File.OpenRead(openFileDialog1.FileName));

            // Try block to handle EndOfStreamExceptions that are generated when the file is done reading
            try
            {

                // Clear the current round
                // Foreach loop that goes through the enemies list and removes all enemies from the screen
                foreach (Label enemy in enemies) this.Controls.Remove(enemy);
                enemies.Clear(); // Clear the enemies list
                enemies.Capacity = 0; // Reset the enemies list capacity

                screenEnemies = loader.ReadInt32();

                while (true)
                {
                    // Read in an enemies information
                    string text = loader.ReadString();
                    int x = loader.ReadInt32();
                    int y = loader.ReadInt32();

                    // Create the label of the enemy
                    Label enemyLabel = new Label();
                    enemyLabel.Font = new Font("Arial", 16, FontStyle.Bold);
                    enemyLabel.Text = text; // Label text is set to the text from the file
                    enemyLabel.Location = new Point(x, y); // Label position is set to the x and y values from the file
                    enemyLabel.AutoSize = true; // Label is autosized

                    // Add events for allowing the user to move the newly created label around with the mouse
                    enemyLabel.MouseDown += new MouseEventHandler(label_MouseDown);
                    enemyLabel.MouseMove += new MouseEventHandler(label_MouseMove);
                    enemyLabel.MouseClick += new MouseEventHandler(label_MouseClick);

                    // Add the enemyLabel to the enemies array and add it to the screen
                    enemies.Add(enemyLabel);
                    this.Controls.Add(enemyLabel);
                }
            }
            // Silently catch EndOfStreamExceptions
            catch (EndOfStreamException)
            { }

            // Close the file
            loader.Close();
        }

        private void enemyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new label
            Label enemyLabel = new Label();
            enemyLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            enemyLabel.Location = new Point(100, 100); // Arbitrary point that the enemy is created
            enemyLabel.AutoSize = true; // Autosizes the label

            // Switch statement
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            switch(item.Text)
            {
                case "Enemy 1": enemyLabel.Text = "1";
                    break;
                case "Enemy 2":
                    enemyLabel.Text = "2";
                    break;
                case "Enemy 3":
                    enemyLabel.Text = "3";
                    break;
                case "Enemy 4":
                    enemyLabel.Text = "4";
                    break;
                case "Enemy 5":
                    enemyLabel.Text = "5";
                    break;
            }

            // Add events for allowing the user to move and interact with the newly created label using the mouse
            enemyLabel.MouseDown += new MouseEventHandler(label_MouseDown);
            enemyLabel.MouseMove += new MouseEventHandler(label_MouseMove);
            enemyLabel.MouseClick += new MouseEventHandler(label_MouseClick);

            // Add the enemyLabel to the enemies array and add it to the screen
            enemies.Add(enemyLabel);
            this.Controls.Add(enemyLabel);
        }

        // Events for letting users move labels around with the mouse
        // Create a point, mLocation, which will hold the point of the mouse when it is down
        Point mLocation;

        // MouseDown event
        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            // Set mLocation equal to the location of the mouse when it is pressed down
            mLocation = e.Location;
        }

        // MouseMove event
        private void label_MouseMove(object sender, MouseEventArgs e)
        {
            // If the left mouse button is clicked, it will run this
            if(e.Button == MouseButtons.Left)
            {
                // Cast the sender object to a new label
                // This allows us to use this event with the multiple labels we generate with code
                Label movingLabel = (Label)sender;

                // Move the label based on the mouse
                movingLabel.Left = e.Location.X + movingLabel.Left - mLocation.X; // Moves the label horizontally
                movingLabel.Top = e.Location.Y + movingLabel.Top - mLocation.Y; // Moves the label vertically
            }
        }

        // MouseClick event
        private void label_MouseClick(object sender, MouseEventArgs e)
        {
            // If the right mouse button is clicked, it will run this and delete the label
            if(e.Button == MouseButtons.Right)
            {
                // Cast the sender object to a new label
                // This allows us to use this event with the multiples labels we generate with code
                Label deleteLabel = (Label)sender;

                int count = 0; // Int to count how many times the foreach loop goes

                // Foreach loop that goes through the list of enemy labels
                foreach(Label enemy in enemies)
                {
                    // If the enemy label being right clicked is the enemy label from the list, break out of the loop
                    if(enemy == deleteLabel)
                    {
                        break;
                    }
                    count++; // Increment the count if the enemy label from the list is not the enemy label being right clicked
                }

                // Delete the enemy
                enemies.RemoveAt(count); // Remove the enemy with index count from the list of enemy labels
                this.Controls.Remove(deleteLabel); // Remove the enemy from the screen
            }
        }

        private void roundSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm = new RoundSettings(this);
            settingsForm.Show();
            this.Enabled = false;
        }

        private void group1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            group = 1;
        }

        private void group2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            group = 2;
        }

        private void group3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            group = 3;
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProfileManager.Properties;

namespace ProfileManager
{
    public partial class FormProfiles : Form
    {
        public FormProfiles()
        {
            InitializeComponent();
        }

        private void ExecuteProfile(string workingDirectory, string executableFile, string profileDir)
        {
            var wrap = new Func<string, string>(r => string.Format("\"{0}\"", r));
            Process.Start("RimWorldHookLoader.exe",
                string.Format("{0} {1} {2}", wrap(profileDir), wrap(executableFile), wrap(workingDirectory)));
        }

        private void FormProfiles_Load(object sender, EventArgs e)
        {
            try
            {
                //    var profileNameCol = new DataGridViewTextBoxColumn();
                //    profileNameCol.CellType = typeof (string);
                //    profileNameCol.DataPropertyName = "ProfileName";

                var profileNameCol = new DataGridViewTextBoxColumn
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    Name = "ProfileName",
                    DataPropertyName = "ProfileName",
                    HeaderText = "Profile Name"
                };

                var profileDirCol = new DataGridViewTextBoxColumn
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    Name = "ProfileDir",
                    DataPropertyName = "ProfileDir",
                    HeaderText = "Profile Directory (optional)"
                };

                var rimworldDirCol = new DataGridViewFileExplorerColumn(ExplorerType.Directory)
                {
                    CellTemplate = new DataGridViewFileExplorerCell(),
                    Name = "RimWorldDir",
                    DataPropertyName = "RimWorldDir",
                    HeaderText = "RimWorld Directory"
                };

                var rimworldExeCol = new DataGridViewFileExplorerColumn(ExplorerType.File)
                {
                    CellTemplate = new DataGridViewFileExplorerCell(),
                    Name = "RimWorldExe",
                    DataPropertyName = "RimWorldExe",
                    HeaderText = "RimWorld Executable"
                };

                rimworldExeCol.GetStartDirectory += () => rimworldDirCol.DialogResult;
                dataGridView1.Columns.Add(profileNameCol);
                dataGridView1.Columns.Add(profileDirCol);
                dataGridView1.Columns.Add(rimworldDirCol);
                dataGridView1.Columns.Add(rimworldExeCol);
                dataGridView1.MultiSelect = false;
                //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = ProfileManager.Profiles;
                dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize Grid: " + ex.Message);
                throw;
            }
        }

        private bool FindSelectedRow(out int rowIndex)
        {
            rowIndex = -1;
            var cell = dataGridView1.SelectedCells.OfType<DataGridViewCell>().FirstOrDefault();
            if (cell == null) return false;
            rowIndex = cell.RowIndex;
            return true;
        }

        private void tsbRun_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (!FindSelectedRow(out rowIndex))
            {
                MessageBox.Show("No cell selected.", Resources.Title);
                return;
            }
            var row = dataGridView1.Rows[rowIndex];
            var profileName = row.Cells["ProfileName"].Value as string;
            var rimworldDir = row.Cells["RimWorldDir"].Value as string;
            if (string.IsNullOrWhiteSpace(rimworldDir))
            {
                MessageBox.Show("RimWorld Directory required.", Resources.Title);
                return;
            }
            var rimworldExe = row.Cells["RimWorldExe"].Value as string;
            if (string.IsNullOrWhiteSpace(rimworldExe))
            {
                MessageBox.Show("RimWorld Executable required.", Resources.Title);
                return;
            }
            var profileDir = row.Cells["ProfileDir"].Value as string;
            if (string.IsNullOrEmpty(profileDir))
            {
                profileDir = ProfileManager.CreateProfileDirFromName(profileName);
                row.Cells["ProfileDir"].Value = profileDir;
            }
            // Validate that we'll be able to create the profileDir when it will be needed.
            if (ProfileManager.CreateProfileDirFromName(profileDir) != profileDir)
            {
                MessageBox.Show("Profile Directory contains invalid characters, please fix.", Resources.Title);
                return;
            }
            var result = MessageBox.Show(string.Format("Execute profile '{0}'?", profileName),
                Resources.Title, MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            ExecuteProfile(rimworldDir, rimworldExe, profileDir);
        }

        private static void OpenDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                var result = MessageBox.Show("Directory does not exist, create it? " + Environment.NewLine + dir, Resources.Title,
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.No) return;
                DirectoryHelper.CreateRecursive(dir);
            }
            Process.Start(dir);
        }

        private void tsbOpenRimWorld_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (!FindSelectedRow(out rowIndex))
            {
                MessageBox.Show("No cell selected.", Resources.Title);
                return;
            }
            var row = dataGridView1.Rows[rowIndex];
            var rimworldDir = row.Cells["RimWorldDir"].Value as string;
            if (string.IsNullOrEmpty(rimworldDir))
            {
                MessageBox.Show("RimWorld Directory is not set.", Resources.Title);
                return;
            }
            OpenDirectory(rimworldDir);
        }

        private void tsbOpenProfile_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (!FindSelectedRow(out rowIndex))
            {
                MessageBox.Show("No cell selected.", Resources.Title);
                return;
            }
            var row = dataGridView1.Rows[rowIndex];
            var profileDirName = row.Cells["ProfileDir"].Value as string;
            if (string.IsNullOrEmpty(profileDirName))
            {
                MessageBox.Show("RimWorld Directory is not set.", Resources.Title);
                return;
            }
            var profilePath = Path.Combine(KnownFolder.ProfilesDirectory, profileDirName);
            OpenDirectory(profilePath);
        }
    }
}
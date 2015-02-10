using System;
using System.Diagnostics;
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

        private void tsbRun_Click(object sender, EventArgs e)
        {
            var rowIndex = dataGridView1.SelectedCells.OfType<DataGridViewCell>().First().RowIndex;
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
                MessageBox.Show("Profile Direcotry contains invalid characters, please fix.", Resources.Title);
                return;
            }
            var result = MessageBox.Show(string.Format("Execute profile '{0}'?", profileName),
                Resources.Title, MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            ExecuteProfile(rimworldDir, rimworldExe, profileDir);
        }
    }
}
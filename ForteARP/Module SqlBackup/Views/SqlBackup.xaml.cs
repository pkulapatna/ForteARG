using ForteArg.Services;
using ForteARP.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ForteARP.Modules
{
    /// <summary>
    /// Interaction logic for SqlBackup.xaml
    /// </summary>
    public partial class SqlBackup : Window
    {
        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        private readonly string strSqlBackupPath = @"C:\Users\Public\Documents\ForteSqlBackup";
        private string sDataSource;
        private string strConnectmaster = string.Empty;
        private readonly string sqlQuery = "SELECT * FROM sys.databases d WHERE d.database_id>4";
    
        private static readonly bool Machine64Bit = System.Environment.Is64BitOperatingSystem;
        readonly int intTimeout = 60;
        readonly string m_strUserName = "forte";
        readonly string m_strPassWrd = "etrof";

        

        public SqlBackup()
        {
            InitializeComponent();

            sDataSource = _sqlhandler.Host + @"\SQLEXPRESS";

            Initsqldb();

            txtBackupFileLocation.Text = strSqlBackupPath;
            txtDataSource.Text = sDataSource;
           
            if (Machine64Bit)
                intTimeout = 60;
            else
                intTimeout = 120;

            progressBar1.Visibility = Visibility.Hidden;
            progressBar1.Value = 0;
        }

        private void Initsqldb()
        {
            strConnectmaster = "workstation id=" + _sqlhandler.Host.ToString()
                                                 + ";packet size=4096;integrated security=SSPI;data source='" + sDataSource 
                                                 + "';persist security info=False;initial catalog= master; user id=admin;password=admin;";

            strConnectmaster = "Data Source ='" + sDataSource
                                       + "'; Database = " + cmbDatabases.Text
                                       + "; User id= '" + m_strUserName
                                       + "'; Password = '" + m_strPassWrd
                                       + "'; connection timeout=30;Persist Security Info=True;initial catalog= master";

            GetSqlDatabase();
            cmbDatabases.SelectedIndex = 0;

            btnBrowse.IsEnabled = true;
            btnBackup.IsEnabled = true;
            cmbDatabases.IsEnabled = true;
        }

        private void GetSqlDatabase()
        {
            try
            {
                using (var sqlConnection = new SqlConnection(strConnectmaster))
                {
                    if (sqlConnection != null) sqlConnection.Open();
                    using (var command = new SqlCommand(sqlQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cmbDatabases.Items.Add(reader[0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtInfo.Text = "ERROR in GetSqlDatabase" + ex.Message;
                //MessageBox.Show("ERROR in GetSqlDatabase" + ex.Message);
            }
        }

        private void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = string.Empty;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.Value = 50;
            progressBar1.UpdateLayout();

            txtInfo.Text = "Backup Database -> in progress";
            txtInfo.UpdateLayout();
        
            //back string query with checksum
            string sql = "BACKUP DATABASE " + cmbDatabases.Text + " TO DISK = '" + txtBackupFileLocation.Text + "\\"
                                            + txtbackupFileName.Text 
                                            + "-" + _sqlhandler.Host.ToString() + "-" 
                                            + DateTime.Now.ToString("MM_dd_yyyy_HH_mm") + ".bak' WITH INIT, CHECKSUM;";

            //For Remote machine from local computer to remote sqlserver
            string strRemoteMasterCon = "Data Source ='" + sDataSource
                                        + "'; Database = " + cmbDatabases.Text 
                                        + "; User id= '" + m_strUserName 
                                        + "'; Password = '"+ m_strPassWrd 
                                        + "'; connection timeout=30;Persist Security Info=True;initial catalog= master";

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                if (cmbDatabases.Text.CompareTo("") == 0)
                {
                    System.Windows.MessageBox.Show("Please Select a database");
                    return;
                }

                FindCreateDir(txtBackupFileLocation.Text);
                txtbackupFileName.Text = cmbDatabases.SelectedItem.ToString();    

                using (var sqlConnection = new SqlConnection(strRemoteMasterCon))
                {
                    sqlConnection.Open();
                    using (var command = new SqlCommand(sql, sqlConnection))
                    {
                        command.CommandTimeout = intTimeout;
                        command.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
                errorMsg = "Backup Database -> complete";
                progressBar1.Value = 100;
                progressBar1.UpdateLayout();
            }
            catch (Exception ex)
            {
                errorMsg = "ERROR in BtnBackup_Click" + ex.Message;
            }
            finally
            {
                txtInfo.Text = errorMsg;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                btnBrowse.IsEnabled = false;
                btnBackup.IsEnabled = false;
                cmbDatabases.IsEnabled = false;
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                txtBackupFileLocation.Text = strSqlBackupPath;
                var dlg = new System.Windows.Forms.FolderBrowserDialog();

                FindCreateDir(strSqlBackupPath);
                dlg.SelectedPath = strSqlBackupPath;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtBackupFileLocation.Text = dlg.SelectedPath;
                    txtbackupFileName.Text = cmbDatabases.Text;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void FindCreateDir(string dirname)
        {
            //string path = @"c:\MyDir";   
            try
            {
                if (!Directory.Exists(dirname))
                {
                    DirectoryInfo di = Directory.CreateDirectory(dirname);
                    di.Attributes |= FileAttributes.ReadOnly;
                    di.Refresh();
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
                txtInfo.Text = ex.Message;
                txtInfo.Text = "ERROR in GetSqlDatabase" + ex.Message;
            }
        }

        private void CmbDatabases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtbackupFileName.Text = cmbDatabases.SelectedValue.ToString();
        }
    }
}

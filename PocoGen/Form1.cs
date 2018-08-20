
// Type: POCOGen.Form1
// Assembly: POCOGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E14F844-B8BF-4B4C-8F4A-E6B2FD050651
// Assembly location: C:\Users\geir\Desktop\POCO\POCOGen.exe

using CommonUtils.Config;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace POCOGen
{
  public class Form1 : Form
  {
    private ConfigImpl _config = (ConfigImpl) null;
    private DBHandler _db = (DBHandler) null;
    private IContainer components = (IContainer) null;
    private string configFile;
    private GroupBox groupBox1;
    private Button btnConnect;
    private TextBox tbConnStr;
    private ComboBox cbConnStr;
    private Label label2;
    private ComboBox cbDBDriver;
    private Label label1;
    private GroupBox groupBox2;
    private CheckBox chkGenVO;
    private Button btnDstDir;
    private TextBox tbDstDir;
    private Label label3;
    private GroupBox groupBox3;
    private ListBox lbTables;
    private Button btnGenerate;
    private Button button2;
    private FolderBrowserDialog folderBrowserDialog1;
    private TextBox tbPrefix;
    private Label label4;
    private TextBox txtDAONS;
    private Label label5;
    private CheckBox chkASP;
    private CheckBox chkXSD;
    private CheckBox chkDAO;
    private CheckBox chkCommon;
    private TextBox txtVONS;
    private Label label6;
    private TextBox txtCommonNS;
    private Label label7;

    public Form1()
    {
      this.InitializeComponent();
    }

    private void cbDBDriver_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      this._config.SetXpathRoot("/pocgen/connection-strings/");
      KVPair[] configKvPair = this._config.GetConfigKVPair(comboBox.Text, true);
      this.cbConnStr.Items.Clear();
      foreach (object obj in configKvPair)
      {
        this.cbConnStr.Items.Add(obj);
        this.cbConnStr.DisplayMember = "Key";
        this.cbConnStr.ValueMember = "Value";
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      try
      {
        this.configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pocgen.config");
        this._config = ConfigFactory.GetConfigFromFile(this.configFile);
        this._config.SetXpathRoot("/pocgen/db-settings/");
        this.SetDDLValue(ref this.cbDBDriver, this._config.GetConfigString("driver"));
        this.SetDDLValue(ref this.cbConnStr, this._config.GetConfigString("connectionString"));
        this._config.SetXpathRoot("/pocgen/generator/");
        this.SetTextBoxValue(ref this.tbDstDir, this._config.GetConfigString("dst-dir"));
        this.SetTextBoxValue(ref this.tbPrefix, this._config.GetConfigString("skip-prefix"));
        this.SetTextBoxValue(ref this.txtDAONS, this._config.GetConfigString("dao-namespace"));
        this.SetTextBoxValue(ref this.txtVONS, this._config.GetConfigString("vo-namespace"));
        this.SetTextBoxValue(ref this.txtCommonNS, this._config.GetConfigString("common-namespace"));
        this.chkGenVO.Checked = this._config.GetConfigBool("gen-value-object");
        this.chkDAO.Checked = this._config.GetConfigBool("gen-dao-class");
        this.chkXSD.Checked = this._config.GetConfigBool("gen-xsd");
        this.chkASP.Checked = this._config.GetConfigBool("gen-asp");
        this.chkCommon.Checked = this._config.GetConfigBool("gen-db-helper");
      }
      catch (Exception)
      {
        int num = (int) MessageBox.Show("Unable to locate config file 'pocgen.config'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
    }

    private void SetTextBoxValue(ref TextBox box, string value)
    {
      if (value == null || !(value != ""))
        return;
      box.Text = value;
    }

    private void SetDDLValue(ref ComboBox box, string value)
    {
      if (box == null || box.Items.Count == 0 || value == null)
        return;
      for (int index = 0; index < box.Items.Count; ++index)
      {
        if (value == (string) box.Items[index])
        {
          box.SelectedIndex = index;
          break;
        }
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void cbConnStr_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.tbConnStr.Text = ((KVPair) this.cbConnStr.SelectedItem).Value;
    }

    private void btnConnect_Click(object sender, EventArgs e)
    {
      if (this.tbConnStr.Text == "")
      {
        int num = (int) MessageBox.Show("You must specify a connection string!", "DB ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this._db = new DBHandler(this.cbDBDriver.Text, this.tbConnStr.Text);
        string[] tableList = this._db.GetTableList();
        this.lbTables.Items.Clear();
        this.lbTables.Items.AddRange((object[]) tableList);
      }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      this._config.SetXpathRoot("/pocgen/");
      this._config.SetConfigString("db-settings/driver", this.cbDBDriver.Text);
      this._config.SetConfigString("db-settings/connectionString", this.tbConnStr.Text);
      this._config.SetConfigString("generator/dst-dir", this.tbDstDir.Text);
      this._config.SetConfigString("generator/skip-prefix", this.tbPrefix.Text);
      this._config.SetConfigString("generator/dao-namespace", this.txtDAONS.Text);
      this._config.SetConfigString("generator/vo-namespace", this.txtVONS.Text);
      this._config.SetConfigString("generator/common-namespace", this.txtCommonNS.Text);
      this._config.SetConfigString("generator/gen-value-object", this.chkGenVO.Checked ? "true" : "false");
      this._config.SetConfigString("generator/gen-dao-class", this.chkDAO.Checked ? "true" : "false");
      this._config.SetConfigString("generator/gen-xsd", this.chkXSD.Checked ? "true" : "false");
      this._config.SetConfigString("generator/gen-asp", this.chkASP.Checked ? "true" : "false");
      this._config.SetConfigString("generator/gen-db-helper", this.chkCommon.Checked ? "true" : "false");
      this._config.Save(this.configFile);
      if (this._db == null)
        return;
      this._db.Close();
    }

    private void btnDstDir_Click(object sender, EventArgs e)
    {
      this.folderBrowserDialog1.Description = "Select destination directory";
      this.folderBrowserDialog1.ShowNewFolderButton = true;
      this.folderBrowserDialog1.SelectedPath = this.tbDstDir.Text;
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.tbDstDir.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.btnGenerate.Enabled = false;
      foreach (int selectedIndex in this.lbTables.SelectedIndices)
        this._db.CreateClass(this.txtDAONS.Text, this.txtVONS.Text, this.txtCommonNS.Text, (string) this.lbTables.Items[selectedIndex], this.tbDstDir.Text, this.tbPrefix.Text, this.chkGenVO.Checked, this.chkDAO.Checked, this.chkXSD.Checked, this.chkASP.Checked, this.chkCommon.Checked);
      this.btnGenerate.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.btnConnect = new Button();
      this.tbConnStr = new TextBox();
      this.cbConnStr = new ComboBox();
      this.label2 = new Label();
      this.cbDBDriver = new ComboBox();
      this.label1 = new Label();
      this.groupBox2 = new GroupBox();
      this.chkCommon = new CheckBox();
      this.chkASP = new CheckBox();
      this.chkXSD = new CheckBox();
      this.chkDAO = new CheckBox();
      this.txtDAONS = new TextBox();
      this.label5 = new Label();
      this.tbPrefix = new TextBox();
      this.label4 = new Label();
      this.chkGenVO = new CheckBox();
      this.btnDstDir = new Button();
      this.tbDstDir = new TextBox();
      this.label3 = new Label();
      this.groupBox3 = new GroupBox();
      this.lbTables = new ListBox();
      this.btnGenerate = new Button();
      this.button2 = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.txtVONS = new TextBox();
      this.label6 = new Label();
      this.txtCommonNS = new TextBox();
      this.label7 = new Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.btnConnect);
      this.groupBox1.Controls.Add((Control) this.tbConnStr);
      this.groupBox1.Controls.Add((Control) this.cbConnStr);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.cbDBDriver);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(558, 117);
      this.groupBox1.TabIndex = 6;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "DB Settings";
      this.btnConnect.Location = new Point(10, 80);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new Size(75, 23);
      this.btnConnect.TabIndex = 4;
      this.btnConnect.Text = "&Connect";
      this.btnConnect.UseVisualStyleBackColor = true;
      this.btnConnect.Click += new EventHandler(this.btnConnect_Click);
      this.tbConnStr.Location = new Point(108, 80);
      this.tbConnStr.Name = "tbConnStr";
      this.tbConnStr.Size = new Size(440, 20);
      this.tbConnStr.TabIndex = 3;
      this.cbConnStr.FormattingEnabled = true;
      this.cbConnStr.Location = new Point(108, 53);
      this.cbConnStr.Name = "cbConnStr";
      this.cbConnStr.Size = new Size(121, 21);
      this.cbConnStr.TabIndex = 2;
      this.cbConnStr.Text = "<Select>";
      this.cbConnStr.SelectedIndexChanged += new EventHandler(this.cbConnStr_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(91, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Connection String";
      this.cbDBDriver.FormattingEnabled = true;
      this.cbDBDriver.Items.AddRange(new object[3]
      {
        (object) "MySQL",
        (object) "PgSQL",
        (object) "MSSQL"
      });
      this.cbDBDriver.Location = new Point(108, 23);
      this.cbDBDriver.Name = "cbDBDriver";
      this.cbDBDriver.Size = new Size(121, 21);
      this.cbDBDriver.TabIndex = 1;
      this.cbDBDriver.Text = "<Select>";
      this.cbDBDriver.SelectedIndexChanged += new EventHandler(this.cbDBDriver_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 26);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Database driver";
      this.groupBox2.Controls.Add((Control) this.txtCommonNS);
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.txtVONS);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Controls.Add((Control) this.chkCommon);
      this.groupBox2.Controls.Add((Control) this.chkASP);
      this.groupBox2.Controls.Add((Control) this.chkXSD);
      this.groupBox2.Controls.Add((Control) this.chkDAO);
      this.groupBox2.Controls.Add((Control) this.txtDAONS);
      this.groupBox2.Controls.Add((Control) this.label5);
      this.groupBox2.Controls.Add((Control) this.tbPrefix);
      this.groupBox2.Controls.Add((Control) this.label4);
      this.groupBox2.Controls.Add((Control) this.chkGenVO);
      this.groupBox2.Controls.Add((Control) this.btnDstDir);
      this.groupBox2.Controls.Add((Control) this.tbDstDir);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Location = new Point(13, 136);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(557, 185);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Generator Settings";
      this.chkCommon.AutoSize = true;
      this.chkCommon.Location = new Point(392, 158);
      this.chkCommon.Name = "chkCommon";
      this.chkCommon.Size = new Size(75, 17);
      this.chkCommon.TabIndex = 15;
      this.chkCommon.Text = "DB Helper";
      this.chkCommon.UseVisualStyleBackColor = true;
      this.chkASP.AutoSize = true;
      this.chkASP.Location = new Point(310, 158);
      this.chkASP.Name = "chkASP";
      this.chkASP.Size = new Size(75, 17);
      this.chkASP.TabIndex = 14;
      this.chkASP.Text = "ASP Page";
      this.chkASP.UseVisualStyleBackColor = true;
      this.chkXSD.AutoSize = true;
      this.chkXSD.Location = new Point((int) byte.MaxValue, 158);
      this.chkXSD.Name = "chkXSD";
      this.chkXSD.Size = new Size(48, 17);
      this.chkXSD.TabIndex = 13;
      this.chkXSD.Text = "XSD";
      this.chkXSD.UseVisualStyleBackColor = true;
      this.chkDAO.AutoSize = true;
      this.chkDAO.Location = new Point(171, 158);
      this.chkDAO.Name = "chkDAO";
      this.chkDAO.Size = new Size(77, 17);
      this.chkDAO.TabIndex = 12;
      this.chkDAO.Text = "DAO Class";
      this.chkDAO.UseVisualStyleBackColor = true;
      this.txtDAONS.Location = new Point(123, 71);
      this.txtDAONS.Name = "txtDAONS";
      this.txtDAONS.Size = new Size(381, 20);
      this.txtDAONS.TabIndex = 11;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 75);
      this.label5.Name = "label5";
      this.label5.Size = new Size(90, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "DAO Namespace";
      this.tbPrefix.Location = new Point(123, 44);
      this.tbPrefix.Name = "tbPrefix";
      this.tbPrefix.Size = new Size(381, 20);
      this.tbPrefix.TabIndex = 9;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 47);
      this.label4.Name = "label4";
      this.label4.Size = new Size(105, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Skip prefix from table";
      this.chkGenVO.AutoSize = true;
      this.chkGenVO.Location = new Point(82, 158);
      this.chkGenVO.Name = "chkGenVO";
      this.chkGenVO.Size = new Size(84, 17);
      this.chkGenVO.TabIndex = 7;
      this.chkGenVO.Text = "ValueObject";
      this.chkGenVO.UseVisualStyleBackColor = true;
      this.btnDstDir.Location = new Point(508, 17);
      this.btnDstDir.Name = "btnDstDir";
      this.btnDstDir.Size = new Size(39, 23);
      this.btnDstDir.TabIndex = 6;
      this.btnDstDir.Text = "...";
      this.btnDstDir.UseVisualStyleBackColor = true;
      this.btnDstDir.Click += new EventHandler(this.btnDstDir_Click);
      this.tbDstDir.Location = new Point(123, 17);
      this.tbDstDir.Name = "tbDstDir";
      this.tbDstDir.Size = new Size(381, 20);
      this.tbDstDir.TabIndex = 5;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 21);
      this.label3.Name = "label3";
      this.label3.Size = new Size(105, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Destination Directory";
      this.groupBox3.Controls.Add((Control) this.lbTables);
      this.groupBox3.Location = new Point(13, 359);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(557, 369);
      this.groupBox3.TabIndex = 8;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Select tables to generate";
      this.lbTables.FormattingEnabled = true;
      this.lbTables.Location = new Point(-1, 19);
      this.lbTables.Name = "lbTables";
      this.lbTables.SelectionMode = SelectionMode.MultiExtended;
      this.lbTables.Size = new Size(557, 342);
      this.lbTables.TabIndex = 0;
      this.btnGenerate.Location = new Point(13, 734);
      this.btnGenerate.Name = "btnGenerate";
      this.btnGenerate.Size = new Size(97, 23);
      this.btnGenerate.TabIndex = 9;
      this.btnGenerate.Text = "&Generate Code";
      this.btnGenerate.UseVisualStyleBackColor = true;
      this.btnGenerate.Click += new EventHandler(this.button1_Click);
      this.button2.Location = new Point(494, 734);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 10;
      this.button2.Text = "C&ancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.txtVONS.Location = new Point(123, 99);
      this.txtVONS.Name = "txtVONS";
      this.txtVONS.Size = new Size(381, 20);
      this.txtVONS.TabIndex = 17;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 103);
      this.label6.Name = "label6";
      this.label6.Size = new Size(82, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "VO Namespace";
      this.txtCommonNS.Location = new Point(123, 126);
      this.txtCommonNS.Name = "txtCommonNS";
      this.txtCommonNS.Size = new Size(381, 20);
      this.txtCommonNS.TabIndex = 19;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 130);
      this.label7.Name = "label7";
      this.label7.Size = new Size(66, 13);
      this.label7.TabIndex = 18;
      this.label7.Text = "Common NS";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(582, 773);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.btnGenerate);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Name = nameof (Form1);
      this.Text = "POCO Generator v 1.0";
      this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

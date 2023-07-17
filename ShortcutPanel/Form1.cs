using ShortcutPanel.Class;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ShortcutPanel
{
    public partial class Form1 : Form
    {
        DB_Connection connection = new DB_Connection();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            DataTable dt = new DataTable();
            dt = connection.getData();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var control in this.Controls)
                    {
                        if (control.GetType() == typeof(Button))
                        {
                            Button button = (Button)control;
                            if (button.Name == row["buttonRef"].ToString())
                            {
                                if (row["flag"].ToString() == "0")
                                {

                                    button.BackColor = Color.LightGray;
                                    button.Image = Image.FromFile(row["path"].ToString());
                                    button.BackgroundImageLayout = ImageLayout.Zoom;
                                    button.Click += new EventHandler(Add_Shortcut);
                                }
                                else
                                {
                                    string appPath = row["path"].ToString();
                                    Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(appPath);

                                    Bitmap iconBitmap = appIcon.ToBitmap();
                                    Bitmap resizedIcon = new Bitmap(iconBitmap, new Size(50, 50));

                                    button.Image = resizedIcon;
                                    button.BackColor = Color.Transparent;
                                    button.Click += new EventHandler(Open_Shortcut);
                                    button.MouseUp += new MouseEventHandler(GenerateMenu);

                                }

                            }
                        }
                    }
                }

            }
            else
            {
                foreach (var control in this.Controls)
                {
                    if (control.GetType() == typeof(Button))
                    {
                        Button button = (Button)control;
                        connection.insertData(button.Name, "D:\\Projects\\ShortcutPanel\\ShortcutPanel\\img\\plus.png", button.Name, "0");

                        button.BackColor = Color.LightGray;
                        button.Image = Image.FromFile("D:\\Projects\\ShortcutPanel\\ShortcutPanel\\img\\plus.png");
                        button.BackgroundImageLayout = ImageLayout.Zoom;
                    }
                }
            }

        }

        private void Add_Shortcut(object sender, EventArgs e)
        {

            Button button = (Button)sender;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File eseguibile (*.*) | *.*";
            ofd.Title = "Seleziona un applicativo";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string appPath = ofd.FileName;

                Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(appPath);

                Bitmap iconBitmap = appIcon.ToBitmap();
                Bitmap resizedIcon = new Bitmap(iconBitmap, new Size(50, 50));

                button.Image = resizedIcon;
                button.BackColor = Color.Transparent;

                button.Click -= Add_Shortcut;

                button.Click += new EventHandler(Open_Shortcut);
                button.MouseUp += new MouseEventHandler(GenerateMenu);

                connection.updateData(System.IO.Path.GetFileName(ofd.FileName), appPath, button.Name, "1");
            }
        }

        private void Delete_Shortcut(object sender, EventArgs e, Button button)
        {
            connection.updateData(button.Name, "D:\\Projects\\ShortcutPanel\\ShortcutPanel\\img\\plus.png", button.Name, "0");

            button.BackColor = Color.LightGray;
            button.Image = Image.FromFile("D:\\Projects\\ShortcutPanel\\ShortcutPanel\\img\\plus.png");
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.Click -= Open_Shortcut;
            button.Click += new EventHandler(Add_Shortcut);
        }


        private void Modify_Shortcut(object sender, EventArgs e, Button button)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File eseguibile (*.*) | *.*";
            ofd.Title = "Seleziona un applicativo";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string appPath = ofd.FileName;
                Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(appPath);

                Bitmap iconBitmap = appIcon.ToBitmap();
                Bitmap resizedIcon = new Bitmap(iconBitmap, new Size(50, 50));

                button.Image = resizedIcon;
                button.BackColor = Color.Transparent;

                connection.updateData(System.IO.Path.GetFileName(ofd.FileName), appPath, button.Name, "1");
            }
        }

        private void Open_Shortcut(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            DataTable data = connection.selectData(button.Name);
            System.Diagnostics.Process.Start(new ProcessStartInfo(data.Rows[0]["path"].ToString()) { UseShellExecute = true });

        }



        public void GenerateMenu(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                Button button = (Button)sender;

                ContextMenuStrip cm = new ContextMenuStrip();
                cm.Items.Add("Modifica", Image.FromFile("D:\\Projects\\ShortcutPanel\\ShortcutPanel\\img\\edit.png"), delegate (object sender, EventArgs e) { Modify_Shortcut(sender, e, button); });
                cm.Items.Add("Cancella", Image.FromFile("D:\\Projects\\ShortcutPanel\\ShortcutPanel\\img\\delete.png"), delegate (object sender, EventArgs e) { Delete_Shortcut(sender, e, button); });

                cm.Show(Cursor.Position);
            }

        }
    }

}
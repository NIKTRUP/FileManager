using System;
using System.IO.Compression;    
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class FileManager : Form
    {
        DirectoryInfo directory;
        DirectoryInfo[] directorys;
        FileInfo[] files;
        ListViewItem listViewItem;
        bool listView1IsActive = false;
        bool listView2IsActive = false;
        public FileManager()
        {
            InitializeComponent();
            LoadData();
        }

        private void FileManager_Load(object sender, EventArgs e)
        {
            textBox1.Text = "C:\\Program Files (x86)";
            textBox2.Text = "D:\\Program Files (x86)";
            LoadFolderContents(listView1, textBox1);
            LoadFolderContents(listView2, textBox2);
        }

        private void LoadData()
        {
            listView1.Items.Clear();

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(20, 20);
            imageList.Images.Add(Image.FromFile("C:\\Users\\nikol\\source\\repos\\FileManager\\Images\\Folder.png"));
            imageList.Images.Add(Image.FromFile("C:\\Users\\nikol\\source\\repos\\FileManager\\Images\\File.png"));
            imageList.Images.Add(Image.FromFile("C:\\Users\\nikol\\source\\repos\\FileManager\\Images\\txtFile.png"));
            imageList.Images.Add(Image.FromFile("C:\\Users\\nikol\\source\\repos\\FileManager\\Images\\pdfFile.png"));

            /*
            Bitmap emptyImage = new Bitmap(20,20);

            Graphics graphics = Graphics.FromImage(emptyImage);
            graphics.Clear(Color.White);
            imageList.Images.Add(emptyImage);
            */
            listView1.SmallImageList = imageList;
            listView2.SmallImageList = imageList;
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GoToFolder(listView1 , textBox1);
        }
        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GoToFolder(listView2, textBox2);
        }
        private void GoToFolder(ListView listView, TextBox textBox)// TO DO
        {
            try
            {
                int index = listView.FocusedItem.Index;
                if (Directory.Exists(directorys[index].FullName))
                {
                    textBox.Text = directorys[index].FullName;
                    LoadFolderContents(listView, textBox);
                }
                else if (File.Exists(files[index].FullName))
                {
                    Process.Start(files[index].FullName); // ошибка при открытии файла
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadFolderContents(ListView listView, TextBox textBox)//TO DO : проверка , если файл не опознан
        {
           // listView.BeginUpdate();
            listView.Items.Clear();

            directory = new DirectoryInfo(textBox.Text);
            directorys = directory.GetDirectories();
            foreach (DirectoryInfo currentDir in directorys)
            {
                listViewItem = new ListViewItem(new string[]{ currentDir.Name, "Папка с файлами" });
                listViewItem.ImageIndex = 0;
                listView.Items.Add(listViewItem);
            }

            files = directory.GetFiles();
            foreach (FileInfo currentFile in files)
            {
                listViewItem = new ListViewItem(new string[] { currentFile.Name, currentFile.Extension });
                    
                switch (Path.GetExtension(currentFile.Name))
                {
                    case ".txt":
                        listViewItem.ImageIndex = 2;// поменять картинку для txt файла
                        break;
                    case ".pdf":
                        listViewItem.ImageIndex = 3;
                        break;
                    default:
                        listViewItem.ImageIndex = 1;
                        break;
                }
                listView.Items.Add(listViewItem);
            }
            //listView.EndUpdate();
        }

        private void bt_Next_Click(object sender, EventArgs e)
        {
            if (listView1IsActive == true)
            {
                GoToFolder(listView1, textBox1);
            }
            else if (listView2IsActive == true)
            { 
                GoToFolder(listView2, textBox2);
            }
        }
        private void bt_Back_Click(object sender, EventArgs e) // TO DO: можно заменить textBox1.Text .
        {
            if (listView1IsActive == true)
            {
                GoBack(listView1 , textBox1);
            }
            else if (listView2IsActive == true)
            {
                GoBack(listView2, textBox2);
            }
        }
        private void GoBack(ListView listView, TextBox textBox)
        {
            if (textBox.Text == directory.Root.Name)
            {
                MessageBox.Show("Вы не можете выйти из корневого каталога");
            }
            else
            {
                if (textBox.Text[textBox.Text.Length - 1] == '\\')
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1, 1);

                    while (textBox.Text[textBox.Text.Length - 1] != '\\')
                    {
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1, 1);
                    }
                }
                else if (textBox.Text[textBox.Text.Length - 1] != '\\')
                {
                    while (textBox.Text[textBox.Text.Length - 1] != '\\')
                    {
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1, 1);
                    }
                }
                LoadFolderContents(listView, textBox);
            }
        }
        private void listView1_Click(object sender, EventArgs e)
        {
            listView1IsActive = true;
            listView2IsActive = false;
        }
        private void listView2_Click(object sender, EventArgs e)
        {
            listView2IsActive = true;
            listView1IsActive = false;
        }
        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView1IsActive == true && e.KeyCode == Keys.Enter)
            {
                GoToFolder(listView1, textBox1);
            }
        }
        private void listView2_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView2IsActive == true && e.KeyCode == Keys.Enter)
            {
                GoToFolder(listView2, textBox2);
            }
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (listView1IsActive == true && e.Button == MouseButtons.Right)
            {
                contextMenu.Show(MousePosition , ToolStripDropDownDirection.Right );  
            }
        }
        private void listView2_MouseUp(object sender, MouseEventArgs e)
        {
            if (listView2IsActive == true && e.Button == MouseButtons.Right)
            {
                contextMenu.Show(MousePosition, ToolStripDropDownDirection.Right);
            }
        }
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listView1.FocusedItem.Index;
                MessageBox.Show($"Полный путь директории {directorys[index].FullName} , её номер {index}  , {directorys[index].Exists} ");
                
                if (File.Exists(files[index].FullName) == true)
                {
                    files[index].Delete();
                }
                else if (Directory.Exists(directorys[index].FullName) == true)
                {
                    if (Directory.GetDirectories(directorys[index].FullName).Length + Directory.GetFiles(directorys[index].FullName).Length > 0)
                    {
                        var result = MessageBox.Show($"Папка {directorys[index].Name} не пуста , вы точно хотите удалить её ?", "Внимание"
                            , MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            directorys[index].Delete(true);
                        }
                        else { }
                    }
                    else { directorys[index].Delete(); }
                }
                listViewItem = listView1.SelectedItems[0];
                listView1.Items.Remove(listViewItem);
                LoadFolderContents(listView1, textBox1);
            }
            catch(Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            bt_Next_Click(sender, e);
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!directory.Exists)//не создаётся
            {
                directory.Create();
                LoadFolderContents(listView1, textBox1);
                MessageBox.Show("Файл создан");
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listView1.FocusedItem.Index;
            Clipboard.SetData(Path.GetExtension(files[index].FullName),files[index]);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void архивироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listView1.FocusedItem.Index;
                ZipFile.CreateFromDirectory(directorys[index].FullName, @"{directorys[index].FullName}.zip");
                // ZipFile.ExtractToDirectory("C:\\temp\\arcex.zip", "C:\\temp\\arcex");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

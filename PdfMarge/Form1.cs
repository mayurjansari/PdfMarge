using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using MYR;

namespace PdfMarge
{
    public partial class Form1 : Form
    {
        proActivation objActivation = new proActivation();
        bool isDemo = true;
        public Form1()
        {
            InitializeComponent();
            this.InitTimer();
        }
        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 3000; // in miliseconds
            timer1.Start();
        }
        public void timer1_Tick(object sender, EventArgs e)
        {

            isDemo = objActivation.IsDemo("pdfMerge");
            if (isDemo)
                System.Windows.Forms.Application.Exit();
        }
        public static bool MergePDFs(IEnumerable<string> fileNames, string targetPdf)
        {
            bool merged = true;
            using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (string file in fileNames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            return merged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //string[] files = Directory.GetFiles(fbd.SelectedPath);
                    //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.Title = "Save pdf Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (!Directory.Exists(textBox2.Text))
            //    Directory.CreateDirectory(textBox2.Text);

            var files = Directory.EnumerateFiles(textBox1.Text, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".pdf"));
            Cursor.Current = Cursors.WaitCursor;
            using (FileStream stream = new FileStream(textBox2.Text, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (string file in files)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    MessageBox.Show("Please try Again");
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("file Marge SuccessFully");
        }
    }
}

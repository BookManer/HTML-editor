using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace HTML_editor
{

    public partial class Form1 : Form
    {

        public string bufferText = "Привет, меня зовут Андрей\nА как зовут тебя!?)\nНовая строка\nСнова новая строка :)";
        List<List<MatchObject>> savedMatches = new List<List<MatchObject>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int indexCurrentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);

            webBrowser1.DocumentText = richTextBox1.Text;
            

            if (richTextBox1.Lines.Length != 0)
            {
                richTextBox1.PaintText();
               /* var matches = Regex.Matches(richTextBox1.Text, @"</?\w+>");

                int index = richTextBox1.SelectionStart;

                foreach (var match in matches.Cast<Match>())
                {
                    richTextBox1.Select(match.Index, match.Length);
                    richTextBox1.SelectionColor = Color.Orange;
                    richTextBox1.SelectionStart = index;
                }

                richTextBox1.SelectionColor = Color.White;

                /*isContainMatch = false;
                foreach (MatchObject mObject in savedMatches.ElementAt(indexCurrentLine)) {
                    Console.WriteLine(richTextBox1.SelectionStart + " : " + mObject.endIndexCharInLine);
                    if (mObject.endIndexCharInLine <= richTextBox1.SelectionStart-1)
                    {
                        isContainMatch = true;
                    }
                }

                if (!isContainMatch)
                {
                    List<MatchObject> matchObjects = savedMatches.ElementAt(indexCurrentLine);
                    matchObjects.Add(new MatchObject(match.Value, richTextBox1.SelectionStart + match.Value.Length));
                    richTextBox1.Text.Replace(richTextBox1.Text.Substring(1, match.Value.Length - 1), "");
                    int size;

                    if (match.Value.Contains("/"))
                    {
                        size = richTextBox1.Lines.Length-1;
                    } else
                    {
                        size = richTextBox1.Lines.Length;
                    }

                    for (int i = 0; i < size; i++)
                    {
                        richTextBox1.AppendText("   ", Color.White);
                    }

                }*/
            }
            
            /* if (richTextBox1.Lines[indexCurrentLine].Contains("<") &&
                richTextBox1.Lines[indexCurrentLine].Contains(">") &&
                (indexStartTag < indexEndTag))
            {
                richTextBox1.Select(richTextBox1.Lines[indexCurrentLine].LastIndexOf('<'), richTextBox1.Lines[indexCurrentLine].LastIndexOf('>')+1);
                richTextBox1.SelectionColor = Color.Magenta;
                int indexStartFirstAttr = currentLine.IndexOf(' ');
                if (indexStartFirstAttr != -1)
                {
                    int lsdg = currentLine.LastIndexOf(' ');
                    int indexEndLastAttr = currentLine.Substring(2, indexEndTag-1).Length;
                    richTextBox1.AppendText("\n\n"+currentLine.Substring(indexStartTag, indexStartFirstAttr) + currentLine.Substring(indexEndLastAttr, indexEndTag) + "\n");
                } else
                {
                    richTextBox1.AppendText("\n\n</"+currentLine.Substring(indexStartTag+1, indexEndTag) + "\n");
                }
            }*/
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            richTextBox1.Size = splitContainer1.Panel2.Size;
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
           if(e.KeyCode == Keys.ControlKey && e.KeyCode == Keys.V)
            {
                string newStrings = Clipboard.GetText();
                List<string> lines = richTextBox1.Lines.Cast<string>().ToList();
                for (int i = 0; i < bufferText.Length; i++)
                {
                    if (bufferText[i] != '\n')
                    {
                        newStrings += bufferText[i];
                    }
                    else
                    {
                        lines.Insert(richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart), newStrings);
                        newStrings = "";
                    }
                }
                richTextBox1.Lines = lines.ToArray();
            } 
        }

        public void insertTextFromClipboard(object sender, EventArgs e)
        {
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                StreamReader read = new StreamReader(openFileDialog1.FileName);
                richTextBox1.Text = read.ReadToEnd();
               // richTextBox1.PaintText();
                read.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                StreamWriter write = new StreamWriter(saveFileDialog1.FileName);
                write.WriteLine(richTextBox1.Text);
                write.Close();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void RichTextBox1_Leave(object sender, EventArgs e)
        {
            //richTextBox1.PaintText();
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

    class MatchObject
    {
        public string Value;
        public int endIndexCharInLine;

        public MatchObject(string v, int sicil)
        {
            this.Value = v;
            this.endIndexCharInLine = sicil;
        }
    }

}

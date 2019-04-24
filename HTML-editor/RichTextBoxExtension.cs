using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HTML_editor
{
    public static class RichTextBoxExtension
    {
        private static RichTextBox _richTextBox;
        private static List<Tuple<int, int, Color>> _marks = new List<Tuple<int, int, Color>>();
        private static void PrintElement(XElement element)
        {
            _richTextBox.Text += "<";
            _marks.Add(new Tuple<int, int, Color>(_richTextBox.Text.Length, element.Name.LocalName.Length, Color.Tomato));
            _richTextBox.Text += element.Name;
            _richTextBox.Text += " ";
            if(element.HasAttributes)
            {
                foreach(var el in element.Attributes())
                {
                    _marks.Add(new Tuple<int, int, Color>(_richTextBox.Text.Length, el.Name.LocalName.Length, Color.Orange));
                    _richTextBox.Text += el.Name + "=";

                    if (!string.IsNullOrEmpty(el.Value))
                    {
                        _marks.Add(new Tuple<int, int, Color>(_richTextBox.Text.Length+1, el.Value.Length, Color.Green));
                        _richTextBox.Text += "\""+el.Value + "\" ";

                    }
                    else
                        _richTextBox.Text += "\"\"";
                }
            }
            if (!string.IsNullOrEmpty(element.Value) && !element.HasElements)
            {
                _richTextBox.Text += ">" + element.Value + "</";
                _marks.Add(new Tuple<int, int, Color>(_richTextBox.Text.Length, element.Name.LocalName.Length, Color.Tomato));
                _richTextBox.Text += element.Name + ">\n";
            }
            else if(string.IsNullOrEmpty(element.Value) && !element.HasElements)
            {
                _richTextBox.Text += "/>\n";

            }
            else _richTextBox.Text += ">\n";

            if (element.HasElements)
            {
                foreach (var el in element.Elements())
                    PrintElement(el);
                _richTextBox.Text += "</";
                _marks.Add(new Tuple<int, int, Color>(_richTextBox.Text.Length, element.Name.LocalName.Length, Color.Tomato));
                _richTextBox.Text += element.Name + ">\n";
            }


        }
        public static void  PaintText(this RichTextBox textBox)
        {
            _richTextBox = textBox;
            var text = textBox.Text;
            XDocument document = XDocument.Parse(text);
            textBox.Clear();
            _marks.Clear();
            var elements = document.Elements();
            foreach (var el in elements)
            {
                PrintElement(el);
                
            }
            foreach(var el in _marks)
            {
                textBox.SelectionStart = el.Item1;
                textBox.SelectionLength = el.Item2;
                textBox.SelectionColor = el.Item3;
            }
    
            

        }
        public static Task PaintTextAsync(this RichTextBox textBox)
        {
            return Task.Run(() =>
            {

            });
        }
    }
}

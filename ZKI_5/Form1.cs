using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZKI_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private int language; //true - англ, false - русск
        private int action; //true - шифрование, false - расшифр.
        private int rotation; //true - rto0, false - rto1
        private int vuborShifra; //true - таблица, false - формула
        private int count;

        //вывод таблиц
        void PrintdataGridView(char[,] m)
        {
            dataGridView1.Visible = true;
            dataGridView1.ColumnCount = m.GetLength(0);
            dataGridView1.RowCount = m.GetLength(1);

            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    dataGridView1.Columns[i].Width = 20;
                    dataGridView1.Rows[i].Height = 20;
                    dataGridView1.Rows[i].Cells[j].Value = char.ToString(m[i, j]);
                }
            }
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    if (i == 0 | j == 0)
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.PapayaWhip;
                    else
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
        }
       
        //создание матрицы с алфавитом
        char[,] NewMatrix(char[] a, bool flag)
        {
            int t = a.Length + 1;
            int m = a.Length;
            char[,] matrix = new char[t, t];
            char[] alfAn1 = new char[m];

            for (int i = 0; i < m; i++)
                alfAn1[i] = a[i];
            int n = 0;
            char b = ' ';
            matrix[0, 0] = ' ';
            for (int i = 1; i < t; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (j == 0)
                    {
                        matrix[i, j] = alfAn1[n];
                        n++;
                    }
                }
            }
            n = 0;
            for (int i = 0; i < t; i++)
            {
                for (int j = 1; j < t; j++)
                {
                    if (i == 0)
                    {
                        matrix[i, j] = alfAn1[n];
                        n++;
                    }
                }
            }
            //если flag == true, то сдвиг в матрице на 1
            if (flag == true)
            {
                b = alfAn1[0];
                for (int l = 0; l < alfAn1.Length - 1; l++)
                    alfAn1[l] = alfAn1[l + 1];
                alfAn1[m - 1] = b;
            }

            n = 0;
            for (int j = 1; j < t; j++)
            {
                for (int i = 1; i < t; i++)
                {
                    matrix[i, j] = alfAn1[n];
                    n++;
                }
                b = alfAn1[0];
                for (int l = 0; l < alfAn1.Length - 1; l++)
                {
                    alfAn1[l] = alfAn1[l + 1];
                }
                alfAn1[m - 1] = b;
                n = 0;
            }
            return matrix;
        }

        //обработка входных строк
        string StringCheck(string s)
        {
            s = s.ToUpper();
            s = s.Replace(" ", "");
            s = s.Replace(",", "");
            s = s.Replace(".", "");
            s = s.Replace("!", "");
            s = s.Replace("&", "");
            s = s.Replace("-", "");
            s = s.Replace(";", "");
            return s;
        }

        //подстановка ключа
        string KeyGenerate(string text, string k)
        {
            char[] kk = new char[text.Length];
            int a = 0;
            for (int i = 0; i < text.Length; i++)
            {
                kk[i] = k[a];
                a++;
                if (a == k.Length)
                    a = 0;
            }
            string key = "";
            for (int i = 0; i < kk.Length; i++)
                key += kk[i];
            return key;
        }

        //шифрование
        string Shifr(char[,] matrix, string text, string key)
        {
            int index1 = 0, index2 = 0;
            string rezult = "";
            int n = matrix.GetLength(0) - 2;
            for (int z = 0; z < text.Length; z++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (text[z] == matrix[1, j])
                            index1 = j;
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (key[z] == matrix[i, 1])
                            index2 = i;
                    }
                }
                rezult += matrix[index1, index2];
            }
            return rezult;
        }

        //расшифровка
        string RasShifr(char[,] matrix, string text, string key)
        {
            int index1 = 0, index2 = 0;
            string rezult2 = "";
            int n = matrix.GetLength(0) - 1;
            //
            for (int z = 0; z < key.Length; z++)
            {
                for (int i = 1; i < n; i++)
                {
                    if (key[z] == matrix[i, 0])
                    {
                        index1 = i;
                        break;
                    }
                }
                for (int j = 1; j < n; j++)
                {
                    if (matrix[index1, j] == text[z])
                    {
                        rezult2 += matrix[0, j];
                        index2 = j;
                        break;
                    }
                }
            }
            return rezult2;
        }

        //шифрование по формуле
        string Formul(string alf, string text, string key)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            string rezult = "";
            int[] mas1 = new int[text.Length];
            int[] mas2 = new int[key.Length];
            int[] mas3 = new int[text.Length];
            int g = alf.Length-1;
            int n = 0;
            for (int j = 0; j < text.Length; j++)
            {
                for (int i = 0; i < alf.Length; i++)
                {
                    if (text[j] == alf[i])
                    {
                        mas1[n] = i;
                        n++;
                        break;
                    }
                }
            }
            n = 0;
            for (int j = 0; j < key.Length; j++)
            {
                for (int i = 0; i < alf.Length; i++)
                {
                    if (key[j] == alf[i])
                    {
                        mas2[n] = i;
                        n++;
                        break;
                    }
                }
            }
            for (int i = 0; i < mas3.Length; i++)
            {
                mas3[i] = mas1[i] + mas2[i];
                if (mas3[i] > g)
                {
                    mas3[i] = mas3[i] - g;
                }
            }
            for (int j = 0; j < mas3.Length; j++)
            {
                for (int i = 0; i < alf.Length; i++)
                {
                    if (mas3[j] == i)
                    {
                        rezult += alf[i];
                    }
                }
            }
            ///////////////////////////////////////////////////
            //заполнение таблицы значениями
            dataGridView1.Visible = true;

            for (int i = 0; i < alf.Length; i++)
            {
                dataGridView1.Columns.Add(Convert.ToString(alf[i]), Convert.ToString(i));
                dataGridView1.Columns[i].Width = 25;
            }
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            for (int i = 0; i < alf.Length; i++)
                dataGridView1.Rows[0].Cells[i].Value = alf[i];
            for (int i = 0; i < alf.Length; i++)
                dataGridView1.Rows[1].Cells[i].Value = " ";

            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            for (int i = 0; i < mas1.Length; i++)
                dataGridView1.Rows[2].Cells[i].Value = mas1[i];
            for (int i = 0; i < text.Length; i++)
                dataGridView1.Rows[3].Cells[i].Value = text[i];
            for (int i = 0; i < text.Length; i++)
                dataGridView1.Rows[4].Cells[i].Value = "+";
            for (int i = 0; i < mas2.Length; i++)
                dataGridView1.Rows[5].Cells[i].Value = mas2[i];
            for (int i = 0; i < key.Length; i++)
                dataGridView1.Rows[6].Cells[i].Value = key[i];
            for (int i = 0; i < text.Length; i++)
                dataGridView1.Rows[7].Cells[i].Value = "| |";
            for (int i = 0; i < mas3.Length; i++)
                dataGridView1.Rows[8].Cells[i].Value = mas3[i];
            for (int i = 0; i < rezult.Length; i++)
                dataGridView1.Rows[9].Cells[i].Value = rezult[i];
            return rezult;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            count = 0;
            label6.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            button2.Visible = false;
            textBox4.Visible = false;
            if (language == 1)//англ
            {
                if (action == 1)//шифр
                {
                    //таблица
                    if (vuborShifra == 1)
                    {//rOt0
                        if (rotation == 1)
                        {
                            char[] alfAn = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', };
                            char[,] matrix = NewMatrix(alfAn, false);

                            string text = textBox1.Text;
                            string k = textBox2.Text;
                            text = StringCheck(text);
                            k = StringCheck(k);
                            string key = KeyGenerate(text, k);

                            PrintdataGridView(matrix);

                            string rezult = Shifr(matrix, text, key);
                            label6.Text += "Открытый текст: " + text + " \nКлюч: " + key;
                            textBox3.Text += rezult;
                        }
                        //rot1
                        else if (rotation == 2)
                        {
                            char[] alfAn = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', };
                            char[,] matrix = NewMatrix(alfAn, true);

                            string text = textBox1.Text;
                            string k = textBox2.Text;

                            text = StringCheck(text);
                            k = StringCheck(k);
                            string key = KeyGenerate(text, k);

                            PrintdataGridView(matrix);

                            string rezult = Shifr(matrix, text, key);
                            label6.Text += "Открытый текст: " + text + " \nКлюч: " + key;
                            textBox3.Text += rezult;

                        }
                        button2.Visible = true;
                        //////////////////////////////////////////////////
                    }
                    //формула
                    else if (vuborShifra == 2)
                    {
                        string alf = "ABCDEIFGHIJKLMNOPQRSTUVWXWZ";

                        string text = textBox1.Text;
                        string k = textBox2.Text;
                        text = StringCheck(text);
                        k = StringCheck(k);
                        string key = KeyGenerate(text, k);

                        string rezult = Formul(alf, text, key);
                        
                        label6.Text += "Открытый текст: " + text + " \nКлюч: " + key;
                        textBox3.Text += rezult;
                    }
                }   //англ расшифр 
                else if (action == 2)
                {
                    char[] alfAn = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', };
                    char[,] matrix = NewMatrix(alfAn, true);

                    string text = textBox1.Text;
                    string k = textBox2.Text;

                    text = StringCheck(text);
                    k = StringCheck(k);
                    string key = KeyGenerate(text, k);

                    PrintdataGridView(matrix);

                    string rezult = RasShifr(matrix, text, key);
                    label6.Text += "Шифртекст: " + text + " \nКлюч: " + key;
                    textBox3.Text += rezult;
                }
            }//русск
            else if (language == 2)
            {
                if (action == 1)//шифр
                {
                    //таблица
                    if (vuborShifra == 1)
                    {//rOt0
                        if (rotation == 1)
                        {
                            char[] alfAn = new char[33] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };
                            char[,] matrix = NewMatrix(alfAn, false);

                            string text = textBox1.Text;
                            string k = textBox2.Text;

                            text = StringCheck(text);
                            k = StringCheck(k);
                            string key = KeyGenerate(text, k);

                            PrintdataGridView(matrix);

                            string rezult = Shifr(matrix, text, key);
                            label6.Text += "Открытый текст: " + text + " \nКлюч: " + key;
                            textBox3.Text += rezult;
                        }
                        //rot1
                        else if (rotation == 2)
                        {
                            char[] alfAn = new char[33] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };
                            char[,] matrix = NewMatrix(alfAn, true);

                            string text = textBox1.Text;
                            string k = textBox2.Text;

                            text = StringCheck(text);
                            k = StringCheck(k);
                            string key = KeyGenerate(text, k);

                            PrintdataGridView(matrix);

                            string rezult = Shifr(matrix, text, key);
                            label6.Text += "Открытый текст: " + text + " \nКлюч: " + key;
                            textBox3.Text += rezult;
                        }
                        button2.Visible = true;
                        ////////////////////////////////////////////////
                    }//формула
                    else if (vuborShifra == 2)
                    {
                        string alf = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

                        string text = textBox1.Text;
                        string k = textBox2.Text;
                        text = StringCheck(text);
                        k = StringCheck(k);
                        string key = KeyGenerate(text, k);

                        string rezult = Formul(alf, text, key);
                        label6.Text += "Открытый текст: " + text + " \nКлюч: " + key;
                        textBox3.Text += rezult;
                    }
                }   //русск расшифр 
                else if (action == 2)
                {
                    char[] alfAn = new char[33] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };
                    char[,] matrix = NewMatrix(alfAn, true);

                    ////rot1
                    string text = textBox1.Text;
                    string k = textBox2.Text;

                    text = StringCheck(text);
                    k = StringCheck(k);
                    string key = KeyGenerate(text, k);

                    PrintdataGridView(matrix);

                    string rezult = RasShifr(matrix, text, key);
                    label6.Text += "Шифртекст: " + text + " \nКлюч: " + key;
                    textBox3.Text += rezult;
                }
            }
            else
            {
                MessageBox.Show("Дэбил заполнить поля надо", "Ошибка человечества");
            }
        }
        //обработчики событий
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                language = 1; //англ.
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                language = 2; //русск.
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                action = 1; //зашифровать
                panel4.Visible = true;
            }
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                action = 2; //расшифровать
                panel3.Visible = false;
                panel4.Visible = false;
            }
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                rotation = 1; //ROT0
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                rotation = 2; //ROT1
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                vuborShifra = 1; //готовая таблица
                panel3.Visible = true;
                dataGridView1.Visible = false;
            }
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                vuborShifra = 2; //формула
                panel3.Visible = false;
            }
        }
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Clear();
        }
        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2.Clear();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox2.Focus();
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Visible = true;


            if (textBox4.Text == "")
                count = 0;
            else
                count = textBox4.Text.Length;

            if (count == textBox3.Text.Length)
            {
                button2.Visible = false;
                dataGridView1.Visible = false;
            }
            else
            {
                string text = textBox1.Text;
                string k = textBox2.Text;
                text = StringCheck(text);
                k = StringCheck(k);
                string key = KeyGenerate(text, k);

                object[,] m = new object[dataGridView1.Rows.Count, dataGridView1.Columns.Count]; //заносим данные из таблица в массив
                for (int x = 0; x < m.GetLength(0); x++)
                    for (int i = 0; i < m.GetLength(1); i++)
                        m[x, i] = dataGridView1.Rows[x].Cells[i].Value;

                for (int i = 0; i < m.GetLength(0); i++)//размеры ячейки
                {
                    for (int j = 0; j < m.GetLength(1); j++)
                    {
                        dataGridView1.Columns[i].Width = 20;
                        dataGridView1.Rows[i].Height = 20;
                        dataGridView1.Rows[i].Cells[j].Value = (m[i, j]);
                    }
                }

                char[,] matrix = new char[dataGridView1.Rows.Count, dataGridView1.Columns.Count];//переносим в нормальный массив букв
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    for (int j = 0; j < m.GetLength(1); j++)
                        matrix[i, j] = Convert.ToChar(m[i, j]);
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (i == 0 | j == 0)
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.PapayaWhip;
                        else
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                    }
                }
                int index1 = 0, index2 = 0;
                int n = m.GetLength(0) - 2;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (text[count] == matrix[1, j])
                            index1 = j;
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (key[count] == matrix[i, 1])
                            index2 = i;
                    }
                }

                for (int i = 1; i < m.GetLength(0); i++)
                {
                    for (int j = 1; j < m.GetLength(1); j++)
                    {
                        if (i == index1 | j == index2)
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightCyan;
                        if (i == index1 & j == index2)
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                    }
                }
                textBox4.Text += matrix[index1, index2];
                count++;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            button3.Visible = false;
            textBox4.Text = "";
            textBox3.Text = "";
            textBox4.Visible = false;
            label6.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            radioButton7.Checked = false;
            radioButton8.Checked = false;
            textBox1.Text = "Введите текст";
            textBox2.Text = "Введите ключ";
        }
    }
}
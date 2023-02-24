using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculator
{
    public partial class MainForm : Form
    {
        // 窗体默认宽高
        int normalWidth = 0;
        int normalHeight = 0;
        // 需要记录的控件的位置以及宽高(x,y, width, height)
        Dictionary<string, Rectangle> normalControlMap = new Dictionary<string, Rectangle>();

        private string _number1;

        public string Number1
        {
            get { return _number1; }
            set 
            {
                _number1 = value; 
                this.label1.Text = value;
            }
        }

        private string _number2;

        public string Number2
        {
            get { return _number2; }
            set 
            {
                _number2 = value;
                this.label1.Text = Number1+ Operator + value;
            }
        }

        private string _operator;

        public string Operator
        {
            get { return _operator; }
            set {
                _operator = value;
                this.label1.Text = Number1 + value;
            }
        }

        List<string> _operatList = new List<string>();
        /**
         * 1. 记录原比例
         * 窗口加载的时候完成
         * 2. 根据原始比例进行新尺寸定位计算
         * 3. 将新的定位尺寸设置给控件
         */

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            this.SizeChanged += MainForm_SizeChanged;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            // 根据原始比例进行新尺寸的计算
            int w = this.btnPanel.Width; // 新的宽度
            int h = this.btnPanel.Height; // 新的高度
            foreach (Control item in this.btnPanel.Controls)
            {
                int newX = (int)(w * 1.0 / normalWidth * normalControlMap[item.Name].X);
                int newY = (int)(h * 1.0 / normalHeight * normalControlMap[item.Name].Y);
                int newW = (int)(w * 1.0 / normalWidth * normalControlMap[item.Name].Width);
                int newH = (int)(h * 1.0 / normalHeight * normalControlMap[item.Name].Height);

                item.Left = newX;
                item.Top = newY;
                item.Width = newW;
                item.Height = newH;
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 记录相关对象以及原始尺寸
            normalWidth = this.btnPanel.Width;
            normalHeight = this.btnPanel.Height;
            // 通过父Panel进行控件的便利
            foreach (Control item in this.btnPanel.Controls)
            {
                normalControlMap.Add(item.Name, new Rectangle(item.Left, item.Top, item.Width, item.Height));
            }
        }
        private void btnClick(object sender, EventArgs e)
        {
            // 获取到触发这个事件的对象
            // 拿到对象后获取对象的Text属性值
            Button btn = sender as Button;

            // 当运算符为空时一直往第一个算数中添加
            if (string.IsNullOrEmpty(Operator))
            {
                if (btn.Text == "." && Number1?.IndexOf(".") <= 0)
                {
                    this.Number1 += btn.Text;
                }
                else if (btn.Text != ".")
                {
                    this.Number1 += btn.Text;
                }
            }
            else
            {
                if (btn.Text == "." && Number2?.IndexOf(".") <= 0)
                {
                    this.Number2 += btn.Text;
                }
                else if (btn.Text != ".")
                {
                    this.Number2 += btn.Text;
                }
            }

            
        }

        private void operationClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string op = btn.Text;
            if (!string.IsNullOrEmpty(Number2))
            {
                // 执行计算 计算结果给number1
                btnCalculate(null, null);
            }
            this.Operator = op;
        }

        private void btnCalculate(object sender, EventArgs e)
        {
            double n1 = double.Parse(Number1);
            double n2 = double.Parse(Number2);

            switch (Operator) 
            {
                case "+":
                    Number1 =(n1 + n2).ToString();
                    break;
                case "-":
                    Number1 = (n1 - n2).ToString();  
                    break;
                case "*":
                    Number1 = (n1 * n2).ToString();
                    break;
                case "/":
                    Number1 = (n1 / n2).ToString();
                    break;
            }
            Number2 = null;
            Operator= null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Furniture2
{
    public partial class TovarControl : UserControl
    {
        public string Category {  get; set; }
        public string NameTovar { get; set; }
        public string Opisanie { get; set; }
        public string Proizvoditel { get; set; }
        public string Postavshik { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int QuantitySclad { get; set; }
        public int Discount { get; set; }
        public string Picture { get; set; }
        public TovarControl()
        {
            InitializeComponent();
        }
        
        public void SetLabels()
        {
            labelCategory.Text = $"{Category} | {NameTovar}";
            labelOpisanie.Text = $"Описание: {Opisanie}";
            labelProizvoditel.Text = $"Производитель: {Proizvoditel}";
            labelPostavshik.Text = $"Поставщик: {Postavshik}";
            labelPrice.Text = $"Цена: {Price} руб.";
            labelTotalPrice.Text = $"Итог: {TotalPrice} руб.";
            labelQuantitySclad.Text = $"Количество на складе: {QuantitySclad} шт.";
            labelDiscount.Text = $"Скидка: {Discount}%";
            pictureBox1.ImageLocation = Path.Combine(Application.StartupPath, @"../../Properties", Picture);
            if(Discount >= 0)
            {
                labelPrice.Font = new Font("Segoe Print", 8, FontStyle.Strikeout);
                labelPrice.ForeColor = Color.Red;
            }
            if(Discount > 15)
            {
                labelDiscount.BackColor = ColorTranslator.FromHtml("#23E1EF");
            }
            if(QuantitySclad == 0)
            {
                labelQuantitySclad.ForeColor = Color.AliceBlue;
            }
        } 
    }
}

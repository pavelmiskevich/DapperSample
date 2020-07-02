using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DapperExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            #region DataService test
            //List<Category> listCategory = DataService.GetAllCategory();
            //List<Photo> listPhoto = DataService.GetPhotoById(10);
            #endregion DataService test

            var watch = System.Diagnostics.Stopwatch.StartNew();

            cbCategory.DataSource = DataService.GetAllCategoryAsync();

            watch.Stop();
            lbwatch.Text = $"Затраченное время: {watch.ElapsedMilliseconds}";

            cbCategory.DisplayMember = "Name";
            cbCategory.ValueMember = "Id";
            Category obj = cbCategory.SelectedItem as Category;

            if (obj != null)
                dataGridView1.DataSource = DataService.GetPhotoByCategoryId(obj.Id);
        }

        private void cbCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Category obj = cbCategory.SelectedItem as Category;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (obj != null)
                dataGridView1.DataSource = DataService.GetPhotoByCategoryIdAsync(obj.Id);

            watch.Stop();
            lbwatch.Text = $"Затраченное время: {watch.ElapsedMilliseconds}";
        }
    }
}

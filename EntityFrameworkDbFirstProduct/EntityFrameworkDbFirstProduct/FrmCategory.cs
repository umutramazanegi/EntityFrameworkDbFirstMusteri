using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkDbFirstProduct
{
    public partial class FrmCategory : Form
    {
        public FrmCategory()
        {
            InitializeComponent();
        }
        DbFirstProductEntities db = new DbFirstProductEntities();
        void CategoryList()
        {
            dataGridView1.DataSource = db.TblCategory.ToList();
        }
        private void btnList_Click(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            TblCategory tblCategory = new TblCategory();
            tblCategory.CategoryName = txtCategoryName.Text;
            db.TblCategory.Add(tblCategory);
            db.SaveChanges();
            CategoryList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtCategoryId.Text);
            var value = db.TblCategory.Find(id);
            db.TblCategory.Remove(value);
            db.SaveChanges();
            CategoryList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtCategoryId.Text);
            var value = db.TblCategory.Find(id);
            value.CategoryName = txtCategoryName.Text;
            db.SaveChanges();
            CategoryList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtCategoryName.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Arama terimi boşsa tüm listeyi göster
                CategoryList();
                // veya kullanıcıya bir uyarı verilebilir:
                // MessageBox.Show("Lütfen aramak için bir kategori adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Kategori adında arama terimini içeren kayıtları bul (büyük/küçük harf duyarsız)
                var results = db.TblCategory
                                .Where(c => c.CategoryName.Contains(searchTerm))
                                .Select(c => new
                                { // Sadece gerekli sütunları seç
                                    c.CategoryId,
                                    c.CategoryName
                                })
                                .ToList();

                if (results.Any()) // results.Count > 0 ile aynı işlevi görür
                {
                    dataGridView1.DataSource = results;
                }
                else
                {
                    MessageBox.Show("Aradığınız kriterlere uygun kategori bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // İsteğe bağlı: Sonuç bulunamayınca DataGridView'i temizle
                    dataGridView1.DataSource = null;
                    // veya tüm listeyi tekrar göster: CategoryList();
                }
            }
        }

        // İsteğe bağlı: DataGridView'den bir satıra tıklandığında
        // bilgileri TextBox'lara doldurma
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Satır başlığına veya geçersiz bir satıra tıklanmadıysa devam et
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1) // Son satır (boş yeni satır) hariç
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Sütun adlarının doğru olduğundan emin olun ('CategoryId' ve 'CategoryName')
                // Eğer Select ile sütunları yeniden adlandırdıysanız o adları kullanın.
                if (row.Cells["CategoryId"] != null && row.Cells["CategoryId"].Value != null)
                {
                    txtCategoryId.Text = row.Cells["CategoryId"].Value.ToString();
                }
                else
                {
                    txtCategoryId.Text = ""; // Değer yoksa temizle
                }

                if (row.Cells["CategoryName"] != null && row.Cells["CategoryName"].Value != null)
                {
                    txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
                }
                else
                {
                    txtCategoryName.Text = ""; // Değer yoksa temizle
                }
            }
        }

        private void FrmCategory_Load(object sender, EventArgs e)
        {

        }
    }
}

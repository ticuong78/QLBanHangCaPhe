using QLBanHangCaPhe.EntitiesDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanHangCaPhe
{
    public partial class Form1 : Form
    {
        private readonly Image image = new Bitmap(Image.FromFile("C:\\Users\\ADMIN\\source\\repos\\QLBanHangCaPhe\\QLBanHangCaPhe\\Resources\\R.jpg"), new Size(75, 75));
        private int soLuongHD = 0;
        private QLBanHangCP_Model database = new QLBanHangCP_Model();
        private BAN curent_ban = null;
        private Button current_button = null;

        public Form1()
        {
            InitializeComponent();
            load_danhmuc();
            load_mon();

            load_currentban();

            load_datagrid(curent_ban.HOADON);

            soLuongHD = database.HOADON.Count();

            load_button_image();
        }

        private void load_currentban()
        {
            curent_ban = database.BAN.
                First(ban => ban.MABAN == 1);
        }

        private void load_mon()
        {
            cmb_mon.DisplayMember = "TENSANPHAM";
            cmb_mon.ValueMember = "MASANPHAM";
            cmb_mon.SelectedIndex = 0;
        }

        private void load_datagrid(ICollection<HOADON> hoadons)
        {
            // truyền vào danh sách hóa đơn, chọn ra những hóa đơn nào đã lập của ngày hiện hành, các ngày trước có thể xem ở db
            // hoặc làm thêm chức năng lọc theo ngày trong lịch sử
            dataGridView1.Rows.Clear();
            var todayHD = hoadons.Where(hd => hd.NGAYBAN.Value.Date == DateTime.Today.Date).ToList();
            foreach (var hd in todayHD)
                foreach(var cthd in hd.CHITIETHOADON)
                    dataGridView1.Rows.Add(cthd.SANPHAM.TENSANPHAM, cthd.SOLUONG, cthd.SANPHAM.GIABAN, cthd.SOLUONG * cthd.SANPHAM.GIABAN);
        }

        private void load_button_image() //Thay ảnh nền cho các button có hóa đơn trong ngày hôm nay khi mở ứng dụng lần đầu
        {
            //Lấy danh sách các button cần thay ảnh
              var button = panel1.Controls.OfType<Button>();

            //Lấy ra hóa đơn có ngày lập là hôm nay
              var todaysHD = database.HOADON.ToList().Where(hd => hd.NGAYBAN.Value.Date == DateTime.Now.Date);

            var s = todaysHD.Count();

            foreach(Button btn in button)
            {
                if (todaysHD.Any(hd => hd.BAN.MABAN == Convert.ToInt32(btn.Tag)))
                    btn.BackgroundImage = image;
            }
        }

        private void load_danhmuc()
        {
            // load danh mục và cài đặt một số thuộc tính mặc định (đơn giản)
            cmb_danhmuc.DataSource = database.LOAISANPHAM.ToList();
            cmb_danhmuc.SelectedIndex = 0;
            cmb_danhmuc.ValueMember = "MALOAI";
            cmb_danhmuc.DisplayMember = "TENLOAI";
        }

        private void btn_themmon_Click(object sender, EventArgs e)
        {
            // lấy thông tin cần thiết cho từng trường của các lớp liên quan
            string maHD = (++soLuongHD).ToString();
            DateTime ngayban = DateTime.Now;
            int maBan = curent_ban.MABAN;
            int soLuongBan = (int) updown_soluong.Value;


            //lấy từng thông tin cơ bản của CHITIETHOADON
            SANPHAM sp = cmb_mon.SelectedItem as SANPHAM;
            CHITIETHOADON cthd = new CHITIETHOADON();

            cthd.SOLUONG = soLuongBan;
            cthd.MASANPHAM = sp.MASANPHAM;

            cthd.SANPHAM = sp;

            //lấy từng thông tin cơ bản của hóa đơn
            HOADON hd = new HOADON();

            hd.NGAYBAN = ngayban;
            hd.MABAN = maBan;
            hd.MAHOADON = $"HD{soLuongHD:D3}";

            //cập nhật thông tin phức tạp của chi tiết hóa đơn
            cthd.MAHOADON = hd.MAHOADON;
            cthd.HOADON = hd;

            //cập nhật thông tin phức tạp của hóa đơn
            hd.CHITIETHOADON.Add(cthd);
            hd.TONGTIEN = hd.CHITIETHOADON.Sum(ct => ct.SANPHAM.GIABAN * ct.SOLUONG);

            //cập nhật thông tin cho sản phẩm
            sp.CHITIETHOADON.Add(cthd);

            //cập nhật thông tin cho bàn
            if (curent_ban != null)
                curent_ban.HOADON.Add(hd);

            load_datagrid(curent_ban.HOADON);

            if(current_button != null)
                current_button.Image = image;

        }

        private void cmb_danhmuc_onIndexChanged(object sender, EventArgs e)
        {
            // mỗi lần thay đổi danh mục loại sản phẩm, cập nhật loại danh mục món
            string loaispp = ((LOAISANPHAM) cmb_danhmuc.SelectedItem).MALOAI;
            cmb_mon.Refresh();
            cmb_mon.DataSource = database.SANPHAM.Where(sp => sp.MALOAISP.Equals(loaispp)).ToList();
        }

        private void btn_chonban_click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            curent_ban = database.BAN.FirstOrDefault(ban => ban.MABAN == (int) btn.Tag);
            current_button = btn;

            load_datagrid(curent_ban.HOADON);
        }

        private void frm_closing(object sender, FormClosingEventArgs e)
        {
            database.SaveChanges();
        }
    }
}

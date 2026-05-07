using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nesto
{
    public class FormMain : Form
    {

        private TabControl tabControl;

        // Websajtovi tab
        private DataGridView dgvWebsajtovi;
        private TextBox txtWebsajtIme, txtWebsajtOpis;
        private Button btnWebsajtInsert, btnWebsajtUpdate, btnWebsajtDelete, btnWebsajtRefresh;

        // Skrejpovi tab
        private DataGridView dgvSkrejpovi;
        private ComboBox cmbSkrejpWebsajt;
        private TextBox txtSkrejpInfo1, txtSkrejpInfo2, txtSkrejpInfo3;
        private Button btnSkrejpInsert, btnSkrejpUpdate, btnSkrejpDelete, btnSkrejpRefresh;

        // Korisnik tab
        private DataGridView dgvKorisnik;
        private TextBox txtKorisnikIme, txtKorisnikPassword;
        private Button btnKorisnikInsert, btnKorisnikUpdate, btnKorisnikDelete, btnKorisnikRefresh;

        // Utakmice tab
        private DataGridView dgvUtakmice;
        private ComboBox cmbUtakmicaKorisnik, cmbUtakmicaSkrejp;
        private TextBox txtUtakmicaPredikcija;
        private Button btnUtakmicaInsert, btnUtakmicaUpdate, btnUtakmicaDelete, btnUtakmicaRefresh;

        // ── Constructor ───────────────────────────────────────────────
        public FormMain()
        {
            InitializeComponent();
            LoadAll();
        }

        // ── Load all tabs ─────────────────────────────────────────────
        private void LoadAll()
        {
            LoadWebsajtovi();
            LoadSkrejpovi();
            LoadKorisnici();
            LoadUtakmice();
        }

        // ═════════════════════════════════════════════════════════════
        // WEBSAJTOVI
        // ═════════════════════════════════════════════════════════════

        private void LoadWebsajtovi()
        {
            try { dgvWebsajtovi.DataSource = DatabaseHelper.GetWebsajtovi(); }
            catch (Exception ex) { ShowError(ex); }
        }

        private void DgvWebsajtovi_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWebsajtovi.CurrentRow == null) return;
            var row = dgvWebsajtovi.CurrentRow;
            txtWebsajtIme.Text = row.Cells["ime"].Value?.ToString();
            txtWebsajtOpis.Text = row.Cells["opis"].Value?.ToString();
        }

        private void BtnWebsajtInsert_Click(object sender, EventArgs e)
        {
            if (!ValidateFields(txtWebsajtIme, txtWebsajtOpis)) return;
            try
            {
                DatabaseHelper.InsertWebsajt(txtWebsajtIme.Text.Trim(), txtWebsajtOpis.Text.Trim());
                LoadWebsajtovi();
                ClearWebsajtFields();
                ShowSuccess("Websajt dodat.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnWebsajtUpdate_Click(object sender, EventArgs e)
        {
            if (dgvWebsajtovi.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (!ValidateFields(txtWebsajtIme, txtWebsajtOpis)) return;
            try
            {
                int id = (int)dgvWebsajtovi.CurrentRow.Cells["websajt_id"].Value;
                DatabaseHelper.UpdateWebsajt(id, txtWebsajtIme.Text.Trim(), txtWebsajtOpis.Text.Trim());
                LoadWebsajtovi();
                ShowSuccess("Websajt azuriran.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnWebsajtDelete_Click(object sender, EventArgs e)
        {
            if (dgvWebsajtovi.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (!ConfirmDelete()) return;
            try
            {
                int id = (int)dgvWebsajtovi.CurrentRow.Cells["websajt_id"].Value;
                DatabaseHelper.DeleteWebsajt(id);
                LoadWebsajtovi();
                ClearWebsajtFields();
                ShowSuccess("Websajt obrisan.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void ClearWebsajtFields()
        {
            txtWebsajtIme.Clear();
            txtWebsajtOpis.Clear();
        }

        // ═════════════════════════════════════════════════════════════
        // SKREJPOVI
        // ═════════════════════════════════════════════════════════════

        private void LoadSkrejpovi()
        {
            try
            {
                dgvSkrejpovi.DataSource = DatabaseHelper.GetSkrejpovi();
                var ws = DatabaseHelper.GetWebsajtoviForeignKey();
                cmbSkrejpWebsajt.DataSource = ws;
                cmbSkrejpWebsajt.DisplayMember = "ime";
                cmbSkrejpWebsajt.ValueMember = "websajt_id";
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void DgvSkrejpovi_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSkrejpovi.CurrentRow == null) return;
            var row = dgvSkrejpovi.CurrentRow;
            txtSkrejpInfo1.Text = row.Cells["skrejp_informacija_1"].Value?.ToString();
            txtSkrejpInfo2.Text = row.Cells["skrejp_informacija_2"].Value?.ToString();
            txtSkrejpInfo3.Text = row.Cells["skrejp_informacija_3"].Value?.ToString();
            try { cmbSkrejpWebsajt.SelectedValue = row.Cells["websajt_id"].Value; } catch { }
        }

        private void BtnSkrejpInsert_Click(object sender, EventArgs e)
        {
            if (cmbSkrejpWebsajt.SelectedValue == null) { ShowWarning("Izaberi websajt."); return; }
            if (!ValidateFields(txtSkrejpInfo1, txtSkrejpInfo2, txtSkrejpInfo3)) return;
            try
            {
                DatabaseHelper.InsertSkrejp(
                    (int)cmbSkrejpWebsajt.SelectedValue,
                    txtSkrejpInfo1.Text.Trim(),
                    txtSkrejpInfo2.Text.Trim(),
                    txtSkrejpInfo3.Text.Trim());
                LoadSkrejpovi();
                ClearSkrejpFields();
                ShowSuccess("Skrejp dodat.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnSkrejpUpdate_Click(object sender, EventArgs e)
        {
            if (dgvSkrejpovi.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (cmbSkrejpWebsajt.SelectedValue == null) { ShowWarning("Izaberi websajt."); return; }
            if (!ValidateFields(txtSkrejpInfo1, txtSkrejpInfo2, txtSkrejpInfo3)) return;
            try
            {
                int id = (int)dgvSkrejpovi.CurrentRow.Cells["skrejp_id"].Value;
                DatabaseHelper.UpdateSkrejp(id,
                    (int)cmbSkrejpWebsajt.SelectedValue,
                    txtSkrejpInfo1.Text.Trim(),
                    txtSkrejpInfo2.Text.Trim(),
                    txtSkrejpInfo3.Text.Trim());
                LoadSkrejpovi();
                ShowSuccess("Skrejp azuriran.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnSkrejpDelete_Click(object sender, EventArgs e)
        {
            if (dgvSkrejpovi.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (!ConfirmDelete()) return;
            try
            {
                int id = (int)dgvSkrejpovi.CurrentRow.Cells["skrejp_id"].Value;
                DatabaseHelper.DeleteSkrejp(id);
                LoadSkrejpovi();
                ClearSkrejpFields();
                ShowSuccess("Skrejp obrisan.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void ClearSkrejpFields()
        {
            txtSkrejpInfo1.Clear();
            txtSkrejpInfo2.Clear();
            txtSkrejpInfo3.Clear();
        }

        // ═════════════════════════════════════════════════════════════
        // KORISNIK
        // ═════════════════════════════════════════════════════════════

        private void LoadKorisnici()
        {
            try { dgvKorisnik.DataSource = DatabaseHelper.GetKorisnici(); }
            catch (Exception ex) { ShowError(ex); }
        }

        private void DgvKorisnik_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKorisnik.CurrentRow == null) return;
            var row = dgvKorisnik.CurrentRow;
            txtKorisnikIme.Text = row.Cells["ime"].Value?.ToString();
            txtKorisnikPassword.Text = row.Cells["password_hash"].Value?.ToString();
        }

        private void BtnKorisnikInsert_Click(object sender, EventArgs e)
        {
            if (!ValidateFields(txtKorisnikIme, txtKorisnikPassword)) return;
            try
            {
                DatabaseHelper.InsertKorisnik(txtKorisnikIme.Text.Trim(), txtKorisnikPassword.Text.Trim());
                LoadKorisnici();
                ClearKorisnikFields();
                ShowSuccess("Korisnik dodat.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnKorisnikUpdate_Click(object sender, EventArgs e)
        {
            if (dgvKorisnik.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (!ValidateFields(txtKorisnikIme, txtKorisnikPassword)) return;
            try
            {
                int id = (int)dgvKorisnik.CurrentRow.Cells["korisnik_id"].Value;
                DatabaseHelper.UpdateKorisnik(id, txtKorisnikIme.Text.Trim(), txtKorisnikPassword.Text.Trim());
                LoadKorisnici();
                ShowSuccess("Korisnik azuriran.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnKorisnikDelete_Click(object sender, EventArgs e)
        {
            if (dgvKorisnik.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (!ConfirmDelete()) return;
            try
            {
                int id = (int)dgvKorisnik.CurrentRow.Cells["korisnik_id"].Value;
                DatabaseHelper.DeleteKorisnik(id);
                LoadKorisnici();
                ClearKorisnikFields();
                ShowSuccess("Korisnik obrisan.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void ClearKorisnikFields()
        {
            txtKorisnikIme.Clear();
            txtKorisnikPassword.Clear();
        }

        // ═════════════════════════════════════════════════════════════
        // UTAKMICE
        // ═════════════════════════════════════════════════════════════

        private void LoadUtakmice()
        {
            try
            {
                dgvUtakmice.DataSource = DatabaseHelper.GetUtakmice();

                var korisnici = DatabaseHelper.GetKorisniciForeignKey();
                cmbUtakmicaKorisnik.DataSource = korisnici;
                cmbUtakmicaKorisnik.DisplayMember = "ime";
                cmbUtakmicaKorisnik.ValueMember = "korisnik_id";

                var skrejpovi = DatabaseHelper.GetSkrejpoviForeignKey();
                cmbUtakmicaSkrejp.DataSource = skrejpovi;
                cmbUtakmicaSkrejp.DisplayMember = "skrejp_informacija_1";
                cmbUtakmicaSkrejp.ValueMember = "skrejp_id";
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void DgvUtakmice_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUtakmice.CurrentRow == null) return;
            var row = dgvUtakmice.CurrentRow;
            txtUtakmicaPredikcija.Text = row.Cells["predikcija"].Value?.ToString();
            try { cmbUtakmicaKorisnik.SelectedValue = row.Cells["korisnik_id"].Value; } catch { }
            try { cmbUtakmicaSkrejp.SelectedValue = row.Cells["skrejp_id_fk"].Value; } catch { }
        }

        private void BtnUtakmicaInsert_Click(object sender, EventArgs e)
        {
            if (cmbUtakmicaKorisnik.SelectedValue == null || cmbUtakmicaSkrejp.SelectedValue == null)
            { ShowWarning("Izaberi korisnika i skrejp."); return; }
            if (!ValidateFields(txtUtakmicaPredikcija)) return;
            try
            {
                DatabaseHelper.InsertUtakmica(
                    (int)cmbUtakmicaKorisnik.SelectedValue,
                    (int)cmbUtakmicaSkrejp.SelectedValue,
                    txtUtakmicaPredikcija.Text.Trim());
                LoadUtakmice();
                txtUtakmicaPredikcija.Clear();
                ShowSuccess("Utakmica dodata.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnUtakmicaUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUtakmice.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (cmbUtakmicaKorisnik.SelectedValue == null || cmbUtakmicaSkrejp.SelectedValue == null)
            { ShowWarning("Izaberi korisnika i skrejp."); return; }
            if (!ValidateFields(txtUtakmicaPredikcija)) return;
            try
            {
                int id = (int)dgvUtakmice.CurrentRow.Cells["utakmica_id"].Value;
                DatabaseHelper.UpdateUtakmica(id,
                    (int)cmbUtakmicaKorisnik.SelectedValue,
                    (int)cmbUtakmicaSkrejp.SelectedValue,
                    txtUtakmicaPredikcija.Text.Trim());
                LoadUtakmice();
                ShowSuccess("Utakmica azurirana.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BtnUtakmicaDelete_Click(object sender, EventArgs e)
        {
            if (dgvUtakmice.CurrentRow == null) { ShowWarning("Izaberi red."); return; }
            if (!ConfirmDelete()) return;
            try
            {
                int id = (int)dgvUtakmice.CurrentRow.Cells["utakmica_id"].Value;
                DatabaseHelper.DeleteUtakmica(id);
                LoadUtakmice();
                txtUtakmicaPredikcija.Clear();
                ShowSuccess("Utakmica obrisana.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // ═════════════════════════════════════════════════════════════
        // HELPERS
        // ═════════════════════════════════════════════════════════════

        private bool ValidateFields(params TextBox[] fields)
        {
            foreach (var f in fields)
                if (string.IsNullOrWhiteSpace(f.Text))
                { ShowWarning("Popuni sva polja."); f.Focus(); return false; }
            return true;
        }

        private bool ConfirmDelete() =>
            MessageBox.Show("Da li si siguran da zelis da obrises ovaj zapis?", "Potvrda brisanja",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

        private void ShowError(Exception ex) =>
            MessageBox.Show("Greska: " + ex.Message, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowSuccess(string msg) =>
            MessageBox.Show(msg, "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void ShowWarning(string msg) =>
            MessageBox.Show(msg, "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        // ═════════════════════════════════════════════════════════════
        // UI BUILDER (replaces Designer)
        // ═════════════════════════════════════════════════════════════

        private void InitializeComponent()
        {
            this.Text = "Skrejpovanje - Menadzment";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            this.Font = new Font("Segoe UI", 9f);

            tabControl = new TabControl { Dock = DockStyle.Fill };
            this.Controls.Add(tabControl);

            BuildWebsajtTab();
            BuildSkrejpTab();
            BuildKorisnikTab();
            BuildUtakmicaTab();
        }

        // ── Layout helpers ────────────────────────────────────────────

        private (Panel grid, Panel form) BuildTabLayout(TabPage tab)
        {
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350,
                Panel1MinSize = 200,
                Panel2MinSize = 150
            };
            tab.Controls.Add(split);
            return (split.Panel1, split.Panel2);
        }

        private DataGridView BuildGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                RowHeadersVisible = false
            };
        }

        private FlowLayoutPanel BuildButtonBar(params Button[] buttons)
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(4)
            };
            foreach (var b in buttons)
            {
                b.Size = new Size(100, 28);
                panel.Controls.Add(b);
            }
            return panel;
        }

        private static Label Lbl(string text) =>
            new Label { Text = text, AutoSize = true, Margin = new Padding(4, 6, 4, 0) };

        private static TextBox Txt(int width = 220) =>
            new TextBox { Width = width, Margin = new Padding(4, 4, 10, 4) };

        private static ComboBox Cmb(int width = 220) =>
            new ComboBox { Width = width, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(4, 4, 10, 4) };

        private static Button Btn(string text, Color back) =>
            new Button { Text = text, BackColor = back, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, UseVisualStyleBackColor = false };

        // ── WEBSAJTOVI tab ────────────────────────────────────────────

        private void BuildWebsajtTab()
        {
            var tab = new TabPage("Websajtovi");
            tabControl.TabPages.Add(tab);
            var (gridPanel, formPanel) = BuildTabLayout(tab);

            dgvWebsajtovi = BuildGrid();
            dgvWebsajtovi.SelectionChanged += DgvWebsajtovi_SelectionChanged;
            gridPanel.Controls.Add(dgvWebsajtovi);

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            flow.Controls.Add(Lbl("Ime:"));
            txtWebsajtIme = Txt();
            flow.Controls.Add(txtWebsajtIme);
            flow.Controls.Add(Lbl("Opis:"));
            txtWebsajtOpis = Txt(350);
            flow.Controls.Add(txtWebsajtOpis);
            formPanel.Controls.Add(flow);

            btnWebsajtInsert = Btn("Dodaj", Color.FromArgb(46, 139, 87));
            btnWebsajtUpdate = Btn("Azuriraj", Color.FromArgb(70, 130, 180));
            btnWebsajtDelete = Btn("Obrisi", Color.FromArgb(178, 34, 34));
            btnWebsajtRefresh = Btn("Osvezi", Color.FromArgb(100, 100, 100));

            btnWebsajtInsert.Click += BtnWebsajtInsert_Click;
            btnWebsajtUpdate.Click += BtnWebsajtUpdate_Click;
            btnWebsajtDelete.Click += BtnWebsajtDelete_Click;
            btnWebsajtRefresh.Click += (s, e) => LoadWebsajtovi();

            formPanel.Controls.Add(BuildButtonBar(btnWebsajtInsert, btnWebsajtUpdate, btnWebsajtDelete, btnWebsajtRefresh));
        }

        // ── SKREJPOVI tab ─────────────────────────────────────────────

        private void BuildSkrejpTab()
        {
            var tab = new TabPage("Skrejpovi");
            tabControl.TabPages.Add(tab);
            var (gridPanel, formPanel) = BuildTabLayout(tab);

            dgvSkrejpovi = BuildGrid();
            dgvSkrejpovi.SelectionChanged += DgvSkrejpovi_SelectionChanged;
            gridPanel.Controls.Add(dgvSkrejpovi);

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            flow.Controls.Add(Lbl("Websajt:"));
            cmbSkrejpWebsajt = Cmb();
            flow.Controls.Add(cmbSkrejpWebsajt);
            flow.Controls.Add(Lbl("Informacija 1:"));
            txtSkrejpInfo1 = Txt();
            flow.Controls.Add(txtSkrejpInfo1);
            flow.Controls.Add(Lbl("Informacija 2:"));
            txtSkrejpInfo2 = Txt();
            flow.Controls.Add(txtSkrejpInfo2);
            flow.Controls.Add(Lbl("Informacija 3:"));
            txtSkrejpInfo3 = Txt();
            flow.Controls.Add(txtSkrejpInfo3);
            formPanel.Controls.Add(flow);

            btnSkrejpInsert = Btn("Dodaj", Color.FromArgb(46, 139, 87));
            btnSkrejpUpdate = Btn("Azuriraj", Color.FromArgb(70, 130, 180));
            btnSkrejpDelete = Btn("Obrisi", Color.FromArgb(178, 34, 34));
            btnSkrejpRefresh = Btn("Osvezi", Color.FromArgb(100, 100, 100));

            btnSkrejpInsert.Click += BtnSkrejpInsert_Click;
            btnSkrejpUpdate.Click += BtnSkrejpUpdate_Click;
            btnSkrejpDelete.Click += BtnSkrejpDelete_Click;
            btnSkrejpRefresh.Click += (s, e) => LoadSkrejpovi();

            formPanel.Controls.Add(BuildButtonBar(btnSkrejpInsert, btnSkrejpUpdate, btnSkrejpDelete, btnSkrejpRefresh));
        }

        // ── KORISNIK tab ──────────────────────────────────────────────

        private void BuildKorisnikTab()
        {
            var tab = new TabPage("Korisnici");
            tabControl.TabPages.Add(tab);
            var (gridPanel, formPanel) = BuildTabLayout(tab);

            dgvKorisnik = BuildGrid();
            dgvKorisnik.SelectionChanged += DgvKorisnik_SelectionChanged;
            gridPanel.Controls.Add(dgvKorisnik);

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            flow.Controls.Add(Lbl("Ime:"));
            txtKorisnikIme = Txt();
            flow.Controls.Add(txtKorisnikIme);
            flow.Controls.Add(Lbl("Password hash:"));
            txtKorisnikPassword = Txt();
            flow.Controls.Add(txtKorisnikPassword);
            formPanel.Controls.Add(flow);

            btnKorisnikInsert = Btn("Dodaj", Color.FromArgb(46, 139, 87));
            btnKorisnikUpdate = Btn("Azuriraj", Color.FromArgb(70, 130, 180));
            btnKorisnikDelete = Btn("Obrisi", Color.FromArgb(178, 34, 34));
            btnKorisnikRefresh = Btn("Osvezi", Color.FromArgb(100, 100, 100));

            btnKorisnikInsert.Click += BtnKorisnikInsert_Click;
            btnKorisnikUpdate.Click += BtnKorisnikUpdate_Click;
            btnKorisnikDelete.Click += BtnKorisnikDelete_Click;
            btnKorisnikRefresh.Click += (s, e) => LoadKorisnici();

            formPanel.Controls.Add(BuildButtonBar(btnKorisnikInsert, btnKorisnikUpdate, btnKorisnikDelete, btnKorisnikRefresh));
        }

        // ── UTAKMICE tab ──────────────────────────────────────────────

        private void BuildUtakmicaTab()
        {
            var tab = new TabPage("Utakmice");
            tabControl.TabPages.Add(tab);
            var (gridPanel, formPanel) = BuildTabLayout(tab);

            dgvUtakmice = BuildGrid();
            dgvUtakmice.SelectionChanged += DgvUtakmice_SelectionChanged;
            gridPanel.Controls.Add(dgvUtakmice);

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            flow.Controls.Add(Lbl("Korisnik:"));
            cmbUtakmicaKorisnik = Cmb();
            flow.Controls.Add(cmbUtakmicaKorisnik);
            flow.Controls.Add(Lbl("Skrejp:"));
            cmbUtakmicaSkrejp = Cmb(300);
            flow.Controls.Add(cmbUtakmicaSkrejp);
            flow.Controls.Add(Lbl("Predikcija:"));
            txtUtakmicaPredikcija = Txt();
            flow.Controls.Add(txtUtakmicaPredikcija);
            formPanel.Controls.Add(flow);

            btnUtakmicaInsert = Btn("Dodaj", Color.FromArgb(46, 139, 87));
            btnUtakmicaUpdate = Btn("Azuriraj", Color.FromArgb(70, 130, 180));
            btnUtakmicaDelete = Btn("Obrisi", Color.FromArgb(178, 34, 34));
            btnUtakmicaRefresh = Btn("Osvezi", Color.FromArgb(100, 100, 100));

            btnUtakmicaInsert.Click += BtnUtakmicaInsert_Click;
            btnUtakmicaUpdate.Click += BtnUtakmicaUpdate_Click;
            btnUtakmicaDelete.Click += BtnUtakmicaDelete_Click;
            btnUtakmicaRefresh.Click += (s, e) => LoadUtakmice();

            formPanel.Controls.Add(BuildButtonBar(btnUtakmicaInsert, btnUtakmicaUpdate, btnUtakmicaDelete, btnUtakmicaRefresh));
        }
    }
}

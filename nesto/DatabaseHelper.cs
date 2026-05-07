using System;
using System.Data;
using System.Data.SqlClient;

namespace nesto
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
            @"Server=DESKTOP-PRM3K4Q\SQLEXPRESS;Database=skrejpovanje_neko;Integrated Security=True;";

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }


        public static DataTable GetWebsajtovi()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT websajt_id, ime, opis FROM websajtovi", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void InsertWebsajt(string ime, string opis)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("insert_websajt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@opis", opis);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateWebsajt(int id, string ime, string opis)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("update_websajt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@opis", opis);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteWebsajt(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("delete_websajt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static DataTable GetSkrejpovi()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT s.skrejp_id, w.ime AS websajt, s.skrejp_informacija_1, s.skrejp_informacija_2, s.skrejp_informacija_3, s.websajt_id " +
                "FROM skrejpovi s JOIN websajtovi w ON s.websajt_id = w.websajt_id", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void InsertSkrejp(int websajtId, string info1, string info2, string info3)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("insert_skrejp", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@websajt_id", websajtId);
                cmd.Parameters.AddWithValue("@info1", info1);
                cmd.Parameters.AddWithValue("@info2", info2);
                cmd.Parameters.AddWithValue("@info3", info3);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateSkrejp(int id, int websajtId, string info1, string info2, string info3)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("update_skrejp", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@websajt_id", websajtId);
                cmd.Parameters.AddWithValue("@info1", info1);
                cmd.Parameters.AddWithValue("@info2", info2);
                cmd.Parameters.AddWithValue("@info3", info3);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteSkrejp(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("delete_skrejp", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static DataTable GetKorisnici()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT korisnik_id, ime, password_hash FROM korisnik", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void InsertKorisnik(string ime, string passwordHash)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("insert_korisnik", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@password_hash", passwordHash);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateKorisnik(int id, string ime, string passwordHash)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("update_korisnik", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@password_hash", passwordHash);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteKorisnik(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("delete_korisnik", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static DataTable GetUtakmice()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT u.utakmica_id, k.ime AS korisnik, s.skrejp_id, s.skrejp_informacija_1, u.predikcija, u.korisnik_id, u.skrejp_id AS skrejp_id_fk " +
                "FROM utakmice u " +
                "JOIN korisnik k ON u.korisnik_id = k.korisnik_id " +
                "JOIN skrejpovi s ON u.skrejp_id = s.skrejp_id", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void InsertUtakmica(int korisnikId, int skrejpId, string predikcija)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("insert_utakmica", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@korisnik_id", korisnikId);
                cmd.Parameters.AddWithValue("@skrejp_id", skrejpId);
                cmd.Parameters.AddWithValue("@predikcija", predikcija);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateUtakmica(int id, int korisnikId, int skrejpId, string predikcija)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("update_utakmica", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@korisnik_id", korisnikId);
                cmd.Parameters.AddWithValue("@skrejp_id", skrejpId);
                cmd.Parameters.AddWithValue("@predikcija", predikcija);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteUtakmica(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("delete_utakmica", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static DataTable GetWebsajtoviForeignKey()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT websajt_id, ime FROM websajtovi", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetSkrejpoviForeignKey()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT skrejp_id, skrejp_informacija_1 FROM skrejpovi", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetKorisniciForeignKey()
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT korisnik_id, ime FROM korisnik", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}

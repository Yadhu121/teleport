using System.Data;
using System.Data.SqlClient;

namespace teleport.Models
{
    public class FileService
    {
        private readonly String _constr;
        public FileService(IConfiguration config)
        {
            _constr = config.GetConnectionString("DefaultConnection");
        }

        public void FileUpload(FileModel model)
        {
            using var con = new SqlConnection(_constr);
            using var cmd = new SqlCommand("sp_FileUpload", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Token", model.Token);
            cmd.Parameters.AddWithValue("@StoredName", model.StoredName);
            cmd.Parameters.AddWithValue("@OriginalName", model.OriginalName);
            cmd.Parameters.AddWithValue("@IsDownloaded", model.IsDownloaded);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public FileModel GetByToken(string token)
        {
            using var con = new SqlConnection(_constr);
            using var cmd = new SqlCommand("sp_GetFileByToken", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Token", token);

            con.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new FileModel
            {
                Id = (int)reader["Id"],
                Token = reader["Token"].ToString(),
                StoredName = reader["StoredName"].ToString(),
                OriginalName = reader["OriginalName"].ToString(),
                IsDownloaded = (bool)reader["IsDownloaded"]
            };
        }

        public void MarkDownloaded(int id)
        {
            using var con = new SqlConnection(_constr);
            using var cmd = new SqlCommand("sp_MarkDownloaded", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

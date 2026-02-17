using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardWareStore.BL;
using HardWareStore.Interfaces;
using HardWareStore.Models;
using MySql.Data.MySqlClient;

namespace HardWareStore.DL
{
    public class CompanyDL : ICompanyDL
    {
        private readonly DatabaseHelper db = DatabaseHelper.Instance;

        public DataTable GetAllCompanies(string search = "")
        {
            string query = "SELECT * FROM supplier WHERE name LIKE @search";
            var parameters = new[]
            {
                new MySqlParameter("@search", "%" + search + "%")
            };
            return db.ExecuteDataTable(query, parameters);
        }

        public void AddCompany(Company company)
        {
            string query = "INSERT INTO supplier (name, contact, address) VALUES (@name, @contact, @address)";
            var parameters = new[]
            {
                new MySqlParameter("@name", company.CompanyName),
                new MySqlParameter("@contact", company.Contact),
                new MySqlParameter("@address", company.Address)
            };
            db.ExecuteNonQuery(query, parameters);
        }

        public void UpdateCompany(Company company)
        {
            string query = "UPDATE supplier SET name=@name, contact=@contact, address=@address WHERE supplier_id=@id";
            var parameters = new[]
            {
                new MySqlParameter("@id", company.CompanyId),
                new MySqlParameter("@name", company.CompanyName),
                new MySqlParameter("@contact", company.Contact),
                new MySqlParameter("@address", company.Address)
            };
            db.ExecuteNonQuery(query, parameters);
        }

        public int DeleteCompany(int id)
        {
            string query = "DELETE FROM supplier WHERE supplier_id = @id";
            MySqlParameter[] parameters = {
        new MySqlParameter("@id", id)
    };

            try
            {
                return DatabaseHelper.Instance.ExecuteNonQuery(query, parameters);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) // Foreign key violation
                {
                    throw; // Let BL handle the user-friendly message
                }
                else
                {
                    throw; // Re-throw other DB errors
                }
            }
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MobileShopee.Db;
using MobileShopee.Models;

namespace MobileShopee.Repository
{
    public class SaleRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public SaleRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public DataTable GetCustomerByIMEI(string imei)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT c.CustId, c.Cust_Name, c.MobileNumber, c.Email, c.Address " +
                               "FROM tbl_Sales s " +
                               "JOIN tbl_Customer c ON s.CustId = c.CustId " +
                               "WHERE s.IMEINO = @IMEI";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IMEI", imei);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public void AddSale(Sale sale)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Sales (SId, IMEINO, PurchaseDate, Price, CustId) " +
                              "VALUES (@SId, @IMEINO, @PurchaseDate, @Price, @CustId)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SId", sale.SId);
                    cmd.Parameters.AddWithValue("@IMEINO", sale.IMEINO);
                    cmd.Parameters.AddWithValue("@PurchaseDate", sale.PurchaseDate);
                    cmd.Parameters.AddWithValue("@Price", sale.Price);
                    cmd.Parameters.AddWithValue("@CustId", sale.CustId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public (bool success, List<SaleReports> data, string message) GetSaleReportsbyDate(DateTime day)
        {
            var reports = new List<SaleReports>();
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT s.SId, c.CName, mod.ModelNum, s.IMEINO, s.Price " +
                        "FROM tbl_Sales s " +
                        "INNER JOIN tbl_Mobile mob ON mob.IMEINO = s.IMEINO " +
                        "INNER JOIN tbl_Model mod ON mob.ModelId = mod.ModelId " +
                        "INNER JOIN tbl_Company c ON c.CompId = mod.CompId " +
                        "WHERE s.PurchaseDate = @PurchaseDate";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PurchaseDate", day);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reports.Add(new SaleReports
                                {
                                    SaleId = reader["SId"]?.ToString(),
                                    CompanyName = reader["CName"]?.ToString(),
                                    ModelNumber = reader["ModelNum"]?.ToString(),
                                    IMEINO = reader["IMEINO"]?.ToString(),
                                    Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]).ToString("F2") : null
                                });
                            }
                        }
                    }
                }
                return (true, reports, reports.Count > 0 ? "Lấy báo cáo bán hàng thành công" : "Không có dữ liệu bán hàng");
            }
            catch (Exception ex)
            {
                return (false, null, "Lỗi: " + ex.Message);
            }
        }

        public (bool success, List<SaleReports> data, string message) GetSaleReportsbyDtD(DateTime? startDate, DateTime? endDate)
        {
            var reports = new List<SaleReports>();
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT s.SId, c.CName, mod.ModelNum, s.IMEINO, s.Price " +
                        "FROM tbl_Sales s " +
                        "INNER JOIN tbl_Mobile mob ON mob.IMEINO = s.IMEINO " +
                        "INNER JOIN tbl_Model mod ON mob.ModelId = mod.ModelId " +
                        "INNER JOIN tbl_Company c ON c.CompId = mod.CompId " +
                        "WHERE 1 = 1";
                    if (startDate.HasValue)
                    {
                        query += " AND s.PurchaseDate >= @StartDate";
                    }
                    if (endDate.HasValue)
                    {
                        query += " AND s.PurchaseDate <= @EndDate";
                    }
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        if (startDate.HasValue)
                        {
                            command.Parameters.AddWithValue("@StartDate", startDate.Value.Date);
                        }
                        if (endDate.HasValue)
                        {
                            command.Parameters.AddWithValue("@EndDate", endDate.Value.Date.AddDays(1).AddTicks(-1));
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reports.Add(new SaleReports
                                {
                                    SaleId = reader["SId"]?.ToString(),
                                    CompanyName = reader["CName"]?.ToString(),
                                    ModelNumber = reader["ModelNum"]?.ToString(),
                                    IMEINO = reader["IMEINO"]?.ToString(),
                                    Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]).ToString("F2") : null
                                });
                            }
                        }
                    }
                }
                return (true, reports, reports.Count > 0 ? "Lấy báo cáo bán hàng thành công" : "Không có dữ liệu bán hàng");
            }
            catch (Exception ex)
            {
                return (false, null, "Lỗi: " + ex.Message);
            }
        }
    }
}
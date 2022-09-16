using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace Tekno.DashboardAgentService.Common
{
    public static class MSSQLDataConnection
    {
        public static Exception ExecError { get; set; }

        public static List<Dictionary<string, object>> SelectDataFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = dbConn = new SqlConnection();
            dbConn.ConnectionString = dbConnStr;
            SqlCommand dbCmd = null;
            DataTable dt = new DataTable();
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                dt.Load(dbCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                ExecError = ex;
                dt = null;
                dbCmd = null;
            }
            finally
            {
                dbCmd = null;
                dbConn.Close();
                dbConn.Dispose();
            }
            return CommonFunc.GetDictionary(dt);
        }

        public static DataTable SelectDataFromDBDT(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = dbConn = new SqlConnection();
            dbConn.ConnectionString = dbConnStr;
            SqlCommand dbCmd = null;
            DataTable dt = new DataTable();
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                dt.Load(dbCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCmd = null;
                dbConn.Close();
                dbConn.Dispose();
            }
            return dt;
        }

        public static List<String> SelectDataListFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            List<String> list = new List<string>();
            DateTime date = DateTime.Now;
            SqlConnection dbConn = dbConn = new SqlConnection();
            dbConn.ConnectionString = dbConnStr;
            SqlCommand dbCmd = null;
            DataTable dt = new DataTable();
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                dt.Load(dbCmd.ExecuteReader());

                for (int i = 0; i < dt.Rows.Count; i++)
                    list.Add(dt.Rows[i][0].ToString());
            }
            catch (Exception ex)
            {
                ExecError = ex;
                dt = null;
                dbCmd = null;
            }
            finally
            {
                dbCmd = null;
                dbConn.Close();
                dbConn.Dispose();
            }
            return list;
        }

        public static DataSet SelectDataSetFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(Query, dbConn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ExecError = ex;
                ds = null;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return ds;
        }

        public static int SelectIntFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            int sonuc = 0;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                sonuc = int.Parse(dbCmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                ExecError = ex;
                sonuc = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }

        public static Int32 SelectInt32FromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            Int32 sonuc = 0;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                sonuc = Int32.Parse(dbCmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                ExecError = ex;
                sonuc = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }

        public static string SelectStringFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            string sonuc;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                sonuc = dbCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }

        public static int InsertDataToDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            SqlTransaction tran = null;
            int row = -1;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                tran = dbConn.BeginTransaction();
                dbCmd.Transaction = tran;
                row = dbCmd.ExecuteNonQuery();
                tran.Commit();
                if (row == -1)
                    row = 0;
            }
            catch (Exception ex)
            {
                ExecError = ex;
                //tran.Rollback();
                row = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return row;
        }

        public static int UpdateDataToDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            SqlTransaction tran = null;
            int row = -1;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                tran = dbConn.BeginTransaction();
                dbCmd.Transaction = tran;
                row = dbCmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                ExecError = ex;
                tran.Rollback();
                row = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return row;
        }

        public static bool DeleteDataFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            SqlTransaction tran = null;
            bool sonuc = false;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                tran = dbConn.BeginTransaction();
                dbCmd.Transaction = tran;
                dbCmd.ExecuteNonQuery();
                tran.Commit();
                sonuc = true;
            }
            catch (Exception ex)
            {
                ExecError = ex;
                tran.Rollback();
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }

        public static DataTable SPExecute(string Query, SqlParameter[] Parameters, string dbConnStr)
        {
            ExecError = null;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbCmd.CommandType = CommandType.StoredProcedure;

                if (Parameters != null)
                {
                    foreach (SqlParameter param in Parameters)
                        dbCmd.Parameters.Add(param);
                }

                dbConn.Open();

                SqlDataAdapter da = new SqlDataAdapter(dbCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ExecError = ex;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return null;
        }

        public static int ConnectionTest(string dbConnStr)
        {
            ExecError = null;
            SqlConnection dbConn = new SqlConnection(dbConnStr);
            int sonuc = 0;
            try
            {
                dbConn.Open();
            }
            catch (Exception ex)
            {
                ExecError = ex;
                sonuc = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }

        /*
        public static DataTable SelectDataFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date=DateTime.Now;
            SqlConnection dbConn = dbConn = new SqlConnection();
            dbConn.ConnectionString = dbConnStr;
            SqlCommand dbCmd = null;
            DataTable dt = new DataTable();
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                dt.Load(dbCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                dt = null;
                dbCmd = null;
            }
            finally
            {
                dbCmd = null;
                dbConn.Close();
                dbConn.Dispose();
            }
            return dt;
        }
        public static List<String> SelectDataListFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            List<String> list = new List<string>();
            DateTime date = DateTime.Now;
            SqlConnection dbConn = dbConn = new SqlConnection();
            dbConn.ConnectionString = dbConnStr;
            SqlCommand dbCmd = null;
            DataTable dt = new DataTable();
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                dt.Load(dbCmd.ExecuteReader());

                for (int i = 0; i < dt.Rows.Count; i++)
                    list.Add(dt.Rows[i][0].ToString());
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                dt = null;
                dbCmd = null;
            }
            finally
            {
                dbCmd = null;
                dbConn.Close();
                dbConn.Dispose();
            }
            return list;
        }
        public static DataSet SelectDataSetFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(Query, dbConn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                ds = null;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return ds;
        }
        public static int SelectIntFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            int sonuc = 0;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                sonuc = int.Parse(dbCmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                sonuc = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }
        public static Int32 SelectInt32FromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            Int32 sonuc = 0;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                sonuc = Int32.Parse(dbCmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                sonuc = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }
        public static string SelectStringFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            string sonuc;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                sonuc = dbCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                sonuc = "";
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }
        public static int InsertDataToDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            SqlTransaction tran = null;
            int row = -1;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                tran = dbConn.BeginTransaction();
                dbCmd.Transaction = tran;
                row = dbCmd.ExecuteNonQuery();
                tran.Commit();
                if (row == -1)
                    row = 0;
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                tran.Rollback();
                row = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return row;
        }
        public static int UpdateDataToDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            SqlTransaction tran = null;
            int row = -1;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                tran = dbConn.BeginTransaction();
                dbCmd.Transaction = tran;
                row = dbCmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                tran.Rollback();
                row = -1;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return row;
        }
        public static bool DeleteDataFromDB(string Query, string dbConnStr)
        {
            ExecError = null;
            DateTime date = DateTime.Now;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            SqlTransaction tran = null;
            bool sonuc = false;
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbConn.Open();
                tran = dbConn.BeginTransaction();
                dbCmd.Transaction = tran;
                dbCmd.ExecuteNonQuery();
                tran.Commit();
                sonuc = true;
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
                tran.Rollback();
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return sonuc;
        }
        public static DataTable SPExecute(string Query, SqlParameter[] Parameters, string dbConnStr)
        {
            ExecError = null;
            SqlConnection dbConn = null;
            SqlCommand dbCmd = null;
            dbConn = new SqlConnection(dbConnStr);
            try
            {
                dbCmd = new SqlCommand(Query, dbConn);
                dbCmd.CommandType = CommandType.StoredProcedure;

                if (Parameters != null)
                {
                    foreach (SqlParameter param in Parameters)
                        dbCmd.Parameters.Add(param);
                }

                dbConn.Open();

                SqlDataAdapter da = new SqlDataAdapter(dbCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ExecError = ex.Message;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return null;
        }
        */
    }
}
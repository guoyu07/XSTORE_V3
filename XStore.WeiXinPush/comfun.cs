using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace WeiXinPush
{
    public class comfun
    {
        public static string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        public System.Data.DataTable GetDataTable(string SuperSQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(SuperSQL, oracleConnection);
            oracleCommand.CommandTimeout = 9999;
            oracleConnection.Open();
            System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(oracleCommand);
            System.Data.DataSet dataSet = new System.Data.DataSet();
            try
            {

                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message + SuperSQL);
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public static System.Data.DataTable GetDataTableBySQL(string SuperSQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(SuperSQL, oracleConnection);
            oracleCommand.CommandTimeout = 9999;
            oracleConnection.Open();
            System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(oracleCommand);
            System.Data.DataSet dataSet = new System.Data.DataSet();
            try
            {

                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message + SuperSQL);
            }
            finally
            {
                oracleConnection.Close();
            }
        }
        public int Insert(string SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string queryString = SQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);
            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;

        }
        public static int InsertBySQL(string SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string queryString = SQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);
            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;
        }

        public int Update(string SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string queryString = SQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);
            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;
        }

        public static int UpdateBySQL(string SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string queryString = SQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);
            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;
        }

        public int Del(string SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string querystring = SQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(querystring, oracleConnection);
            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;
        }

        public static int DelbySQL(string SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string querystring = SQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(querystring, oracleConnection);
            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;
        }


        /// ''' 取值类函数
        public static string GetStrByInt(string 字段名, string 表名, string 依据字段, int 依据值)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "select Top 1 " + 字段名 + " from " + 表名 + " where  " + 依据字段 + "=" + 依据值 + "";
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            string rowsAffected = "";
            oracleConnection.Open();
            try
            {
                rowsAffected = ((oracleCommand.ExecuteScalar() == DBNull.Value) ? "" : Convert.ToString(oracleCommand.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return (rowsAffected.Trim());
        }
        //获取表集合
        public static System.Data.DataSet GetDataSetBySQL(string SuperSQL)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(SuperSQL, oracleConnection);
            oracleCommand.CommandTimeout = 999;

            System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(oracleCommand);
            System.Data.DataSet dataSet = new System.Data.DataSet();
            try
            {
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public static System.Data.DataTable GetDataTableBySQL2005(string SuperSQL, int PageSize, int PageIndex, string SortField, bool SortType)
        {
            //改造SuperSQL，使其具备分页的功能
            SuperSQL = SuperSQL.ToLower();
            //Dim SuperSQL_tmp = SuperSQL.Substring(0, SuperSQL.IndexOf("from") + 4)
            //Dim SuperSQL_tmp2 = SuperSQL.Substring(SuperSQL.IndexOf("from") + 4)

            if (SortType)
            {
                SuperSQL = SuperSQL.Replace("from", ",ROW_NUMBER() OVER (ORDER BY " + SortField + ") AS RowNo from");
            }
            else
            {
                SuperSQL = SuperSQL.Replace("from", ",ROW_NUMBER() OVER (ORDER BY " + SortField + " DESC) AS RowNo from");
            }
            SuperSQL = "SELECT TOP " + PageSize + " * FROM (" + SuperSQL + ") AS A WHERE RowNo > " + (PageIndex - 1) * PageSize;
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(SuperSQL, oracleConnection);

            System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(oracleCommand);
            System.Data.DataSet dataSet = new System.Data.DataSet();
            try
            {
                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
        }
        public static int GetMaxField(string 表名, string 字段名)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            string queryString = "SELECT MAX(" + 字段名 + ") AS MaxField FROM  " + 表名;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = (oracleCommand.ExecuteScalar() == DBNull.Value) ? 0 : Convert.ToInt32(oracleCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return rowsAffected;
        }

        public static int GetMinField(string 表名, string 字段名)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "SELECT Min(" + 字段名 + ") AS MaxField FROM  " + 表名;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = (oracleCommand.ExecuteScalar() == DBNull.Value) ? 0 : Convert.ToInt32(oracleCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return rowsAffected;
        }

        public static int GetFieldCount(string 表名, string ParaSQL)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "SELECT COUNT(*) AS FieldCount FROM " + 表名 + " where " + ParaSQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = Convert.ToInt32(oracleCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return rowsAffected;
        }

        public static int GetFieldSumByInt(string 表名, string 字段名, string ParaSQL)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "SELECT isnull(sum(" + 字段名 + "),0) AS CountNumber FROM " + 表名 + " where " + ParaSQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = Convert.ToInt32(oracleCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return rowsAffected;
        }

        public static decimal GetFieldSumByDec(string 表名, string 字段名, string ParaSQL)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "SELECT isnull(sum(" + 字段名 + "),0) AS CountNumber FROM " + 表名 + " where " + ParaSQL;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            decimal rowsAffected = default(decimal);
            oracleConnection.Open();
            try
            {
                rowsAffected = Convert.ToDecimal(oracleCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return rowsAffected;
        }

        /// '''  删除类函数
        public static int DelByInt(string 表名, string 依据字段, long 依据值)
        {

            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "DELETE FROM " + 表名 + " WHERE " + 依据字段 + " =" + 依据值;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            int rowsAffected = 0;
            oracleConnection.Open();
            try
            {
                rowsAffected = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// ----------
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public void ExecuteNonQuery(string sql, System.Data.SqlClient.SqlParameter[] paras)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, oracleConnection);
            cmd.Parameters.AddRange(paras);
            cmd.CommandType = CommandType.Text;
            oracleConnection.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            // return dt;
        }

        

        public static int InsertBySQL(List<string> SQL)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand();
            oracleCommand.Connection = oracleConnection;
            int rowsAffected = 0;
            oracleConnection.Open();
            System.Data.SqlClient.SqlTransaction tran = oracleConnection.BeginTransaction();
            try
            {
                foreach (string s in SQL)
                {
                    oracleCommand.Transaction = tran;
                    oracleCommand.CommandText = s;
                    rowsAffected += oracleCommand.ExecuteNonQuery();

                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new NotSupportedException(ex.Message);

            }
            finally
            {
                oracleConnection.Close();
            }
            return rowsAffected;
        }

        public static string GetStrbySQL(string 字段名, string 表名, string 语句Sql)
        {
            System.Data.SqlClient.SqlConnection oracleConnection = new System.Data.SqlClient.SqlConnection(constr);

            string queryString = "select Top 1 " + 字段名 + " from " + 表名 + " where 1=1 and " + 语句Sql;
            System.Data.SqlClient.SqlCommand oracleCommand = new System.Data.SqlClient.SqlCommand(queryString, oracleConnection);

            string rowsAffected = "";
            oracleConnection.Open();
            try
            {
                rowsAffected = ((oracleCommand.ExecuteScalar() == DBNull.Value) ? "" : Convert.ToString(oracleCommand.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return (rowsAffected.Trim());
        }

        #region


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="ps">参数列表</param>
        /// <returns>返回存储过程成功与否</returns>
        public int ExecuteNonQuerySP(string cmdText, params SqlParameter[] ps)
        {
            return this.ExecuteNonQuery(cmdText, CommandType.StoredProcedure, ps);
        }

        /// <summary>
        /// 执行SQL语句或存储过程，返回结果
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名称</param>
        /// <param name="type">执行类型</param>
        /// <param name="ps">参数列表</param>
        /// <returns>返回受影响行数或存储过程成功与否</returns>
        public int ExecuteNonQuery(string cmdText, CommandType type, params SqlParameter[] ps)
        {
            if (type == CommandType.Text)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                        {
                            if (ps != null)
                            {
                                cmd.Parameters.AddRange(ps);
                            }
                            conn.Open();
                            return cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (type == CommandType.StoredProcedure)
            {
                try
                {
                    ps[ps.Length - 1].Direction = ParameterDirection.Output;
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (ps != null)
                            {
                                cmd.Parameters.AddRange(ps);
                            }
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return (int)ps[ps.Length - 1].Value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 执行查询语句，返回第一行第一列数据
        /// </summary>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="ps">参数列表</param>
        /// <returns>第一行第一列数据</returns>
        public object ExecuteScalar(string cmdText, params SqlParameter[] ps)
        {
            return ExecuteScalar(cmdText, CommandType.Text, ps);
        }

        /// <summary>
        /// 执行存储过程，返回存储过程返回值
        /// </summary>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="ps">参数列表</param>
        /// <returns>存储过程返回值</returns>
        public object ExecuteScalarSP(string cmdText, params SqlParameter[] ps)
        {
            return ExecuteScalar(cmdText, CommandType.StoredProcedure, ps);
        }

        /// <summary>
        /// 执行查询语句或存储过程，返回第一行第一列数据或存储过程返回值
        /// </summary>
        /// <param name="cmdText">Sql语句或存储过程名称</param>
        /// <param name="ps">参数列表</param>
        /// <returns>第一行第一列数据或存储过程返回值</returns>
        public object ExecuteScalar(string cmdText, CommandType type, params SqlParameter[] ps)
        {
            if (type == CommandType.Text)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                        {
                            if (ps != null)
                            {
                                cmd.Parameters.AddRange(ps);
                            }
                            conn.Open();
                            return cmd.ExecuteScalar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else if (type == CommandType.StoredProcedure)
            {
                try
                {
                    for (int i = 0; i < ps.Length; i++)
                    {
                        if (ps[i].Value == null)
                        {
                            ps[i].Direction = ParameterDirection.Output;
                        }
                    }
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                        {
                            cmd.CommandType = type;
                            if (ps != null)
                            {
                                cmd.Parameters.AddRange(ps);
                            }
                            conn.Open();
                            cmd.ExecuteScalar();
                        }
                    }
                    List<Object> returnValue = new List<object>();
                    for (int i = 0; i < ps.Length; i++)
                    {
                        if (ps[i].Direction == ParameterDirection.Output)
                        {
                            returnValue.Add(ps[i].Value);
                        }
                    }
                    return returnValue;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 执行查询语句，并返回一个DataReader阅读器
        /// </summary>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="ps">参数列表</param>
        /// <returns>DataReader阅读器</returns>
        public SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] ps)
        {
            return ExecuteReader(cmdText, CommandType.Text, ps);
        }

        /// <summary>
        /// 执行存储过程，并返回一个DataReader阅读器
        /// </summary>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="ps">参数列表</param>
        /// <returns>DataReader阅读器</returns>
        public SqlDataReader ExecuteReaderSP(string cmdText, params SqlParameter[] ps)
        {
            return ExecuteReader(cmdText, CommandType.StoredProcedure, ps);
        }

        /// <summary>
        /// 执行查询语句或存储过程，并返回一个DataReader阅读器
        /// </summary>
        /// <param name="cmdText">Sql语句或存储过程名称</param>
        /// <param name="ps">参数列表</param>
        /// <returns>DataReader阅读器</returns>
        public SqlDataReader ExecuteReader(string cmdText, CommandType type, params SqlParameter[] ps)
        {
            SqlConnection conn = new SqlConnection(constr);
            try
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandType = type;
                    if (ps != null)
                    {
                        cmd.Parameters.AddRange(ps);
                    }
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                conn.Dispose();
                throw (ex);
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="ps">参数列表</param>
        /// <returns>返回的DataSet</returns>
        public DataSet GetDataSet(string cmdText, params SqlParameter[] ps)
        {
            return this.GetDataSet(cmdText, CommandType.Text, ps);
        }

        /// <summary>
        /// 执行存储过程返回DataSet
        /// </summary>
        /// <param name="cmdText">存储过程名</param>
        /// <param name="ps">参数列表</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSetSP(string cmdText, params SqlParameter[] ps)
        {
            return this.GetDataSet(cmdText, CommandType.StoredProcedure, ps);
        }

        /// <summary>
        /// 执行存储过程，或SQL语句，返回DataSet
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名称</param>
        /// <param name="type">执行类型</param>
        /// <param name="ps">参数列表</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string cmdText, CommandType type, params SqlParameter[] ps)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmdText, constr))
                {
                    if (ps != null)
                    {
                        sda.SelectCommand.Parameters.AddRange(ps);
                    }
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                ds.Dispose();
                throw (ex);
            }
        }
        #endregion

    }
}
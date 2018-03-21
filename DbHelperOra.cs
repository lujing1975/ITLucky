using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;//这个是添加的引用
using System.Runtime.InteropServices;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace LuckyIT
{
    /// <summary>
    /// Copyright (C) 2004-2008 LiTianPing 
    /// 数据访问基础类(基于Oracle)
    /// 可以用户可以修改满足自己项目的需要。

    /// </summary>
    public abstract class DbHelperOra
    {
        
       
        public DbHelperOra()
        {
            
        }
       

    


        public static OleDbConnection createConnection()//返回一个连接
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            string strTempPath = System.Windows.Forms.Application.StartupPath;
            string filepath = System.IO.Path.Combine(strTempPath, "2014Dinner.mdb");
            return new OleDbConnection(connectionString + filepath);
        }

        public static DataTable QueryDataTable(string selectSQL, string tableName)//获得一个表,2个参数,一个SQL语句,一个表名
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            string strTempPath = System.Windows.Forms.Application.StartupPath;
            string filepath = System.IO.Path.Combine(strTempPath, "2014Dinner.mdb");
            connectionString = connectionString + filepath; ;
            DataTable table = new DataTable(tableName);
            OleDbConnection oleConnection = createConnection();
            try
            {
                OleDbDataAdapter oleDataAdapter = new OleDbDataAdapter(selectSQL, connectionString);
                oleDataAdapter.Fill(table);
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oleConnection.State != ConnectionState.Closed)
                {
                    oleConnection.Close();
                }
            }
            return table;
        }
        public static int ExecuteSql(string cmdText)//执行SQL语句,1个参数,为所执行的语句,返回整数值为判断所影响的行数
        {
            int iValue = -1;
            OleDbConnection oleConnecion = createConnection();
            oleConnecion.Open();
            OleDbTransaction oleTransaction = oleConnecion.BeginTransaction();
            try
            {
                OleDbCommand oleCommand = new OleDbCommand();
                oleCommand.Connection = oleConnecion;
                oleCommand.CommandText = cmdText;
                oleCommand.Transaction = oleTransaction;
                iValue = oleCommand.ExecuteNonQuery();
                oleTransaction.Commit();
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                oleTransaction.Rollback();
                throw ex;
            }
            catch (System.Exception ex2)
            {
                throw ex2;
            }
            finally
            {
                oleConnecion.Close();
            }
            return iValue;
        }   
    }
}

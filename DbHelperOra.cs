using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;//�������ӵ�����
using System.Runtime.InteropServices;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace LuckyIT
{
    /// <summary>
    /// Copyright (C) 2004-2008 LiTianPing 
    /// ���ݷ��ʻ�����(����Oracle)
    /// �����û������޸������Լ���Ŀ����Ҫ��

    /// </summary>
    public abstract class DbHelperOra
    {
        
       
        public DbHelperOra()
        {
            
        }
       

    


        public static OleDbConnection createConnection()//����һ������
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            string strTempPath = System.Windows.Forms.Application.StartupPath;
            string filepath = System.IO.Path.Combine(strTempPath, "2014Dinner.mdb");
            return new OleDbConnection(connectionString + filepath);
        }

        public static DataTable QueryDataTable(string selectSQL, string tableName)//���һ����,2������,һ��SQL���,һ������
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
        public static int ExecuteSql(string cmdText)//ִ��SQL���,1������,Ϊ��ִ�е����,��������ֵΪ�ж���Ӱ�������
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

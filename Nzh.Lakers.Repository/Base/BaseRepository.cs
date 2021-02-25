using Nzh.Lakers.IRepository.Base;
using Nzh.Lakers.Model;
using Nzh.Lakers.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Lakers.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private DbContext context;

        private SqlSugarClient db;

        private SimpleClient<T> entityDB;

        public DbContext Context
        {
            get { return context; }
            set { context = value; }
        }

        internal SqlSugarClient Db
        {
            get { return db; }
            private set { db = value; }
        }

        internal SimpleClient<T> EntityDB
        {
            get { return entityDB; }
            private set { entityDB = value; }
        }

        public BaseRepository()
        {
            DbContext.Init(BaseDBConfig.ConnectionString);
            context = DbContext.GetDbContext();
            db = context.Db;
            entityDB = context.GetEntityDB<T>(db);
        }

        #region  事务

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTran()
        {
            db.Ado.BeginTran();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran()
        {
            db.Ado.CommitTran();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTran()
        {
            db.Ado.RollbackTran();
        }

        #endregion

        #region    Sql

        /// <summary>
        /// 执行sql获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public List<T> GetListBySql(string Sql)
        {
            return db.SqlQueryable<T>(Sql).ToList();
        }

        /// <summary>
        /// 执行sql获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBySqlAsync(string Sql)
        {
            return await Task.Run(() => db.SqlQueryable<T>(Sql).ToList());
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public List<T> GetListBySql(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.SqlQueryable<T>(Sql).Where(Expression).ToList();
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBySqlAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.SqlQueryable<T>(Sql).Where(Expression).ToList());
        }

        /// <summary>
        /// 执行sql根据条件获取分页
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public Pagination<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.SqlQueryable<T>(Sql).Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 执行sql根据条件获取分页（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.SqlQueryable<T>(Sql).Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        ///  执行sql根据条件获取分页并且排序
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public Pagination<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> Expression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.SqlQueryable<T>(Sql).OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        ///  执行sql根据条件获取分页并且排序（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> Expression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.SqlQueryable<T>(Sql).OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 执行sql根据多条件获取分页
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public Pagination<T> GetPageListBySql(string Sql, List<IConditionalModel> ConditionalList, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.SqlQueryable<T>(Sql).Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 执行sql根据多条件获取分页（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> ConditionalList, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.SqlQueryable<T>(Sql).Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        ///  执行sql根据多条件获取分页并且排序
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public Pagination<T> GetPageListBySql(string Sql, List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.SqlQueryable<T>(Sql).OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        ///  执行sql根据多条件获取分页并且排序（异步）
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.SqlQueryable<T>(Sql).OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 执行sql获取DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql)
        {
            return db.Ado.GetDataTable(Sql);
        }

        /// <summary>
        /// 执行sql获取DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql));
        }

        /// <summary>
        /// 执行sql根据条件获取DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, object Parameters)
        {
            return db.Ado.GetDataTable(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, object Parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据数条件获取DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, params SugarParameter[] Parameters)
        {
            return db.Ado.GetDataTable(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据数条件获取DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, List<SugarParameter> Parameters)
        {
            return db.Ado.GetDataTable(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.GetDataTable(Sql, Expression);
        }

        /// <summary>
        /// 执行sql根据条件获取DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, Expression));
        }

        /// <summary>
        /// 执行sql获取DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql)
        {
            return db.Ado.GetDataSetAll(Sql);
        }

        /// <summary>
        /// 执行sql获取DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql));
        }

        /// <summary>
        /// 执行sql根据条件获取DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, object Parameters)
        {
            return db.Ado.GetDataSetAll(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, object Parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据数条件获取DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, params SugarParameter[] Parameters)
        {
            return db.Ado.GetDataSetAll(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, Parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, List<SugarParameter> Parameters)
        {
            return db.Ado.GetDataSetAll(Sql, Parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.GetDataSetAll(Sql, Expression);
        }

        /// <summary>
        /// 执行sql根据条件获取DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, Expression));
        }

        /// <summary>
        /// 执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql)
        {
            return db.Ado.ExecuteCommand(Sql) > 0 ? true : false;
        }

        /// <summary>
        /// 执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql)) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, object Parameters)
        {
            return db.Ado.ExecuteCommand(Sql, Parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, object Parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, Parameters)) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, params SugarParameter[] Parameters)
        {
            return db.Ado.ExecuteCommand(Sql, Parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, Parameters)) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, List<SugarParameter> Parameters)
        {
            return db.Ado.ExecuteCommand(Sql, Parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, Parameters)) > 0 ? true : false;
        }

        /// <summary>
        ///  根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.ExecuteCommand(Sql, Expression) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, Expression)) > 0 ? true : false;
        }

        /// <summary>
        /// 执行sql获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql)
        {
            return db.Ado.SqlQuery<T>(Sql);
        }

        /// <summary>
        /// 执行sql获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, object Parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, object Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, params SugarParameter[] Parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, List<SugarParameter> Parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.SqlQuery<T>(Sql, Expression);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, Expression));
        }

        /// <summary>
        /// 执行sql获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public T Get(string Sql)
        {
            return db.Ado.SqlQuerySingle<T>(Sql);
        }

        /// <summary>
        /// 执行sql获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql));
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, object Parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, Parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, object Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, Parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, params SugarParameter[] Parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, Parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, List<SugarParameter> Parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public T Get(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, Expression);
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, Expression));
        }

        /// <summary>
        /// 执行sql获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql)
        {
            return db.Ado.SqlQueryDynamic(Sql);
        }

        /// <summary>
        /// 执行sql获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql));
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, object Parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, Parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, object Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, Parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, params SugarParameter[] Parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, Parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, Parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, List<SugarParameter> Parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, Parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, Parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.SqlQueryDynamic(Sql, Expression);
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, Expression));
        }

        #endregion

        #region 其他

        /// <summary>
        /// 查询存储过程获取DataTable
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string ProcedureName)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(ProcedureName);
        }

        /// <summary>
        /// 查询存储过程获取DataTable（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(ProcedureName));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string ProcedureName, object Parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(ProcedureName, Parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, object Parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(ProcedureName, Parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string ProcedureName, params SugarParameter[] Parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(ProcedureName, Parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(ProcedureName, Parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string ProcedureName, List<SugarParameter> Parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(ProcedureName, Parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(ProcedureName, Parameters));
        }

        /// <summary>
        ///  根据条件查询存储过程获取DataTable
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string ProcedureName, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(ProcedureName, Expression);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataTable（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(ProcedureName, Expression));
        }

        /// <summary>
        /// 查询存储过程获取DataSet
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string ProcedureName)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(ProcedureName);
        }

        /// <summary>
        /// 查询存储过程获取DataSet（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(ProcedureName));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string ProcedureName, object Parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(ProcedureName, Parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, object Parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(ProcedureName, Parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string ProcedureName, params SugarParameter[] Parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(ProcedureName, Parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, params SugarParameter[] Parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(ProcedureName, Parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string ProcedureName, List<SugarParameter> Parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(ProcedureName, Parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, List<SugarParameter> Parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(ProcedureName, Parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string ProcedureName, Expression<Func<T, bool>> Expression)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(ProcedureName, Expression);
        }

        /// <summary>
        /// 根据条件查询存储过程获取DataSet（异步）
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(ProcedureName, Expression));
        }

        /// <summary>
        /// 查询前多少条数据
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public List<T> Take(int Num)
        {
            return db.Queryable<T>().With(SqlWith.NoLock).Take(Num).ToList();
        }

        /// <summary>
        /// 查询前多少条数据（异步）
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<List<T>> TakeAsync(int Num)
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Take(Num).ToListAsync());
        }

        /// <summary>
        /// 根据条件查询前多少条数据 
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public List<T> Take(Expression<Func<T, bool>> Expression, int Num)
        {
            return db.Queryable<T>().With(SqlWith.NoLock).Where(Expression).Take(Num).ToList();
        }

        /// <summary>
        /// 根据条件查询前多少条数据 （异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<List<T>> TakeAsync(Expression<Func<T, bool>> Expression, int Num)
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Where(Expression).Take(Num).ToListAsync());
        }

        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            return db.Queryable<T>().With(SqlWith.NoLock).First();
        }

        /// <summary>
        /// 查询单个对象（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<T> FirstAsync()
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).FirstAsync());
        }

        /// <summary>
        /// 根据条件查询单个对象
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().With(SqlWith.NoLock).Where(Expression).First();
        }

        /// <summary>
        /// 根据条件查询单个对象（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Where(Expression).FirstAsync());
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public int Sum(string Field)
        {
            return db.Queryable<T>().Sum<int>(Field);
        }

        /// <summary>
        /// 求和（异步）
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public async Task<int> SumAsync(string Field)
        {
            return await Task.Run(() => db.Queryable<T>().SumAsync<int>(Field));
        }

        /// <summary>
        /// 根据条件求和
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public int Sum(Expression<Func<T, int>> Expression)
        {
            return db.Queryable<T>().Sum(Expression);
        }

        /// <summary>
        /// 根据条件求和（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<int> SumAsync(Expression<Func<T, int>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().SumAsync(Expression));
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public object Max(string Field)
        {
            return db.Queryable<T>().Max<object>(Field);
        }

        /// <summary>
        /// 获取最大值（异步）
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public async Task<object> MaxAsync(string Field)
        {
            return await Task.Run(() => db.Queryable<T>().MaxAsync<object>(Field));
        }

        /// <summary>
        /// 根据条件获取最大值
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public object Max(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Max(Expression);
        }

        /// <summary>
        /// 根据条件获取最大值（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<object> MaxAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().MaxAsync(Expression));
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public object Min(string Field)
        {
            return db.Queryable<T>().Min<object>(Field);
        }

        /// <summary>
        /// 获取最小值（异步）
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public async Task<object> MinAsync(string Field)
        {
            return await Task.Run(() => db.Queryable<T>().MinAsync<object>(Field));
        }

        /// <summary>
        /// 根据条件获取最小值
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public object Min(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Min(Expression);
        }

        /// <summary>
        ///  根据条件获取最小值（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<object> MinAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().MinAsync(Expression));
        }

        /// <summary>
        /// 获取平均值
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public int Avg(string Field)
        {
            return db.Queryable<T>().Avg<int>(Field);
        }

        /// <summary>
        /// 获取平均值（异步）
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        public async Task<int> AvgAsync(string Field)
        {
            return await Task.Run(() => db.Queryable<T>().AvgAsync<int>(Field));
        }

        /// <summary>
        /// 根据条件获取平均值
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public int Avg(Expression<Func<T, int>> Expression)
        {
            return db.Queryable<T>().Avg(Expression);
        }

        /// <summary>
        /// 根据条件获取平均值（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<int> AvgAsync(Expression<Func<T, int>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().AvgAsync(Expression));
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return db.Queryable<T>().Count();
        }

        /// <summary>
        /// 获取数量（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await Task.Run(() => db.Queryable<T>().CountAsync());
        }

        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Where(Expression).Count();
        }

        /// <summary>
        /// 根据条件获取数量（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(Expression).CountAsync());
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsAny()
        {
            return db.Queryable<T>().Any();
        }

        /// <summary>
        /// 判断是否存在（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync()
        {
            return await Task.Run(() => db.Queryable<T>().AnyAsync());
        }

        /// <summary>
        /// 根据条件获取判断是否存在
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public bool IsAny(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Where(Expression).Any();
        }

        /// <summary>
        /// 根据条件获取判断是否存在（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(Expression).AnyAsync());
        }

        #endregion

        #region  查询

        /// <summary>
        ///根据Id获取单个对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetById(dynamic Id)
        {
            return db.Queryable<T>().InSingle(Id);
        }

        /// <summary>
        ///根据Id获取单个对象（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(dynamic Id)
        {
            return await Task.Run(() => db.Queryable<T>().InSingleAsync(Id));
        }

        /// <summary>
        /// 根据主键获取List
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public List<T> GetListByIds(object[] Ids)
        {
            return Db.Queryable<T>().In(Ids).ToList();
        }

        /// <summary>
        /// 根据主键获取List（异步）
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListByIdsAsync(object[] Ids)
        {
            return await Task.Run(() => db.Queryable<T>().In(Ids).ToListAsync());
        }

        /// <summary>
        /// 根据主键获取List
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public List<T> GetListByIds(object[] Ids, Expression<Func<T, bool>> Expression)
        {
            return Db.Queryable<T>().Where(Expression).In(Ids).ToList();
        }

        /// <summary>
        /// 根据主键获取List（异步）
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListByIdsAsync(object[] Ids, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(Expression).In(Ids).ToListAsync());
        }

        /// <summary>
        /// 获取List
        /// </summary>
        /// <returns></returns>
        public List<T> GetList()
        {
            return db.Queryable<T>().ToList();
        }

        /// <summary>
        /// 获取List（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync()
        {
            return await Task.Run(() => db.Queryable<T>().ToListAsync());
        }

        /// <summary>
        /// 根据条件获取List
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Where(Expression).ToList();
        }

        /// <summary>
        /// 根据条件获取List（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(Expression).ToListAsync());
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <returns></returns>
        public T GetSingle()
        {
            return db.Queryable<T>().Single();
        }

        /// <summary>
        /// 获取单个对象（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetSingleAsync()
        {
            return await Task.Run(() => db.Queryable<T>().SingleAsync());
        }

        /// <summary>
        /// 根据条件获取单个对象
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public T GetSingle(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Single(Expression);
        }

        /// <summary>
        /// 根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().SingleAsync(Expression));
        }

        /// <summary>
        /// 获取Json
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            return db.Queryable<T>().ToJson();
        }

        /// <summary>
        ///  获取Json（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetJsonAsync()
        {
            return await Task.Run(() => db.Queryable<T>().ToJsonAsync());
        }

        /// <summary>
        ///  根据条件获取Json
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public string GetJson(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Where(Expression).ToJson();
        }

        /// <summary>
        ///  根据条件获取Json（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<string> GetJsonAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(Expression).ToJsonAsync());
        }

        /// <summary>
        /// 根据条件获取Json分页
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public Pagination<string> GetJsonPage(Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<string> result = new Pagination<string>();
            int Count = 0;
            result.List = db.Queryable<T>().Where(Expression).ToJsonPage(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据条件获取Json分页（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Pagination<string>> GetJsonPageAsync(Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<string> result = new Pagination<string>();
            int Count = 0;
            result.List = await Task.Run(() => db.Queryable<T>().Where(Expression).ToJsonPage(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            return db.Queryable<T>().ToDataTable();
        }

        /// <summary>
        /// 获取DataTable（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableAsync()
        {
            return await Task.Run(() => db.Queryable<T>().ToDataTableAsync());
        }

        /// <summary>
        /// 根据条件获取DataTable
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public DataTable GetDataTable(Expression<Func<T, bool>> Expression)
        {
            return db.Queryable<T>().Where(Expression).ToDataTable();
        }

        /// <summary>
        /// 根据条件获取DataTable（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(Expression).ToDataTableAsync());
        }

        /// <summary>
        /// 根据条件获取DataTable分页
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public Pagination<DataTable> GetDataTablePage(Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<DataTable> result = new Pagination<DataTable>();
            int Count = 0;
            result.List = db.Queryable<T>().Where(Expression).ToDataTablePage(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据条件获取DataTable分页（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Pagination<DataTable>> GetDataTablePageAsync(Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<DataTable> result = new Pagination<DataTable>();
            int Count = 0;
            result.List = await Task.Run(() => db.Queryable<T>().Where(Expression).ToDataTablePage(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public Pagination<T> GetPageList(Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.Queryable<T>().Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListAsync(Expression<Func<T, bool>> Expression, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.Queryable<T>().Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页并排序
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public Pagination<T> GetPageList(Expression<Func<T, bool>> Expression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.Queryable<T>().OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(Expression).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页并排序（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.Queryable<T>().OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(whereExpression).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public Pagination<T> GetPageList(List<IConditionalModel> ConditionalList, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.Queryable<T>().Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页（异步）
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListAsync(List<IConditionalModel> ConditionalList, PageModel Page)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.Queryable<T>().Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页并排序
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public Pagination<T> GetPageList(List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = db.Queryable<T>().OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count);
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页并排序（异步）
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public async Task<Pagination<T>> GetPageListAsync(List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc)
        {
            Pagination<T> result = new Pagination<T>();
            int Count = 0;
            result.List = await Task.Run(() => db.Queryable<T>().OrderByIF(OrderByExpression != null, OrderByExpression, OrderByType).Where(ConditionalList).ToPageList(Page.PageIndex, Page.PageSize, ref Count));
            result.PageIndex = Page.PageIndex;
            result.PageSize = Page.PageSize;
            result.TotalCount = Count;
            return result;
        }

        /// <summary>
        /// 获取Tree
        /// </summary>
        /// <param name="ChildListExpression"></param>
        /// <param name="ParentIdExpression"></param>
        /// <param name="RootValue"></param>
        /// <returns></returns>
        public List<T> GetTreeList(Expression<Func<T, IEnumerable<object>>> ChildListExpression, Expression<Func<T, object>> ParentIdExpression, object RootValue)
        {
            return db.Queryable<T>().ToTree(ChildListExpression, ParentIdExpression,RootValue);
        }

        /// <summary>
        /// 获取Tree（异步）
        /// </summary>
        /// <param name="ChildListExpression"></param>
        /// <param name="ParentIdExpression"></param>
        /// <param name="RootValue"></param>
        /// <returns></returns>
        public async Task<List<T>> GetTreeListAsync(Expression<Func<T, IEnumerable<object>>> ChildListExpression, Expression<Func<T, object>> ParentIdExpression, object RootValue)
        {
            return await Task.Run(() => db.Queryable<T>().ToTreeAsync(ChildListExpression, ParentIdExpression, RootValue));
        }

        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetDictionary(Expression<Func<T, object>> Key, Expression<Func<T, object>> Value)
        {
            return db.Queryable<T>().ToDictionary(Key, Value);
        }

        /// <summary>
        /// 获取字典（异步）
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> GetDictionaryAsync(Expression<Func<T, object>> Key, Expression<Func<T, object>> Value)
        {
            return await Task.Run(() => db.Queryable<T>().ToDictionaryAsync(Key, Value));
        }

        /// <summary>
        /// 获取字典List
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetDictionaryList()
        {
            return db.Queryable<T>().ToDictionaryList();
        }

        /// <summary>
        /// 获取字典List（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, object>>> GetDictionaryListAsync()
        {
            return await Task.Run(() => db.Queryable<T>().ToDictionaryListAsync());
        }

        #endregion

        #region  新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="InsertObj"></param>
        /// <returns></returns>
        public bool Insert(T InsertObj)
        {
            return db.Insertable(InsertObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="InsertObj"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T InsertObj)
        {
            return await Task.Run(() => db.Insertable(InsertObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增获取Id
        /// </summary>
        /// <param name="InsertObj"></param>
        /// <returns></returns>
        public int InsertReturnIdentity(T InsertObj)
        {
            return db.Insertable(InsertObj).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 新增获取Id（异步）
        /// </summary>
        /// <param name="InsertObj"></param>
        /// <returns></returns>
        public async Task<int> InsertReturnIdentityAsync(T InsertObj)
        {
            return await Task.Run(() => db.Insertable(InsertObj).ExecuteReturnIdentityAsync());
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(T[] InsertObjs)
        {
            return db.Insertable(InsertObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(T[] InsertObjs)
        {
            return await Task.Run(() => db.Insertable(InsertObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(List<T>[] InsertObjs)
        {
            return db.Insertable(InsertObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(List<T>[] InsertObjs)
        {
            return await Task.Run(() => db.Insertable(InsertObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(List<T> InsertObjs)
        {
            return db.Insertable(InsertObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(List<T> InsertObjs)
        {
            return await Task.Run(() => db.Insertable(InsertObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增获取单个对象
        /// </summary>
        /// <param name="InsertObj"></param>
        /// <returns></returns>
        public T InsertReturnEntity(T InsertObj)
        {
            return db.Insertable(InsertObj).ExecuteReturnEntity();
        }

        /// <summary>
        /// 新增获取单个对象（异步）
        /// </summary>
        /// <param name="InsertObjs"></param>
        /// <returns></returns>
        public async Task<T> InsertReturnEntityAsync(T InsertObjs)
        {
            return await Task.Run(() => db.Insertable(InsertObjs).ExecuteReturnEntityAsync());
        }

        #endregion

        #region   修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="UpdateObj"></param>
        /// <returns></returns>
        public bool Update(T UpdateObj)
        {
            return db.Updateable(UpdateObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="UpdateObj"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T UpdateObj)
        {
            return await Task.Run(() => db.Updateable(UpdateObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="UpdateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(T[] UpdateObjs)
        {
            return db.Updateable(UpdateObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="UpdateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(T[] UpdateObjs)
        {
            return await Task.Run(() => db.Updateable(UpdateObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="UpdateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(List<T>[] UpdateObjs)
        {
            return db.Updateable(UpdateObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="UpdateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(List<T>[] UpdateObjs)
        {
            return await Task.Run(() => db.Updateable(UpdateObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="UpdateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(List<T> UpdateObjs)
        {
            return db.Updateable(UpdateObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="UpdateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(List<T> UpdateObjs)
        {
            return await Task.Run(() => db.Updateable(UpdateObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件修改列
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        [Obsolete]
        public bool Update(Expression<Func<T, T>> Columns, Expression<Func<T, bool>> Expression)
        {
            return db.Updateable<T>().UpdateColumns(Columns).Where(Expression).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件修改列（异步）
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="Expression"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<bool> UpdateAsync(Expression<Func<T, T>> Columns, Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Updateable<T>().UpdateColumns(Columns).Where(Expression).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, bool>> Expression)
        {
            return db.Updateable<T>().Where(Expression).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件修改（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Updateable<T>().Where(Expression).ExecuteCommandAsync()) > 0 ? true : false;
        }

        #endregion

        #region  删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public bool Delete(T DeleteObj)
        {
            return db.Deleteable<T>().Where(DeleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T DeleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(DeleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public bool Delete(T[] DeleteObj)
        {
            return db.Deleteable<T>(DeleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T[] DeleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(DeleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public bool Delete(List<T>[] DeleteObj)
        {
            return db.Deleteable<T>(DeleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<T>[] DeleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(DeleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public bool Delete(List<T> DeleteObj)
        {
            return db.Deleteable<T>(DeleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<T> DeleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(DeleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> Expression)
        {
            return db.Deleteable<T>().Where(Expression).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件删除（异步）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> Expression)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(Expression).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(dynamic[] Ids)
        {
            return db.Deleteable<T>().In(Ids).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(dynamic[] Ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(Ids).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        ///  批量删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(List<dynamic> Ids)
        {
            return db.Deleteable<T>().In(Ids).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(List<dynamic> Ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(Ids).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteById(long Id)
        {
            return db.Deleteable<T>().In(Id).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据Id删除（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(long Id)
        {
            return await Task.Run(() => db.Deleteable<T>().In(Id).ExecuteCommandAsync()) > 0 ? true : false;
        }

        #endregion
    }
}

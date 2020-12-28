using Nzh.Lakers.IRepository.Base;
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetListBySql(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.SqlQueryable<T>(Sql).Where(expression).ToList();
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBySqlAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.SqlQueryable<T>(Sql).Where(expression).ToList());
        }

        /// <summary>
        /// 执行sql根据条件获取分页
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = db.SqlQueryable<T>(Sql).Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 执行sql根据条件获取分页（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.SqlQueryable<T>(Sql).Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        ///  执行sql根据条件获取分页并且排序
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> expression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = db.SqlQueryable<T>(Sql).OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        ///  执行sql根据条件获取分页并且排序（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> expression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = await Task.Run(() => db.SqlQueryable<T>(Sql).OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 执行sql根据多条件获取分页
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<T> GetPageListBySql(string Sql, List<IConditionalModel> conditionalList, PageModel page)
        {
            int count = 0;
            var result = db.SqlQueryable<T>(Sql).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 执行sql根据多条件获取分页（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> conditionalList, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.SqlQueryable<T>(Sql).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        ///  执行sql根据多条件获取分页并且排序
        /// </summary>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public List<T> GetPageListBySql(string Sql, List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = db.SqlQueryable<T>(Sql).OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        ///  执行sql根据多条件获取分页并且排序（异步）
        /// </summary>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = await Task.Run(() => db.SqlQueryable<T>(Sql).OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 执行sql返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql)
        {
            return db.Ado.GetDataTable(Sql);
        }

        /// <summary>
        /// 执行sql返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql));
        }

        /// <summary>
        /// 执行sql根据条件返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, object parameters)
        {
            return db.Ado.GetDataTable(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据数条件返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.GetDataTable(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据数条件返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.GetDataTable(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.Ado.GetDataTable(Sql, expression);
        }

        /// <summary>
        /// 执行sql根据条件返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.GetDataTableAsync(Sql, expression));
        }

        /// <summary>
        /// 执行sql返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql)
        {
            return db.Ado.GetDataSetAll(Sql);
        }

        /// <summary>
        /// 执行sql返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql));
        }

        /// <summary>
        /// 执行sql根据条件返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, object parameters)
        {
            return db.Ado.GetDataSetAll(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据数条件返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.GetDataSetAll(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据条件返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.GetDataSetAll(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据条件返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.Ado.GetDataSetAll(Sql, expression);
        }

        /// <summary>
        /// 执行sql根据条件返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.GetDataSetAllAsync(Sql, expression));
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, object parameters)
        {
            return db.Ado.ExecuteCommand(Sql, parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, parameters)) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.ExecuteCommand(Sql, parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, parameters)) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.ExecuteCommand(Sql, parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, parameters)) > 0 ? true : false;
        }

        /// <summary>
        ///  根据条件执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.Ado.ExecuteCommand(Sql, expression) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.ExecuteCommandAsync(Sql, expression)) > 0 ? true : false;
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, object parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.Ado.SqlQuery<T>(Sql, expression);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.SqlQueryAsync<T>(Sql, expression));
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, object parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T Get(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, expression);
        }

        /// <summary>
        /// 执行sql根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingleAsync<T>(Sql, expression));
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, object parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public dynamic GetDynamic(string Sql, Expression<Func<T, bool>> expression)
        {
            return db.Ado.SqlQueryDynamic(Sql, expression);
        }

        /// <summary>
        /// 执行sql根据条件获取匿名对象（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        [Obsolete("Use SqlQuery<dynamic>(sql)")]
        public async Task<dynamic> GetDynamicAsync(string Sql, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, expression));
        }

        #endregion

        #region 其他

        /// <summary>
        /// 查询存储过程返回DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string procedureName)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(procedureName);
        }

        /// <summary>
        /// 查询存储过程返回DataTable（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string procedureName)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string procedureName, object parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, object parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string procedureName, params SugarParameter[] parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string procedureName, List<SugarParameter> parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, parameters));
        }

        /// <summary>
        ///  根据条件查询存储过程返回DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataTable QueryDataTableByProcedure(string procedureName, Expression<Func<T, bool>> expression)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(procedureName, expression);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataTable（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, expression));
        }

        /// <summary>
        /// 查询存储过程返回DataSet
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string procedureName)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(procedureName);
        }

        /// <summary>
        /// 查询存储过程返回DataSet（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string procedureName)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string procedureName, object parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(procedureName, parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, object parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName, parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string procedureName, params SugarParameter[] parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(procedureName, parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName, parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string procedureName, List<SugarParameter> parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(procedureName, parameters);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName, parameters));
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataSet QueryDataSetByProcedure(string procedureName, Expression<Func<T, bool>> expression)
        {
            return db.Ado.UseStoredProcedure().GetDataSetAll(procedureName, expression);
        }

        /// <summary>
        /// 根据条件查询存储过程返回DataSet（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName, expression));
        }

        /// <summary>
        /// 查询前多少条数据
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<T> Take(int num)
        {
            return db.Queryable<T>().With(SqlWith.NoLock).Take(num).ToList();
        }

        /// <summary>
        /// 查询前多少条数据（异步）
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task<List<T>> TakeAsync(int num)
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Take(num).ToListAsync());
        }

        /// <summary>
        /// 根据条件查询前多少条数据 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<T> Take(Expression<Func<T, bool>> expression, int num)
        {
            return db.Queryable<T>().With(SqlWith.NoLock).Where(expression).Take(num).ToList();
        }

        /// <summary>
        /// 根据条件查询前多少条数据 （异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task<List<T>> TakeAsync(Expression<Func<T, bool>> expression, int num)
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Where(expression).Take(num).ToListAsync());
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().With(SqlWith.NoLock).Where(expression).First();
        }

        /// <summary>
        /// 根据条件查询单个对象（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Where(expression).FirstAsync());
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public int Sum(string field)
        {
            return db.Queryable<T>().Sum<int>(field);
        }

        /// <summary>
        /// 求和（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<int> SumAsync(string field)
        {
            return await Task.Run(() => db.Queryable<T>().SumAsync<int>(field));
        }

        /// <summary>
        /// 根据条件求和
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int Sum(Expression<Func<T, int>> expression)
        {
            return db.Queryable<T>().Sum(expression);
        }

        /// <summary>
        /// 根据条件求和（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> SumAsync(Expression<Func<T, int>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().SumAsync(expression));
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object Max(string field)
        {
            return db.Queryable<T>().Max<object>(field);
        }

        /// <summary>
        /// 最大值（异步）
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<object> MaxAsync(string field)
        {
            return await Task.Run(() => db.Queryable<T>().MaxAsync<object>(field));
        }

        /// <summary>
        /// 根据条件获取最大值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Max(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Max(expression);
        }

        /// <summary>
        /// 根据条件获取最大值（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<object> MaxAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().MaxAsync(expression));
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object Min(string field)
        {
            return db.Queryable<T>().Min<object>(field);
        }

        /// <summary>
        /// 最小值（异步）
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<object> MinAsync(string field)
        {
            return await Task.Run(() => db.Queryable<T>().MinAsync<object>(field));
        }

        /// <summary>
        /// 根据条件获取最小值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Min(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Min(expression);
        }

        /// <summary>
        ///  根据条件获取最小值（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<object> MinAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().MinAsync(expression));
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public int Avg(string field)
        {
            return db.Queryable<T>().Avg<int>(field);
        }

        /// <summary>
        /// 平均值（异步）
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<int> AvgAsync(string field)
        {
            return await Task.Run(() => db.Queryable<T>().AvgAsync<int>(field));
        }

        /// <summary>
        /// 根据条件获取平均值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int Avg(Expression<Func<T, int>> expression)
        {
            return db.Queryable<T>().Avg(expression);
        }

        /// <summary>
        /// 根据条件获取平均值（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> AvgAsync(Expression<Func<T, int>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().AvgAsync(expression));
        }

        /// <summary>
        /// 返回数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return db.Queryable<T>().Count();
        }

        /// <summary>
        /// 返回数量（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await Task.Run(() => db.Queryable<T>().CountAsync());
        }

        /// <summary>
        /// 根据条件返回数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Where(expression).Count();
        }

        /// <summary>
        /// 根据条件返回数量（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(expression).CountAsync());
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsAny()
        {
            return db.Queryable<T>().Any();
        }

        /// <summary>
        /// 是否存在（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync()
        {
            return await Task.Run(() => db.Queryable<T>().AnyAsync());
        }

        /// <summary>
        /// 根据条件获取判断是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool IsAny(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Where(expression).Any();
        }

        /// <summary>
        /// 根据条件获取判断是否存在（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(expression).AnyAsync());
        }

        #endregion

        #region  查询

        /// <summary>
        ///根据id获取单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(dynamic id)
        {
            return db.Queryable<T>().InSingle(id);
        }

        /// <summary>
        ///根据id获取单个对象（异步）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(dynamic id)
        {
            return await Task.Run(() => db.Queryable<T>().InSingleAsync(id));
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Where(expression).ToList();
        }

        /// <summary>
        /// 根据条件获取List（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(expression).ToListAsync());
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public T GetSingle(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Single(expression);
        }

        /// <summary>
        /// 根据条件获取单个对象（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().SingleAsync(expression));
        }

        /// <summary>
        /// 根据条件获取分页
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<T> GetPageList(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = db.Queryable<T>().Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetJson(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Where(expression).ToJson();
        }

        /// <summary>
        ///  根据条件获取Json（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<string> GetJsonAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(expression).ToJsonAsync());
        }

        /// <summary>
        /// 根据条件获取Json分页
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetJsonPage(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = db.Queryable<T>().Where(expression).ToJsonPage(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据条件获取Json分页（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<string> GetJsonPageAsync(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().Where(expression).ToJsonPage(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataTable GetDataTable(Expression<Func<T, bool>> expression)
        {
            return db.Queryable<T>().Where(expression).ToDataTable();
        }

        /// <summary>
        /// 根据条件获取DataTable（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(expression).ToDataTableAsync());
        }

        /// <summary>
        /// 根据条件获取DataTable分页
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public DataTable GetDataTablePage(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = db.Queryable<T>().Where(expression).ToDataTablePage(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据条件获取DataTable分页（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTablePageAsync(Expression<Func<T, bool>> expression, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().Where(expression).ToDataTablePage(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页并排序
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public List<T> GetPageList(Expression<Func<T, bool>> expression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(expression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页并排序（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页
        /// </summary>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page)
        {
            int count = 0;
            var result = db.Queryable<T>().Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页（异步）
        /// </summary>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页并分页
        /// </summary>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据多条件获取分页并分页（异步）
        /// </summary>
        /// <param name="conditionalList"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 获取Tree
        /// </summary>
        /// <param name="childListExpression"></param>
        /// <param name="parentIdExpression"></param>
        /// <param name="rootValue"></param>
        /// <returns></returns>
        public List<T> GetTreeList(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, object rootValue)
        {
            return db.Queryable<T>().ToTree(childListExpression, parentIdExpression,rootValue);
        }

        /// <summary>
        /// 获取Tree（异步）
        /// </summary>
        /// <param name="childListExpression"></param>
        /// <param name="parentIdExpression"></param>
        /// <param name="rootValue"></param>
        /// <returns></returns>
        public async Task<List<T>> GetTreeListAsync(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, object rootValue)
        {
            return await Task.Run(() => db.Queryable<T>().ToTreeAsync(childListExpression, parentIdExpression, rootValue));
        }

        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetDictionary(Expression<Func<T, object>> key, Expression<Func<T, object>> value)
        {
            return db.Queryable<T>().ToDictionary(key, value);
        }

        /// <summary>
        /// 获取字典（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> GetDictionaryAsync(Expression<Func<T, object>> key, Expression<Func<T, object>> value)
        {
            return await Task.Run(() => db.Queryable<T>().ToDictionaryAsync(key, value));
        }

        /// <summary>
        /// 获取字典List
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> ToDictionaryList()
        {
            return db.Queryable<T>().ToDictionaryList();
        }

        /// <summary>
        /// 获取字典List（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, object>>> ToDictionaryListAsync()
        {
            return await Task.Run(() => db.Queryable<T>().ToDictionaryListAsync());
        }

        #endregion

        #region  新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public bool Insert(T insertObj)
        {
            return db.Insertable(insertObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T insertObj)
        {
            return await Task.Run(() => db.Insertable(insertObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增返回id
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public int InsertReturnIdentity(T insertObj)
        {
            return db.Insertable(insertObj).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 新增返回id（异步）
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public async Task<int> InsertReturnIdentityAsync(T insertObj)
        {
            return await Task.Run(() => db.Insertable(insertObj).ExecuteReturnIdentityAsync());
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(T[] insertObjs)
        {
            return db.Insertable(insertObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(T[] insertObjs)
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(List<T>[] insertObjs)
        {
            return db.Insertable(insertObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(List<T>[] insertObjs)
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(List<T> insertObjs)
        {
            return db.Insertable(insertObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(List<T> insertObjs)
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 新增返回单个对象
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public T InsertReturnEntity(T insertObj)
        {
            return db.Insertable(insertObj).ExecuteReturnEntity();
        }

        /// <summary>
        /// 新增返回单个对象（异步）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<T> InsertReturnEntityAsync(T insertObjs)
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteReturnEntityAsync());
        }

        #endregion

        #region   修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        public bool Update(T updateObj)
        {
            return db.Updateable(updateObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T updateObj)
        {
            return await Task.Run(() => db.Updateable(updateObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(T[] updateObjs)
        {
            return db.Updateable(updateObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(T[] updateObjs)
        {
            return await Task.Run(() => db.Updateable(updateObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(List<T>[] updateObjs)
        {
            return db.Updateable(updateObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(List<T>[] updateObjs)
        {
            return await Task.Run(() => db.Updateable(updateObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(List<T> updateObjs)
        {
            return db.Updateable(updateObjs).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(List<T> updateObjs)
        {
            return await Task.Run(() => db.Updateable(updateObjs).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件更新列
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression)
        {
            return db.Updateable<T>().UpdateColumns(columns).Where(expression).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件更新列（异步）
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Updateable<T>().UpdateColumns(columns).Where(expression).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件更新
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, bool>> expression)
        {
            return db.Updateable<T>().Where(expression).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件更新（异步）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Updateable<T>().Where(expression).ExecuteCommandAsync()) > 0 ? true : false;
        }

        #endregion

        #region  删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(T deleteObj)
        {
            return db.Deleteable<T>().Where(deleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(deleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(T[] deleteObj)
        {
            return db.Deleteable<T>(deleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T[] deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(deleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(List<T>[] deleteObj)
        {
            return db.Deleteable<T>(deleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<T>[] deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(deleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(List<T> deleteObj)
        {
            return db.Deleteable<T>(deleteObj).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<T> deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(deleteObj).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> expression)
        {
            return db.Deleteable<T>().Where(expression).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据条件删除（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(expression).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(dynamic[] ids)
        {
            return db.Deleteable<T>().In(ids).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(dynamic[] ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        ///  批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(List<dynamic> ids)
        {
            return db.Deleteable<T>().In(ids).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(List<dynamic> ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).ExecuteCommandAsync()) > 0 ? true : false;
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(long id)
        {
            return db.Deleteable<T>().In(id).ExecuteCommand() > 0 ? true : false;
        }

        /// <summary>
        /// 根据id删除（异步）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(long id)
        {
            return await Task.Run(() => db.Deleteable<T>().In(id).ExecuteCommandAsync()) > 0 ? true : false;
        }

        #endregion
    }
}

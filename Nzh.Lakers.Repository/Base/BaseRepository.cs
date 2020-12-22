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
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public List<T> GetListBySql(string Sql, Expression<Func<T, bool>> whereExpression)
        {
            return db.SqlQueryable<T>(Sql).Where(whereExpression).ToList();
        }

        // <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBySqlAsync(string Sql, Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.SqlQueryable<T>(Sql).Where(whereExpression).ToList());
        }

        /// <summary>
        /// 执行sql根据条件获取分页
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page)
        {
            int count = 0;
            var result = db.SqlQueryable<T>(Sql).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 执行sql根据条件获取分页（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.SqlQueryable<T>(Sql).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        ///  执行sql根据条件获取分页并且排序
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="whereExpression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = db.SqlQueryable<T>(Sql).OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        ///  执行sql根据条件获取分页并且排序（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="whereExpression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = await Task.Run(() => db.SqlQueryable<T>(Sql).OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count));
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
        /// 执行sql根据参数返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, object parameters)
        {
            return db.Ado.GetDataTable(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据参数返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTable(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据数组参数返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.GetDataTable(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据数组参数返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTable(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据集合参数返回DataTable
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.GetDataTable(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据集合参数返回DataTable（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableBySqlAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.GetDataTable(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据参数返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, object parameters)
        {
            return db.Ado.GetDataSetAll(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据参数返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, object parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAll(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据数组参数返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.GetDataSetAll(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据数组参数返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAll(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据集合参数返回DataSet
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetBySql(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.GetDataSetAll(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据集合参数返回DataSet（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetBySqlAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.GetDataSetAll(Sql, parameters));
        }

        /// <summary>
        /// 根据参数执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, object parameters = null)
        {
            return db.Ado.ExecuteCommand(Sql, parameters) > 0;
        }

        /// <summary>
        /// 根据参数执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, object parameters = null)
        {
            return await Task.Run(() => db.Ado.ExecuteCommand(Sql, parameters) > 0);
        }

        /// <summary>
        /// 根据数组参数执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.ExecuteCommand(Sql, parameters) > 0;
        }

        /// <summary>
        /// 根据数组参数执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommand(Sql, parameters) > 0);
        }

        /// <summary>
        /// 根据集合参数执行Sql
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ExecuteSql(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.ExecuteCommand(Sql, parameters) > 0;
        }

        /// <summary>
        /// 根据集合参数执行Sql（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteSqlAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.ExecuteCommand(Sql, parameters) > 0);
        }

        /// <summary>
        /// 执行sql根据条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, object parameters = null)
        {
            return db.Ado.SqlQuery<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, object parameters = null)
        {
            return await Task.Run(() => db.Ado.SqlQuery<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据数组条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据数组条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuery<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据集合条件获取List
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetList(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQuery<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据集合条件获取List（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuery<T>(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据条件获取实体
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, object parameters = null)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据条件获取实体（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, object parameters = null)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingle<T>(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据数组条件获取实体
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据数组条件获取实体（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingle<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据集合条件获取实体
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Get(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQuerySingle<T>(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据集合条件获取实体（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.SqlQuerySingle<T>(Sql, parameters));
        }

        /// <summary>
        /// 执行sql根据条件获取结果
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic GetDynamic(string Sql, object parameters = null)
        {
            return db.Ado.SqlQueryDynamic(Sql, parameters);
        }

        /// <summary>
        /// 执行sql根据条件获取结果（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<dynamic> GetDynamicAsync(string Sql, object parameters = null)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据数组条件获取结果
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic GetDynamic(string Sql, params SugarParameter[] parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据数组条件获取结果（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<dynamic> GetDynamicAsync(string Sql, params SugarParameter[] parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, parameters));
        }

        /// <summary>
        ///  执行sql根据集合条件获取结果
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic GetDynamic(string Sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQueryDynamic(Sql, parameters);
        }

        /// <summary>
        ///  执行sql根据集合条件获取结果（异步）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<dynamic> GetDynamicAsync(string Sql, List<SugarParameter> parameters)
        {
            return await Task.Run(() => db.Ado.SqlQueryDynamic(Sql, parameters));
        }

        #endregion

        #region 其他

        /// <summary>
        /// 查询存储过程
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable QueryProcedure(string procedureName, List<SugarParameter> parameters)
        {
            var datas = db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
            return datas;
        }

        /// <summary>
        /// 查询存储过程（异步）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryProcedureAsync(string procedureName, List<SugarParameter> parameters)
        {
            var datas = await Task.Run(() => db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters));
            return datas;
        }

        /// <summary>
        /// 查询前多少条数据 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<T> Take(Expression<Func<T, bool>> whereLambda, int num)
        {
            var datas = db.Queryable<T>().With(SqlWith.NoLock).Where(whereLambda).Take(num).ToList();
            return datas;
        }

        /// <summary>
        /// 查询前多少条数据 （异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task<List<T>> TakeAsync(Expression<Func<T, bool>> whereLambda, int num)
        {
            var datas = await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Where(whereLambda).Take(num).ToList());
            return datas;
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> whereLambda)
        {
            var datas = db.Queryable<T>().With(SqlWith.NoLock).Where(whereLambda).First();
            return datas;
        }

        /// <summary>
        /// 查询单条数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> whereLambda)
        {
            var datas = await Task.Run(() => db.Queryable<T>().With(SqlWith.NoLock).Where(whereLambda).First());
            return datas;
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public int Sum(string field)
        {
            var datas = db.Queryable<T>().Sum<int>(field);
            return datas;
        }

        /// <summary>
        /// 求和（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<int> SumAsync(string field)
        {
            var datas = await Task.Run(() => db.Queryable<T>().Sum<int>(field));
            return datas;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public object Max(string field)
        {
            var datas = db.Queryable<T>().Max<object>(field);
            return datas;
        }

        /// <summary>
        /// 最大值（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<object> MaxAsync(string field)
        {
            var datas = await Task.Run(() => db.Queryable<T>().Max<object>(field));
            return datas;
        }


        /// <summary>
        /// 最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public object Min(string field)
        {
            var datas = db.Queryable<T>().Min<object>(field);
            return datas;
        }

        /// <summary>
        /// 最小值（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<object> MinAsync(string field)
        {
            var datas = await Task.Run(() => db.Queryable<T>().Min<object>(field));
            return datas;
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public int Avg(string field)
        {
            var datas = db.Queryable<T>().Avg<int>(field);
            return datas;
        }

        /// <summary>
        /// 平均值（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<int> AvgAsync(string field)
        {
            var datas = await Task.Run(() => db.Queryable<T>().Avg<int>(field));
            return datas;
        }

        /// <summary>
        /// 根据条件返回数量
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> whereExpression)
        {
            return db.Queryable<T>().Where(whereExpression).Count();
        }

        /// <summary>
        /// 根据条件返回数量（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(whereExpression).Count());
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool IsAny(Expression<Func<T, bool>> whereExpression)
        {
            return db.Queryable<T>().Where(whereExpression).Any();
        }

        /// <summary>
        /// 是否存在（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(whereExpression).Any());
        }

        #endregion

        #region  查询

        /// <summary>
        ///根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(dynamic id)
        {
            return db.Queryable<T>().InSingle(id);
        }

        /// <summary>
        ///根据Id获取单个实体（异步）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(dynamic id)
        {
            return await Task.Run(() => db.Queryable<T>().InSingle(id));
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
            return await Task.Run(() => db.Queryable<T>().ToList());
        }

        /// <summary>
        /// 根据条件获取List
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return db.Queryable<T>().Where(whereExpression).ToList();
        }

        /// <summary>
        /// 根据条件获取List（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.Queryable<T>().Where(whereExpression).ToList());
        }

        /// <summary>
        /// 根据条件获取参数或者列
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public T GetSingle(Expression<Func<T, bool>> whereExpression)
        {
            return db.Queryable<T>().Single(whereExpression);
        }

        /// <summary>
        /// 根据条件获取参数或者列（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.Queryable<T>().Single(whereExpression));
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page)
        {
            int count = 0;
            var result = db.Queryable<T>().Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }


        /// <summary>
        /// 获取分页（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel page)
        {
            int count = 0;
            var result = await Task.Run(() => db.Queryable<T>().Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count));
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 根据条件获取分页并排序
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="page"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
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

        #endregion

        #region  新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public bool Insert(T insertObj)
        {
            return db.Insertable(insertObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 新增（异步）
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T insertObj)
        {
            return await Task.Run(() => db.Insertable(insertObj).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 新增返回Id
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public int InsertReturnIdentity(T insertObj)
        {
            return db.Insertable(insertObj).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 新增返回Id（异步）
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public async Task<int> InsertReturnIdentityAsync(T insertObj)
        {
            return await Task.Run(() => db.Insertable(insertObj).ExecuteReturnIdentity());
        }

        /// <summary>
        /// 新增（实体）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(T[] insertObjs)
        {
            return db.Insertable(insertObjs).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 新增（实体）（异步）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(T[] insertObjs)
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 新增（List）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public bool InsertRange(List<T>[] insertObjs)
        {
            return db.Insertable(insertObjs).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 新增（List）（异步）
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<bool> InsertRangeAsync(List<T>[] insertObjs)
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteCommand() > 0);
        }

        #endregion

        #region   修改

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        public bool Update(T updateObj)
        {
            return db.Updateable(updateObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新实体（异步）
        /// </summary>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T updateObj)
        {
            return await Task.Run(() => db.Updateable(updateObj).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 更新多实体
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(T[] updateObjs)
        {
            return db.Updateable(updateObjs).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新多实体（异步）
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(T[] updateObjs)
        {
            return await Task.Run(() => db.Updateable(updateObjs).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public bool UpdateRange(List<T>[] updateObjs)
        {
            return db.Updateable(updateObjs).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新实体（异步）
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(List<T>[] updateObjs)
        {
            return await Task.Run(() => db.Updateable(updateObjs).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 根据条件更新
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression)
        {
            return db.Updateable<T>().UpdateColumns(columns).Where(whereExpression).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据条件更新（异步）
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.Updateable<T>().UpdateColumns(columns).Where(whereExpression).ExecuteCommand() > 0);
        }

        #endregion

        #region  删除

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(T deleteObj)
        {
            return db.Deleteable<T>().Where(deleteObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 删除实体（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(deleteObj).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(T[] deleteObj)
        {
            return db.Deleteable<T>(deleteObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 删除实体（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T[] deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(deleteObj).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public bool Delete(List<T>[] deleteObj)
        {
            return db.Deleteable<T>(deleteObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新实体（异步）
        /// </summary>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<T>[] deleteObj)
        {
            return await Task.Run(() => db.Deleteable<T>(deleteObj).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> whereExpression)
        {
            return db.Deleteable<T>().Where(whereExpression).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据条件删除（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(whereExpression).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(dynamic id)
        {
            return db.Deleteable<T>().In(id).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(dynamic id)
        {
            return await Task.Run(() => db.Deleteable<T>().In(id).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(dynamic[] ids)
        {
            return db.Deleteable<T>().In(ids).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(dynamic[] ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).ExecuteCommand() > 0);
        }

        #endregion

    }
}

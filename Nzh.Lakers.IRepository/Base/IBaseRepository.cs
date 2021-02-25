using Nzh.Lakers.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Lakers.IRepository.Base
{
    public interface IBaseRepository<T> where T : class
    {
        #region  事务

        void BeginTran();

        void CommitTran();

        void RollbackTran();

        #endregion

        #region  Sql

        List<T> GetListBySql(string Sql);

        Task<List<T>> GetListBySqlAsync(string Sql);

        List<T> GetListBySql(string Sql, Expression<Func<T, bool>> Expression);

        Task<List<T>> GetListBySqlAsync(string Sql, Expression<Func<T, bool>> Expression);

        Pagination<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> Expression, PageModel Page);

        Task<Pagination<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> Expression, PageModel Page);

        Pagination<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> Expression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        Task<Pagination<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> Expression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        Pagination<T> GetPageListBySql(string Sql, List<IConditionalModel> ConditionalList, PageModel Page);

        Task<Pagination<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> ConditionalList, PageModel Page);

        Pagination<T> GetPageListBySql(string Sql, List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        Task<Pagination<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        DataTable GetDataTableBySql(string Sql);

        Task<DataTable> GetDataTableBySqlAsync(string Sql);

        DataTable GetDataTableBySql(string Sql, object Parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, object Parameters);

        DataTable GetDataTableBySql(string Sql, params SugarParameter[] Parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, params SugarParameter[] Parameters);

        DataTable GetDataTableBySql(string Sql, List<SugarParameter> Parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, List<SugarParameter> Parameters);

        DataTable GetDataTableBySql(string Sql, Expression<Func<T, bool>> Expression);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, Expression<Func<T, bool>> Expression);

        DataSet GetDataSetBySql(string Sql);

        Task<DataSet> GetDataSetBySqlAsync(string Sql);

        DataSet GetDataSetBySql(string Sql, object Parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, object Parameters);

        DataSet GetDataSetBySql(string Sql, params SugarParameter[] Parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, params SugarParameter[] Parameters);

        DataSet GetDataSetBySql(string Sql, List<SugarParameter> Parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, List<SugarParameter> Parameters);

        DataSet GetDataSetBySql(string Sql, Expression<Func<T, bool>> Expression);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, Expression<Func<T, bool>> Expression);

        bool ExecuteSql(string Sql);

        Task<bool> ExecuteSqlAsync(string Sql);

        bool ExecuteSql(string Sql, object Parameters);

        Task<bool> ExecuteSqlAsync(string Sql, object Parameters);

        bool ExecuteSql(string Sql, params SugarParameter[] Parameters);

        Task<bool> ExecuteSqlAsync(string Sql, params SugarParameter[] Parameters);

        bool ExecuteSql(string Sql, List<SugarParameter> Parameters);

        Task<bool> ExecuteSqlAsync(string Sql, List<SugarParameter> Parameters);

        bool ExecuteSql(string Sql, Expression<Func<T, bool>> Expression);

        Task<bool> ExecuteSqlAsync(string Sql, Expression<Func<T, bool>> Expression);

        List<T> GetList(string Sql);

        Task<List<T>> GetListAsync(string Sql);

        List<T> GetList(string Sql, object Parameters);

        Task<List<T>> GetListAsync(string Sql, object Parameters);

        List<T> GetList(string Sql, params SugarParameter[] Parameters);

        Task<List<T>> GetListAsync(string Sql, params SugarParameter[] Parameters);

        List<T> GetList(string Sql, List<SugarParameter> Parameters);

        Task<List<T>> GetListAsync(string Sql, List<SugarParameter> Parameters);

        List<T> GetList(string Sql, Expression<Func<T, bool>> Expression);

        Task<List<T>> GetListAsync(string Sql, Expression<Func<T, bool>> Expression);

        T Get(string Sql);

        Task<T> GetAsync(string Sql);

        T Get(string Sql, object Parameters);

        Task<T> GetAsync(string Sql, object Parameters);

        T Get(string Sql, params SugarParameter[] Parameters);

        Task<T> GetAsync(string Sql, params SugarParameter[] Parameters);

        T Get(string Sql, List<SugarParameter> Parameters);

        Task<T> GetAsync(string Sql, List<SugarParameter> Parameters);

        T Get(string Sql, Expression<Func<T, bool>> Expression);

        Task<T> GetAsync(string Sql, Expression<Func<T, bool>> Expression);

        dynamic GetDynamic(string Sql);

        Task<dynamic> GetDynamicAsync(string Sql);

        dynamic GetDynamic(string Sql, object Parameters);

        Task<dynamic> GetDynamicAsync(string Sql, object Parameters);

        dynamic GetDynamic(string Sql, params SugarParameter[] Parameters);

        Task<dynamic> GetDynamicAsync(string Sql, params SugarParameter[] Parameters);

        dynamic GetDynamic(string Sql, List<SugarParameter> Parameters);

        Task<dynamic> GetDynamicAsync(string Sql, List<SugarParameter> Parameters);

        dynamic GetDynamic(string Sql, Expression<Func<T, bool>> Expression);

        Task<dynamic> GetDynamicAsync(string Sql, Expression<Func<T, bool>> Expression);

        #endregion

        #region  其他

        DataTable QueryDataTableByProcedure(string ProcedureName);

        Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName);

        DataTable QueryDataTableByProcedure(string ProcedureName, object Parameters);

        Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, object Parameters);

        DataTable QueryDataTableByProcedure(string ProcedureName, params SugarParameter[] Parameters);

        Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, params SugarParameter[] Parameters);

        DataTable QueryDataTableByProcedure(string ProcedureName, List<SugarParameter> Parameters);

        Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, List<SugarParameter> Parameters);

        DataTable QueryDataTableByProcedure(string ProcedureName, Expression<Func<T, bool>> Expression);

        Task<DataTable> QueryDataTableByProcedureAsync(string ProcedureName, Expression<Func<T, bool>> Expression);

        DataSet QueryDataSetByProcedure(string ProcedureName);

        Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName);

        DataSet QueryDataSetByProcedure(string ProcedureName, object Parameters);

        Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, object Parameters);

        DataSet QueryDataSetByProcedure(string ProcedureName, params SugarParameter[] Parameters);

        Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, params SugarParameter[] Parameters);

        DataSet QueryDataSetByProcedure(string ProcedureName, List<SugarParameter> Parameters);

        Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, List<SugarParameter> Parameters);

        DataSet QueryDataSetByProcedure(string ProcedureName, Expression<Func<T, bool>> Expression);

        Task<DataSet> QueryDataSetByProcedureAsync(string ProcedureName, Expression<Func<T, bool>> Expression);

        List<T> Take(int Num);

        Task<List<T>> TakeAsync(int Num);

        List<T> Take(Expression<Func<T, bool>> Expression, int Num);

        Task<List<T>> TakeAsync(Expression<Func<T, bool>> Expression, int Num);

        T First();

        Task<T> FirstAsync();

        T First(Expression<Func<T, bool>> Expression);

        Task<T> FirstAsync(Expression<Func<T, bool>> Expression);

        int Sum(string Field);

        Task<int> SumAsync(string Field);

        int Sum(Expression<Func<T, int>> Expression);

        Task<int> SumAsync(Expression<Func<T, int>> Expression);

        object Max(string Field);

        Task<object> MaxAsync(string Field);

        object Max(Expression<Func<T, bool>> Expression);

        Task<object> MaxAsync(Expression<Func<T, bool>> Expression);

        object Min(string Field);

        Task<object> MinAsync(string Field);

        object Min(Expression<Func<T, bool>> Expression);

        Task<object> MinAsync(Expression<Func<T, bool>> Expression);

        int Avg(string Field);

        Task<int> AvgAsync(string Field);

        int Avg(Expression<Func<T, int>> Expression);

        Task<int> AvgAsync(Expression<Func<T, int>> Expression);

        int Count();

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> Expression);

        int Count(Expression<Func<T, bool>> Expression);

        bool IsAny();

        Task<bool> IsAnyAsync();

        bool IsAny(Expression<Func<T, bool>> Expression);
   
        Task<bool> IsAnyAsync(Expression<Func<T, bool>> Expression);

        #endregion

        #region  查询

        T GetById(dynamic Id);

        Task<T> GetByIdAsync(dynamic Id);

        List<T> GetListByIds(object[] Ids);

        Task<List<T>> GetListByIdsAsync(object[] Ids);

        List<T> GetListByIds(object[] Ids, Expression<Func<T, bool>> Expression);

        Task<List<T>> GetListByIdsAsync(object[] Ids, Expression<Func<T, bool>> Expression);

        List<T> GetList();

        Task<List<T>> GetListAsync();

        List<T> GetList(Expression<Func<T, bool>> Expression);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> Expression);

        T GetSingle();

        Task<T> GetSingleAsync();

        T GetSingle(Expression<Func<T, bool>> Expression);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> Expression);

        string GetJson();

        Task<string> GetJsonAsync();

        string GetJson(Expression<Func<T, bool>> Expression);

        Task<string> GetJsonAsync(Expression<Func<T, bool>> Expression);

        Pagination<string> GetJsonPage(Expression<Func<T, bool>> Expression, PageModel Page);

        Task<Pagination<string>> GetJsonPageAsync(Expression<Func<T, bool>> Expression, PageModel Page);

        DataTable GetDataTable();

        Task<DataTable> GetDataTableAsync();

        DataTable GetDataTable(Expression<Func<T, bool>> Expression);

        Task<DataTable> GetDataTableAsync(Expression<Func<T, bool>> Expression);

        Pagination<DataTable> GetDataTablePage(Expression<Func<T, bool>> Expression, PageModel Page);

        Task<Pagination<DataTable>> GetDataTablePageAsync(Expression<Func<T, bool>> Expression, PageModel Page);

        Pagination<T> GetPageList(Expression<Func<T, bool>> Expression, PageModel Page);

        Task<Pagination<T>> GetPageListAsync(Expression<Func<T, bool>> Expression, PageModel Page);

        Pagination<T> GetPageList(Expression<Func<T, bool>> Expression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        Task<Pagination<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        Pagination<T> GetPageList(List<IConditionalModel> ConditionalList, PageModel Page);

        Task<Pagination<T>> GetPageListAsync(List<IConditionalModel> ConditionalList, PageModel Page);

        Pagination<T> GetPageList(List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        Task<Pagination<T>> GetPageListAsync(List<IConditionalModel> ConditionalList, PageModel Page, Expression<Func<T, object>> OrderByExpression = null, OrderByType OrderByType = OrderByType.Asc);

        List<T> GetTreeList(Expression<Func<T, IEnumerable<object>>> ChildListExpression, Expression<Func<T, object>> ParentIdExpression, object RootValue);

        Task<List<T>> GetTreeListAsync(Expression<Func<T, IEnumerable<object>>> ChildListExpression, Expression<Func<T, object>> ParentIdExpression, object RootValue);

        Dictionary<string, object> GetDictionary(Expression<Func<T, object>> Key, Expression<Func<T, object>> Value);

        Task<Dictionary<string, object>> GetDictionaryAsync(Expression<Func<T, object>> Key, Expression<Func<T, object>> Value);

        List<Dictionary<string, object>> GetDictionaryList();

        Task<List<Dictionary<string, object>>> GetDictionaryListAsync();

        #endregion

        #region  新增

        bool Insert(T InsertObj);

        Task<bool> InsertAsync(T InsertObj);

        int InsertReturnIdentity(T InsertObj);

        Task<int> InsertReturnIdentityAsync(T InsertObj);

        bool InsertRange(T[] InsertObjs);

        Task<bool> InsertRangeAsync(T[] InsertObjs);

        bool InsertRange(List<T>[] InsertObjs);

        Task<bool> InsertRangeAsync(List<T>[] InsertObjs);

        bool InsertRange(List<T> InsertObjs);

        Task<bool> InsertRangeAsync(List<T> InsertObjs);

        T InsertReturnEntity(T InsertObj);

        Task<T> InsertReturnEntityAsync(T InsertObjs);

        #endregion

        #region  修改

        bool Update(T UpdateObj);

        Task<bool> UpdateAsync(T UpdateObj);

        bool UpdateRange(T[] UpdateObjs);

        Task<bool> UpdateRangeAsync(T[] UpdateObjs);

        bool UpdateRange(List<T>[] UpdateObjs);

        Task<bool> UpdateRangeAsync(List<T>[] UpdateObjs);

        bool UpdateRange(List<T> UpdateObjs);

        Task<bool> UpdateRangeAsync(List<T> UpdateObjs);

        bool Update(Expression<Func<T, T>> Columns, Expression<Func<T, bool>> Expression);

        Task<bool> UpdateAsync(Expression<Func<T, T>> Columns, Expression<Func<T, bool>> Expression);

        bool Update(Expression<Func<T, bool>> Expression);

        Task<bool> UpdateAsync(Expression<Func<T, bool>> Expression);

        #endregion

        #region  删除

        bool Delete(T DeleteObj);

        Task<bool> DeleteAsync(T DeleteObj);

        bool Delete(T[] DeleteObj);

        Task<bool> DeleteAsync(T[] DeleteObj);

        bool Delete(List<T>[] DeleteObj);

        Task<bool> DeleteAsync(List<T>[] DeleteObj);

        bool Delete(List<T> DeleteObj);

        Task<bool> DeleteAsync(List<T> DeleteObj);

        bool Delete(Expression<Func<T, bool>> Expression);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> Expression);

        bool DeleteByIds(dynamic[] Ids);

        Task<bool> DeleteByIdsAsync(dynamic[] Ids);

        bool DeleteByIds(List<dynamic> Ids);

        Task<bool> DeleteByIdsAsync(List<dynamic> Ids);

        bool DeleteById(long Id);

        Task<bool> DeleteByIdAsync(long Id);

        #endregion
    }
}

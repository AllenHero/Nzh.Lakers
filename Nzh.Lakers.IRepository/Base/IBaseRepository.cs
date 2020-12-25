﻿using SqlSugar;
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

        List<T> GetListBySql(string Sql, Expression<Func<T, bool>> expression);

        Task<List<T>> GetListBySqlAsync(string Sql, Expression<Func<T, bool>> expression);

        List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> expression, PageModel page);

        Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> expression, PageModel page);

        List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> expression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> expression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        List<T> GetPageListBySql(string Sql, List<IConditionalModel> conditionalList, PageModel page);

        Task<List<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> conditionalList, PageModel page);

        List<T> GetPageListBySql(string Sql, List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        DataTable GetDataTableBySql(string Sql);

        Task<DataTable> GetDataTableBySqlAsync(string Sql);

        DataTable GetDataTableBySql(string Sql, object parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, object parameters);

        DataTable GetDataTableBySql(string Sql, params SugarParameter[] parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, params SugarParameter[] parameters);

        DataTable GetDataTableBySql(string Sql, List<SugarParameter> parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, List<SugarParameter> parameters);

        DataTable GetDataTableBySql(string Sql, Expression<Func<T, bool>> expression);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, Expression<Func<T, bool>> expression);

        DataSet GetDataSetBySql(string Sql);

        Task<DataSet> GetDataSetBySqlAsync(string Sql);

        DataSet GetDataSetBySql(string Sql, object parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, object parameters);

        DataSet GetDataSetBySql(string Sql, params SugarParameter[] parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, params SugarParameter[] parameters);

        DataSet GetDataSetBySql(string Sql, List<SugarParameter> parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, List<SugarParameter> parameters);

        DataSet GetDataSetBySql(string Sql, Expression<Func<T, bool>> expression);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, Expression<Func<T, bool>> expression);

        bool ExecuteSql(string Sql);

        Task<bool> ExecuteSqlAsync(string Sql);

        bool ExecuteSql(string Sql, object parameters);

        Task<bool> ExecuteSqlAsync(string Sql, object parameters);

        bool ExecuteSql(string Sql, params SugarParameter[] parameters);

        Task<bool> ExecuteSqlAsync(string Sql, params SugarParameter[] parameters);

        bool ExecuteSql(string Sql, List<SugarParameter> parameters);

        Task<bool> ExecuteSqlAsync(string Sql, List<SugarParameter> parameters);

        bool ExecuteSql(string Sql, Expression<Func<T, bool>> expression);

        Task<bool> ExecuteSqlAsync(string Sql, Expression<Func<T, bool>> expression);

        List<T> GetList(string Sql);

        Task<List<T>> GetListAsync(string Sql);

        List<T> GetList(string Sql, object parameters);

        Task<List<T>> GetListAsync(string Sql, object parameters);

        List<T> GetList(string Sql, params SugarParameter[] parameters);

        Task<List<T>> GetListAsync(string Sql, params SugarParameter[] parameters);

        List<T> GetList(string Sql, List<SugarParameter> parameters);

        Task<List<T>> GetListAsync(string Sql, List<SugarParameter> parameters);

        List<T> GetList(string Sql, Expression<Func<T, bool>> expression);

        Task<List<T>> GetListAsync(string Sql, Expression<Func<T, bool>> expression);

        T Get(string Sql);

        Task<T> GetAsync(string Sql);

        T Get(string Sql, object parameters);

        Task<T> GetAsync(string Sql, object parameters);

        T Get(string Sql, params SugarParameter[] parameters);

        Task<T> GetAsync(string Sql, params SugarParameter[] parameters);

        T Get(string Sql, List<SugarParameter> parameters);

        Task<T> GetAsync(string Sql, List<SugarParameter> parameters);

        T Get(string Sql, Expression<Func<T, bool>> expression);

        Task<T> GetAsync(string Sql, Expression<Func<T, bool>> expression);

        dynamic GetDynamic(string Sql);

        Task<dynamic> GetDynamicAsync(string Sql);

        dynamic GetDynamic(string Sql, object parameters);

        Task<dynamic> GetDynamicAsync(string Sql, object parameters);

        dynamic GetDynamic(string Sql, params SugarParameter[] parameters);

        Task<dynamic> GetDynamicAsync(string Sql, params SugarParameter[] parameters);

        dynamic GetDynamic(string Sql, List<SugarParameter> parameters);

        Task<dynamic> GetDynamicAsync(string Sql, List<SugarParameter> parameters);

        dynamic GetDynamic(string Sql, Expression<Func<T, bool>> expression);

        Task<dynamic> GetDynamicAsync(string Sql, Expression<Func<T, bool>> expression);

        #endregion

        #region  其他

        DataTable QueryDataTableByProcedure(string procedureName);

        Task<DataTable> QueryDataTableByProcedureAsync(string procedureName);

        DataTable QueryDataTableByProcedure(string procedureName, object parameters);

        Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, object parameters);

        DataTable QueryDataTableByProcedure(string procedureName, params SugarParameter[] parameters);

        Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, params SugarParameter[] parameters);

        DataTable QueryDataTableByProcedure(string procedureName, List<SugarParameter> parameters);

        Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, List<SugarParameter> parameters);

        DataTable QueryDataTableByProcedure(string procedureName, Expression<Func<T, bool>> expression);

        Task<DataTable> QueryDataTableByProcedureAsync(string procedureName, Expression<Func<T, bool>> expression);

        DataSet QueryDataSetByProcedure(string procedureName);

        Task<DataSet> QueryDataSetByProcedureAsync(string procedureName);

        DataSet QueryDataSetByProcedure(string procedureName, object parameters);

        Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, object parameters);

        DataSet QueryDataSetByProcedure(string procedureName, params SugarParameter[] parameters);

        Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, params SugarParameter[] parameters);

        DataSet QueryDataSetByProcedure(string procedureName, List<SugarParameter> parameters);

        Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, List<SugarParameter> parameters);

        DataSet QueryDataSetByProcedure(string procedureName, Expression<Func<T, bool>> expression);

        Task<DataSet> QueryDataSetByProcedureAsync(string procedureName, Expression<Func<T, bool>> expression);

        List<T> Take(int num);

        Task<List<T>> TakeAsync(int num);

        List<T> Take(Expression<Func<T, bool>> expression, int num);

        Task<List<T>> TakeAsync(Expression<Func<T, bool>> expression, int num);

        T First();

        Task<T> FirstAsync();

        T First(Expression<Func<T, bool>> expression);

        Task<T> FirstAsync(Expression<Func<T, bool>> expression);

        int Sum(string field);

        Task<int> SumAsync(string field);

        int Sum(Expression<Func<T, int>> expression);

        Task<int> SumAsync(Expression<Func<T, int>> expression);

        object Max(string field);

        Task<object> MaxAsync(string field);

        object Max(Expression<Func<T, bool>> expression);

        Task<object> MaxAsync(Expression<Func<T, bool>> expression);

        object Min(string field);

        Task<object> MinAsync(string field);

        object Min(Expression<Func<T, bool>> expression);

        Task<object> MinAsync(Expression<Func<T, bool>> expression);

        int Avg(string field);

        Task<int> AvgAsync(string field);

        int Avg(Expression<Func<T, int>> expression);

        Task<int> AvgAsync(Expression<Func<T, int>> expression);

        int Count();

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> expression);

        int Count(Expression<Func<T, bool>> expression);

        bool IsAny();

        Task<bool> IsAnyAsync();

        bool IsAny(Expression<Func<T, bool>> expression);
   
        Task<bool> IsAnyAsync(Expression<Func<T, bool>> expression);

        #endregion

        #region  查询

        T GetById(dynamic id);

        Task<T> GetByIdAsync(dynamic id);

        List<T> GetList();

        Task<List<T>> GetListAsync();

        List<T> GetList(Expression<Func<T, bool>> expression);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression);

        T GetSingle();

        Task<T> GetSingleAsync();

        T GetSingle(Expression<Func<T, bool>> expression);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression);

        List<T> GetPageList(Expression<Func<T, bool>> expression, PageModel page);

        Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> expression, PageModel page);

        string GetJson();

        Task<string> GetJsonAsync();

        string GetJson(Expression<Func<T, bool>> expression);

        Task<string> GetJsonAsync(Expression<Func<T, bool>> expression);

        string GetJsonPage(Expression<Func<T, bool>> expression, PageModel page);

        Task<string> GetJsonPageAsync(Expression<Func<T, bool>> expression, PageModel page);

        DataTable GetDataTable();

        Task<DataTable> GetDataTableAsync();

        DataTable GetDataTable(Expression<Func<T, bool>> expression);

        Task<DataTable> GetDataTableAsync(Expression<Func<T, bool>> expression);

        DataTable GetDataTablePage(Expression<Func<T, bool>> expression, PageModel page);

        Task<DataTable> GetDataTablePageAsync(Expression<Func<T, bool>> expression, PageModel page);

        List<T> GetPageList(Expression<Func<T, bool>> expression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page);

        Task<List<T>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page);

        List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        List<T> GetTreeList(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, object rootValue);

        Task<List<T>> GetTreeListAsync(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, object rootValue);

        Dictionary<string, object> GetDictionary(Expression<Func<T, object>> key, Expression<Func<T, object>> value);

        Task<Dictionary<string, object>> GetDictionaryAsync(Expression<Func<T, object>> key, Expression<Func<T, object>> value);

        List<Dictionary<string, object>> ToDictionaryList();

        Task<List<Dictionary<string, object>>> ToDictionaryListAsync();

        #endregion

        #region  新增

        bool Insert(T insertObj);

        Task<bool> InsertAsync(T insertObj);

        int InsertReturnIdentity(T insertObj);

        Task<int> InsertReturnIdentityAsync(T insertObj);

        bool InsertRange(T[] insertObjs);

        Task<bool> InsertRangeAsync(T[] insertObjs);

        bool InsertRange(List<T>[] insertObjs);

        Task<bool> InsertRangeAsync(List<T>[] insertObjs);

        bool InsertRange(List<T> insertObjs);

        Task<bool> InsertRangeAsync(List<T> insertObjs);

        T InsertReturnEntity(T insertObj);

        Task<T> InsertReturnEntityAsync(T insertObjs);

        #endregion

        #region  修改

        bool Update(T updateObj);

        Task<bool> UpdateAsync(T updateObj);

        bool UpdateRange(T[] updateObjs);

        Task<bool> UpdateRangeAsync(T[] updateObjs);

        bool UpdateRange(List<T>[] updateObjs);

        Task<bool> UpdateRangeAsync(List<T>[] updateObjs);

        bool UpdateRange(List<T> updateObjs);

        Task<bool> UpdateRangeAsync(List<T> updateObjs);

        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression);

        Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression);

        bool Update(Expression<Func<T, bool>> expression);

        Task<bool> UpdateAsync(Expression<Func<T, bool>> expression);

        #endregion

        #region  删除

        bool Delete(T deleteObj);

        Task<bool> DeleteAsync(T deleteObj);

        bool Delete(T[] deleteObj);

        Task<bool> DeleteAsync(T[] deleteObj);

        bool Delete(List<T>[] deleteObj);

        Task<bool> DeleteAsync(List<T>[] deleteObj);

        bool Delete(List<T> deleteObj);

        Task<bool> DeleteAsync(List<T> deleteObj);

        bool Delete(Expression<Func<T, bool>> expression);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);

        bool DeleteByIds(dynamic[] ids);

        Task<bool> DeleteByIdsAsync(dynamic[] ids);

        bool DeleteByIds(List<dynamic> ids);

        Task<bool> DeleteByIdsAsync(List<dynamic> ids);

        bool DeleteById(long id);

        Task<bool> DeleteByIdAsync(long id);

        #endregion
    }
}

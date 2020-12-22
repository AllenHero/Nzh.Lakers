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
        void BeginTran();

        void CommitTran();

        void RollbackTran();

        List<T> GetListBySql(string Sql);

        Task<List<T>> GetListBySqlAsync(string Sql);

        List<T> GetListBySql(string Sql, Expression<Func<T, bool>> whereExpression);

        Task<List<T>> GetListBySqlAsync(string Sql, Expression<Func<T, bool>> whereExpression);

        List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page);

        Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page);

        List<T> GetPageListBySql(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListBySqlAsync(string Sql, Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        List<T> GetPageListBySql(string Sql, List<IConditionalModel> conditionalList, PageModel page);

        Task<List<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> conditionalList, PageModel page);

        List<T> GetPageListBySql(string Sql, List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListBySqlAsync(string Sql, List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        DataTable GetDataTableBySql(string Sql, object parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, object parameters);

        DataTable GetDataTableBySql(string Sql, params SugarParameter[] parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, params SugarParameter[] parameters);

        DataTable GetDataTableBySql(string Sql, List<SugarParameter> parameters);

        Task<DataTable> GetDataTableBySqlAsync(string Sql, List<SugarParameter> parameters);

        DataSet GetDataSetBySql(string Sql, object parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, object parameters);

        DataSet GetDataSetBySql(string Sql, params SugarParameter[] parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, params SugarParameter[] parameters);

        DataSet GetDataSetBySql(string Sql, List<SugarParameter> parameters);

        Task<DataSet> GetDataSetBySqlAsync(string Sql, List<SugarParameter> parameters);

        bool ExecuteSql(string Sql, object parameters = null);

        Task<bool> ExecuteSqlAsync(string Sql, object parameters = null);

        bool ExecuteSql(string Sql, params SugarParameter[] parameters);

        Task<bool> ExecuteSqlAsync(string Sql, params SugarParameter[] parameters);

        bool ExecuteSql(string Sql, List<SugarParameter> parameters);

        Task<bool> ExecuteSqlAsync(string Sql, List<SugarParameter> parameters);

        List<T> GetList(string Sql, object parameters = null);

        Task<List<T>> GetListAsync(string Sql, object parameters = null);

        List<T> GetList(string Sql, params SugarParameter[] parameters);

        Task<List<T>> GetListAsync(string Sql, params SugarParameter[] parameters);

        List<T> GetList(string Sql, List<SugarParameter> parameters);

        Task<List<T>> GetListAsync(string Sql, List<SugarParameter> parameters);

        T Get(string Sql, object parameters = null);

        Task<T> GetAsync(string Sql, object parameters = null);

        T Get(string Sql, params SugarParameter[] parameters);

        Task<T> GetAsync(string Sql, params SugarParameter[] parameters);

        T Get(string Sql, List<SugarParameter> parameters);

        Task<T> GetAsync(string Sql, List<SugarParameter> parameters);

        dynamic GetDynamic(string Sql, object parameters = null);

        Task<dynamic> GetDynamicAsync(string Sql, object parameters = null);

        dynamic GetDynamic(string Sql, params SugarParameter[] parameters);

        Task<dynamic> GetDynamicAsync(string Sql, params SugarParameter[] parameters);

        dynamic GetDynamic(string Sql, List<SugarParameter> parameters);

        Task<dynamic> GetDynamicAsync(string Sql, List<SugarParameter> parameters);

        DataTable QueryProcedure(string procedureName, List<SugarParameter> parameters);

        Task<DataTable> QueryProcedureAsync(string procedureName, List<SugarParameter> parameters);

        List<T> Take(Expression<Func<T, bool>> whereLambda, int num);

        Task<List<T>> TakeAsync(Expression<Func<T, bool>> whereLambda, int num);

        T First(Expression<Func<T, bool>> whereLambda);

        Task<T> FirstAsync(Expression<Func<T, bool>> whereLambda);

        int Sum(string field);

        Task<int> SumAsync(string field);

        object Max(string field);

        Task<object> MaxAsync(string field);

        object Min(string field);

        Task<object> MinAsync(string field);

        int Avg(string field);

        Task<int> AvgAsync(string field);

        int Count(Expression<Func<T, bool>> whereExpression);

        Task<int> CountAsync(Expression<Func<T, bool>> whereExpression);

        bool IsAny(Expression<Func<T, bool>> whereExpression);

        Task<bool> IsAnyAsync(Expression<Func<T, bool>> whereExpression);

        T GetById(dynamic id);

        Task<T> GetByIdAsync(dynamic id);

        List<T> GetList();

        Task<List<T>> GetListAsync();

        List<T> GetList(Expression<Func<T, bool>> whereExpression);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression);

        T GetSingle(Expression<Func<T, bool>> whereExpression);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> whereExpression);

        List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page);

        Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel page);

        List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page);

        Task<List<T>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page);

        List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<List<T>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);

        bool Insert(T insertObj);

        Task<bool> InsertAsync(T insertObj);

        int InsertReturnIdentity(T insertObj);

        Task<int> InsertReturnIdentityAsync(T insertObj);

        bool InsertRange(T[] insertObjs);

        Task<bool> InsertRangeAsync(T[] insertObjs);

        bool InsertRange(List<T>[] insertObjs);

        Task<bool> InsertRangeAsync(List<T>[] insertObjs);

        bool Update(T updateObj);

        Task<bool> UpdateAsync(T updateObj);

        bool UpdateRange(T[] updateObjs);

        Task<bool> UpdateRangeAsync(T[] updateObjs);

        bool UpdateRange(List<T>[] updateObjs);

        Task<bool> UpdateRangeAsync(List<T>[] updateObjs);

        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression);

        Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression);

        bool Delete(T deleteObj);

        Task<bool> DeleteAsync(T deleteObj);

        bool Delete(T[] deleteObj);

        Task<bool> DeleteAsync(T[] deleteObj);

        bool Delete(List<T>[] deleteObj);

        Task<bool> DeleteAsync(List<T>[] deleteObj);

        bool Delete(Expression<Func<T, bool>> whereExpression);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> whereExpression);

        bool DeleteById(dynamic id);

        Task<bool> DeleteByIdAsync(dynamic id);

        bool DeleteByIds(dynamic[] ids);

        Task<bool> DeleteByIdsAsync(dynamic[] ids);

    }
}

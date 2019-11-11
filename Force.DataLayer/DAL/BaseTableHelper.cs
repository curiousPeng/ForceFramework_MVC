using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Force.DataLayer.Base
{
    public interface IColumn
    {
        string Table { get; }
        string Name { get; }
        bool IsAddEqual { get; }
        string Asc { get; }
    }

    internal class PredicateParser : ExpressionVisitor
    {
        private StringBuilder sb = new StringBuilder();
        private bool invert = false;
        private bool quote = true;
        private bool boolean = false;
        private bool invert_used = true;
        private ExpressionType? prev_op = null;

        public string Parse(Expression predicate)
        {
            this.Visit(predicate);
            return this.ToString();
        }

        public void Reset()
        {
            sb.Clear();
            invert = false;
            quote = true;
            boolean = false;
            invert_used = true;
            prev_op = null;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            sb.Append("(");
            prev_op = node.NodeType;
            var tmp = invert;
            this.Visit(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    sb.Append(invert ? " OR " : " AND ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(invert ? " AND " : " OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(node.Right))
                    {
                        sb.Append(invert ? " IS NOT " : " IS ");
                    }
                    else
                    {
                        if (boolean)
                        {
                            sb.Append(" = ");
                        }
                        else
                        {
                            sb.Append(invert ? " <> " : " = ");
                        }
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(node.Right))
                    {
                        sb.Append(invert ? " IS " : " IS NOT ");
                    }
                    else
                    {
                        if (boolean)
                        {
                            sb.Append(" = ");
                            invert = invert ^ true;
                        }
                        else
                        {
                            sb.Append(invert ? " = " : " != ");
                        }
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(invert ? " >= " : " < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(invert ? " > " : " <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(invert ? " <= " : " > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(invert ? " < " : " >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", node.NodeType));
            }

            this.Visit(node.Right);
            invert = tmp;
            sb.Append(")");
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Not:
                    var tmp = invert;
                    invert = invert ^ true;
                    this.Visit(node.Operand);
                    if (!invert_used)
                    {
                        invert = true;
                    }
                    else
                    {
                        invert = tmp;
                    }
                    break;
                case ExpressionType.Convert:
                    this.Visit(node.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", node.NodeType));
            }

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)node.Value) ^ invert ? 1 : 0);
                        break;

                    case TypeCode.String:
                        if (quote) sb.Append("'");
                        sb.Append(node.Value);
                        if (quote) sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(node.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", node.Value));

                    default:
                        sb.Append(node.Value);
                        break;
                }
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            // 访问静态字段或属性
            if (node.Expression == null)
            {
                var value = ((FieldInfo)node.Member).GetValue(null);
                sb.Append(value);
            }
            else
            {
                // 闭包带进来的变量是生成类型的一个Field
                switch (node.Expression.NodeType)
                {
                    case ExpressionType.Constant:
                        var container = ((ConstantExpression)node.Expression).Value;
                        var value = ((FieldInfo)node.Member).GetValue(container);
                        var coll = value as IList;
                        if (coll != null)
                        {
                            for (int i = 0; i < coll.Count; i++)
                            {
                                var item = coll[i];
                                if (i == coll.Count - 1)
                                {
                                    if (item is string)
                                    {
                                        sb.Append($"'{item}'");
                                    }
                                    else
                                    {
                                        sb.Append($"{item}");
                                    }
                                }
                                else
                                {
                                    if (item is string)
                                    {
                                        sb.Append($"'{item}', ");
                                    }
                                    else
                                    {
                                        sb.Append($"{item}, ");
                                    }
                                }
                            }
                        }
                        else
                        {
                            switch (Type.GetTypeCode(value.GetType()))
                            {
                                case TypeCode.Boolean:
                                    sb.Append(((bool)value) ^ invert ? 1 : 0);
                                    break;

                                case TypeCode.String:
                                    if (quote) sb.Append("'");
                                    sb.Append(value);
                                    if (quote) sb.Append("'");
                                    break;

                                case TypeCode.DateTime:
                                    sb.Append("'");
                                    sb.Append(value);
                                    sb.Append("'");
                                    break;

                                case TypeCode.Object:
                                    throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));

                                default:
                                    sb.Append(value);
                                    break;
                            }
                        }
                        break;
                    case ExpressionType.Parameter:
                        sb.Append($"[{node.Expression.Type.Name}].[{node.Member.Name}]");
                        if (node.Type == typeof(bool))
                        {
                            boolean = true;
                            if (!prev_op.HasValue || (prev_op != ExpressionType.Equal && prev_op != ExpressionType.NotEqual))
                            {
                                sb.Append(invert ? " = 0" : " = 1");
                                invert_used = true;
                            }
                            else
                            {
                                invert_used = !invert;
                            }
                        }
                        break;
                    case ExpressionType.MemberAccess:
                        this.Visit(Evaluator.PartialEval(node));
                        break;
                }
            }

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method == typeof(string).GetMethod("Contains", new[] { typeof(string) }))
            {
                sb.Append("(");
                this.Visit(node.Object);
                sb.Append(invert ? " NOT LIKE '%" : " LIKE '%");
                quote = false;
                this.Visit(node.Arguments[0]);
                quote = true;
                sb.Append("%')");
                return node;
            }
            if (node.Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) }))
            {
                sb.Append("(");
                this.Visit(node.Object);
                sb.Append(invert ? " NOT LIKE '" : " LIKE '");
                quote = false;
                this.Visit(node.Arguments[0]);
                quote = true;
                sb.Append("%')");
                return node;
            }
            if (node.Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) }))
            {
                sb.Append("(");
                this.Visit(node.Object);
                sb.Append(invert ? " NOT LIKE '%" : " LIKE '%");
                quote = false;
                this.Visit(node.Arguments[0]);
                quote = true;
                sb.Append("')");
                return node;
            }

            // 注意区分两种contains方法的方式，一个是在对象上list.contains，一个是在string上string.contains
            if (node.Method.Name == "Contains")
            {
                Expression collection;
                Expression property;
                if (node.Method.IsDefined(typeof(ExtensionAttribute)) && node.Arguments.Count == 2) // 支持直接调用扩展方法的形式
                {
                    collection = node.Arguments[0];
                    property = node.Arguments[1];
                }
                else if (!node.Method.IsDefined(typeof(ExtensionAttribute)) && node.Arguments.Count == 1)
                {
                    collection = node.Object;
                    property = node.Arguments[0];
                }
                else
                {
                    throw new Exception("Unsupported method call: " + node.Method.Name);
                }
                sb.Append("(");
                this.Visit(property);
                sb.Append(invert ? " NOT IN (" : " IN (");
                this.Visit(collection);
                sb.Append("))");
            }
            else
            {
                throw new Exception("Unsupported method call: " + node.Method.Name);
            }

            return node;
        }

        private bool IsNullConstant(Expression expr)
        {
            return (expr.NodeType == ExpressionType.Constant && ((ConstantExpression)expr).Value == null);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
    #region 处理引用类型值类
    internal static class Evaluator
    {
        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
        }

        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }

        /// <summary>
        /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
        /// </summary>
        class SubtreeEvaluator : ExpressionVisitor
        {
            HashSet<Expression> candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return this.Visit(exp);
            }

            public override Expression Visit(Expression exp)
            {
                if (exp == null)
                {
                    return null;
                }
                if (this.candidates.Contains(exp))
                {
                    return this.Evaluate(exp);
                }
                return base.Visit(exp);
            }

            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }
                LambdaExpression lambda = Expression.Lambda(e);
                Delegate fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }
        }

        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        class Nominator : ExpressionVisitor
        {
            Func<Expression, bool> fnCanBeEvaluated;
            HashSet<Expression> candidates;
            bool cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                this.candidates = new HashSet<Expression>();
                this.Visit(expression);
                return this.candidates;
            }

            public override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                    this.cannotBeEvaluated = false;
                    base.Visit(expression);
                    if (!this.cannotBeEvaluated)
                    {
                        if (this.fnCanBeEvaluated(expression))
                        {
                            this.candidates.Add(expression);
                        }
                        else
                        {
                            this.cannotBeEvaluated = true;
                        }
                    }
                    this.cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return expression;
            }
        }
    }
    #endregion
    public abstract class BaseTableHelper
    {
        public static string ConnectionString { get; }

        static BaseTableHelper()
        {
            // 添加json配置文件路径
#if LOCAL
            var builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.Local.json");
#elif DEBUG
            var builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.Development.json");
#else
            var builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json");
#endif
            // 创建配置根对象
            var configurationRoot = builder.Build();
            ConnectionString = configurationRoot.GetSection("DbConnect").Value;
        }

        protected static SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = ConnectionString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };
                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        protected static PageDataView<T> Paged<T>(
            string tableName,
            string where,
            string orderBy,
            string columns,
            int pageSize,
            int currentPage)
        {
            var result = new PageDataView<T>();
            var count_sql = string.Format("SELECT COUNT(1) FROM {0}", tableName);
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "id desc";
            }
            if (!string.IsNullOrWhiteSpace(where))
            {
                if (where.ToLower().Contains("where"))
                {
                    throw new ArgumentException("where子句不需要带where关键字");
                }
                where = " WHERE " + where;
            }

            var sql = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS Row, {0} FROM {2} {3}) AS Paged ", columns, orderBy, tableName, where);
            var pageStart = (currentPage - 1) * pageSize;
            sql += string.Format(" WHERE Row >{0} AND Row <={1}", pageStart, pageStart + pageSize);
            count_sql += where;
            using (var conn = GetOpenConnection())
            {
                result.TotalRecords = conn.ExecuteScalar<int>(count_sql);
                result.TotalPages = result.TotalRecords / pageSize;
                if (result.TotalRecords % pageSize > 0)
                    result.TotalPages += 1;
                var list = conn.Query<T>(sql);
                result.Items = list.Count() == 0 ? (new List<T>()) : list.ToList();
            }

            return result;
        }
    }
}

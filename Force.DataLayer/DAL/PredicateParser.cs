﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataLayer.Base
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
                var type = node.Value.GetType();
                switch (Type.GetTypeCode(type))
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
                        if (type.Name.StartsWith("List"))
                        {
                            IList list = node.Value as IList;
                            for(var i =0;i< list.Count; i++)
                            {
                                if (type.GenericTypeArguments[0].Name.ToString().StartsWith("Int"))
                                {
                                    sb.Append("");
                                    sb.Append(list[i]);
                                    sb.Append(",");
                                }
                                else
                                {
                                    sb.Append("'");
                                    sb.Append(list[i]);
                                    sb.Append("',");
                                }
                            }
                            sb.Remove(sb.ToString().LastIndexOf(','), 1);
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", node.Value));
                        }
                        break;
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
            if(node.Method == typeof(string).GetMethod("Equals", new[] { typeof(string) }))
            {
                sb.Append("(");
                this.Visit(node.Object);
                sb.Append(invert ? " <> '": " = '");
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
}
